# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Magic Academy — a Unity 6 desk-style 2D management sim (원 계획은 탑다운 확장형 경영 게임이었으나 데스크형 2D로 재설계됨; 이전 방향은 `develop_project.v1` 브랜치에 보존). Player runs a magic school from the principal's desk: admit students, assign teachers/courses, manage limited "action points" (업무 시간) per day across a 3-phase day loop, handle events, and eventually graduate students into outcomes that feed back into reputation/economy. Full design detail lives in the GitHub Wiki, not in this repo.

Read `README.md` for the full pitch, resource list, and phase roadmap. Roadmap status as of last check: Phase 0 (project setup) complete, Phase 1 (day loop FSM) close to done — the phase state machine, its action-point economy, day counter, a headless simulation entry point, and a first (temporary) Presentation debug panel all exist (see below, tracked as GitHub issues #20, #21, #23, #24, #25).

## Engine / environment

- Unity **6000.5.0f1** (Unity 6), URP 2D Renderer, uGUI, TextMeshPro.
- Windows-only target. No CI workflows in this repo — builds/tests are run manually from the Unity Editor.
- `Assets/Scripts/Core.Tests/MagicAcademy.Core.Tests.asmdef` is an Edit Mode NUnit test assembly referencing `MagicAcademy.Core` (by GUID) plus `UnityEngine.TestRunner`/`UnityEditor.TestRunner`. Run it via **Window > General > Test Runner > EditMode** in the Unity Editor, or headlessly via CLI batch mode (`Unity.exe -runTests -projectPath . -testPlatform EditMode -testResults <path>`). There is no equivalent test assembly for `Presentation` yet.
- There is no CLI build/lint command for this project — compile verification happens by opening the project in Unity Editor and checking the Console for errors.

## Architecture

Two assemblies enforce a strict domain/engine split:

- **`Assets/Scripts/Core/MagicAcademy.Core.asmdef`** (`MagicAcademy.Core` namespace) — pure C# simulation/domain logic. `noEngineReferences: true` and zero references, enforced by the asmdef itself — do not add `UnityEngine`/`UnityEditor` usings or any dependency here. This is meant to run headless (no Unity) so balance can be simulated in bulk (day totals, dropout rate, financial curves) outside the editor.
- **`Assets/Scripts/Presentation/MagicAcademy.Presentation.asmdef`** (`MagicAcademy.Presentation` namespace) — all Unity-dependent code (MonoBehaviours, UI, scene control). References Core.

Static definitions vs. runtime state is a deliberate split:
- Static/design data (students, teachers, courses, encounters) → `ScriptableObject` classes, suffixed `Def` (e.g. `StudentDef`), living under `Core`. Instances of these SOs go in `Assets/Data/<Category>/`.
- Runtime state → plain C# classes (no `Def` suffix, e.g. `StudentState`), also in `Core`. Save/load is a single JSON serialization pass over runtime state (not yet implemented).

Actual current folder layout (note: `docs/folder-structure.md` describes an aspirational `_Project/` + `ThirdParty/` wrapper that categorizes third-party assets under `Free/`/`Paid/`; in practice the repo is flatter — code lives directly under `Assets/Scripts/{Core,Presentation}`, data under `Assets/Data`, etc., with no `_Project/` prefix). When adding files, mirror what's already there rather than the doc's nested form unless asked to restructure.

Within `Core`, organize by domain concept (`DayLoop/`, and eventually `Students/`, `Teachers/`, `Courses/`, `Encounters/`, `Resources/`, `Simulation/` per the folder-structure doc). Within `Presentation`, organize by screen/UI unit (`AdmissionScreen/`, `VisitorPanel/`, `DailyReport/`, etc.) — the mapping between a Core domain and its Presentation screens is not always 1:1.

### Day loop FSM (current focus, Phase 1)

This follows a handler-per-state pattern (per issue #20), not a monolithic switch statement — deliberately so that each phase's rules stay isolated in its own class, and the FSM itself never needs to know about any individual phase's business rules (like the action-point economy).

- **`GamePhase`** (`Core/DayLoop/GamePhase.cs`) — the 3-phase enum: `MorningReport` → `Work` → `DayEnd` → (loops back to) `MorningReport`.
- **`IDayPhaseHandler`** (`Core/DayLoop/IDayPhaseHandler.cs`) — `Enter()` / `Tick()` / `Exit()` / `CanTransitionToNext()`. Every phase is a class implementing this.
  - `MorningReportHandler`, `DayEndHandler` — both automatic: `CanTransitionToNext()` always returns `true` (nothing to compute yet; `DayEndHandler` is a placeholder until actual day-end settlement logic exists). `DayEndHandler` owns a `DayCounter` (issue #23, `Core/DayLoop/DayCounter.cs`) and advances it in `Exit()`, i.e. exactly at the DayEnd→MorningReport boundary. Same pattern as `WorkPhaseHandler`/`ActionPointBudget`: the handler owns the resource, the FSM never sees it. `DayCounter.CurrentDay` (starts at 1) is the source of truth, incremented by `AdvanceDay()`; `Year`/`Month`/`Day` (a `DateTime`-style calendar breakdown, all 1-based) are computed from it, not stored separately — there's one counter, not two things to keep in sync. The calendar is a fixed fantasy scheme, `DAYS_PER_MONTH` = 30, `MONTHS_PER_YEAR` = 12 (1 year = 360 days); no leap-year-style irregularity.
  - `WorkPhaseHandler` — the only handler with real state. It owns an `ActionPointBudget` (see below), refills it on `Enter()`, and `CanTransitionToNext()` returns `true` once the budget is depleted (`Current <= 0`) *or* `RequestEarlyEnd()` has been called. This is where the "행동력 소모 시 페이즈 변경" rule actually lives — not in the FSM.
- **`DayLoopFSM`** (`Core/DayLoop/DayLoopFSM.cs`) — holds `CurrentPhase`, maps each `GamePhase` to its handler, and exposes a single `Advance()` method: it `Tick()`s the current handler, and if `CanTransitionToNext()` is now true, calls `Exit()`/`Enter()` across the phase boundary and raises `PhaseChanged`. Callers (Presentation, or the debug harness below) drive the whole loop just by calling `Advance()` repeatedly — automatic phases fall through immediately since they're ready the instant they're entered; `Work` just sits until its handler says otherwise. `DayLoopFSM` has zero knowledge of action points or any other phase-specific concern.

The action-point economy (issue #21) is a separate resource type, **`ActionPointBudget`** (`Core/Resources/ActionPointBudget.cs`) — `Current`/`Maximum`, `CanSpend(amount)`, `Spend(amount)` (returns a `SpendResult`, not an exception or a silent clamp — see `SpendResult`/`SpendFailureReason` in the same folder, reasons are `InvalidAmount` and `InsufficientActionPoints`), `Refill()`, and an `ActionPointsChanged` event. It has no idea what a `GamePhase` is; `WorkPhaseHandler` is the only thing that couples it to phase transitions. If more per-day resources show up later, follow the same shape (a standalone class under `Core/Resources/`, coupled to phase logic only via whichever handler needs it) rather than growing logic inside `DayLoopFSM` or any handler that doesn't need it.

### Headless simulation entry point (issue #24)

**`GameSimulator`** (`Core/Simulation/GameSimulator.cs`) assembles a full `DayLoopFSM` + `WorkPhaseHandler`/`ActionPointBudget` + `DayEndHandler`/`DayCounter` stack internally — it's the thing that proves "just `Core.dll`, no Unity" can run the day loop end to end, and is the shared entry point for both headless balance simulation and the Presentation debug panel below. Its public surface is split by intent, not merged into one method:
- `Advance()` — a pure FSM step (no resource side effects); this is what a "next step" UI control should call.
- `PerformAction(cost = 1)` — spends `cost` action points during `Work` (a stand-in for "did a work action" until real gameplay actions exist); no-ops (and logs) outside `Work`.
- `EndWorkEarly()` — requests an early end of `Work` regardless of remaining AP; no-ops (and logs) outside `Work`.
- `Step()` — `PerformAction(1)` + `Advance()` combined, used only by `RunDays(count)` as the minimal self-driving policy needed to complete a day with no real gameplay behind it yet. Once real Work-phase actions exist, `RunDays`'s policy is what should change — not `Advance`/`PerformAction`/`EndWorkEarly`, which are the stable primitives.

Logging never touches `UnityEngine.Debug` (can't — `Core` has no engine reference anyway) — it goes through **`ISimulationLogger.Log(string)`** (`Core/Simulation/ISimulationLogger.cs`), injected via `GameSimulator`'s constructor. **`SimulationLog`** (`Core/Simulation/SimulationLog.cs`) is the default implementation, collecting messages into an `Entries` list — useful directly in tests/headless runs.

### Presentation debug panel (issue #25)

The first real (if still temporary — "UI 아키텍처는 Phase 2에서 확정") Presentation entry point, under `Presentation/DayLoop/`:
- **`UnityConsoleSimulationLogger`** implements `ISimulationLogger` by forwarding to `Debug.Log` — the concrete logger `GameSimulator` gets wired up with here.
- **`DayLoopDebugPanel`** (`MonoBehaviour`) owns a `GameSimulator` instance, wires 3 `Button`s to `Advance()` / `PerformAction(1)` / `EndWorkEarly()`, and renders `{day}일차 · {phase} · 행동력 {points}` into one `TMP_Text` after every click. No phase or resource logic lives here — it only forwards clicks and reads state, same rule as everywhere else in this doc.
- This superseded and replaced the earlier keyboard-only harness (`DayLoopDebugTrigger`), which has been deleted.
- **The scene is not built by hand-editing `.unity` YAML** — Unity scene files are edited through the Editor (see the `.editorconfig`/naming-convention note about minimizing manual edits to Unity-generated files). `Scene_Test_DayLoop.unity` (Canvas + TMP Text + 3 Buttons + a GameObject holding `DayLoopDebugPanel` with the 4 references wired in the Inspector) needs to be created in-editor, not by this agent.
- `Presentation.asmdef` had to gain a reference to `UnityEngine.UI` (`GUID:2bafac87e7f4b9b418d9448d219b01ab`) for `Button` — it previously only referenced `Unity.TextMeshPro` and `Unity.InputSystem`.

## Conventions

Full rules: `docs/naming-convention.md` and `docs/folder-structure.md`. Key points that aren't obvious from skimming code:

- Private fields (including `[SerializeField] private`): `_camelCase`. Public members: `PascalCase`. Constants: `SCREAMING_SNAKE_CASE`. `static readonly`: `PascalCase`.
- Interfaces: `I`-prefixed (`IVisitor`). Avoid abbreviations like `Mgr`/`Ctrl` — spell out `Manager`/`Controller`. Established abbreviations (`UI`, `HP`, `AP`, `SDF`) are fine.
- Allman brace style (opening brace on its own line), 4-space indent for C#, enforced via `.editorconfig`.
- Prefabs: `PF_[Name]`; UI prefabs preferably under `Prefabs/UI/` rather than a `PF_UI_` prefix. Materials: `M_[Name]`. Shaders: `SH_[Name]` (file) with `Shader.name` = `MagicAcademy/[Category]/[Name]`. Scenes: `Scene_[Name]`, test scenes get a `Test_` sub-prefix. SO data assets: `[Type]_[Name].asset` (Type = SO class name minus `Def`/`Data` suffix, e.g. `Student_Aria.asset`).
- Git branches: `type/description` kebab-case (`feat`, `fix`, `refactor`, `chore`, `docs`, `design`), optionally with an issue number (`feature/23-admission-screen`); phase integration branches are `phase-[N]`. Commits: `type: 설명 (#이슈번호)` — this repo's actual history uses `type : 설명 #이슈번호` (space before colon, no parens around the issue number) as often as the documented format, so match whichever style is more common in recent `git log` output rather than the doc literally.
- Exceptions to naming/folder rules must be recorded in the exception tables at the bottom of the respective doc, with rationale (existing examples: `TextMesh Pro/` and `Settings/` can't be moved/renamed because they're Unity-auto-generated).
