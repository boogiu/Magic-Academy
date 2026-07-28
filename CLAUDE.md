# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Magic Academy — a Unity 6 desk-style 2D management sim (원 계획은 탑다운 확장형 경영 게임이었으나 데스크형 2D로 재설계됨; 이전 방향은 `develop_project.v1` 브랜치에 보존). Player runs a magic school from the principal's desk: admit students, assign teachers/courses, manage limited "action points" (업무 시간) per day across a 3-phase day loop, handle events, and eventually graduate students into outcomes that feed back into reputation/economy. Full design detail lives in the GitHub Wiki, not in this repo.

Read `README.md` for the full pitch, resource list, and phase roadmap. Roadmap status as of last check: Phase 0 (project setup) complete, Phase 1 (day loop FSM) in progress — only the `GamePhase` enum exists so far, no controller/driver yet.

## Engine / environment

- Unity **6000.5.0f1** (Unity 6), URP 2D Renderer, uGUI, TextMeshPro.
- Windows-only target. No CI workflows in this repo — builds/tests are run manually from the Unity Editor.
- `com.unity.test-framework` is installed but **no test assembly exists yet** — there is no `*.Tests.asmdef` anywhere in `Assets/`. Don't assume a test command exists; if asked to add tests, a new Test Assembly (edit-mode, referencing `MagicAcademy.Core`) needs to be created first.
- There is no CLI build/lint/test command for this project — verification happens by opening the project in Unity Editor and checking the Console for compile errors, or via Unity's own Test Runner window once test asmdefs exist.

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

`Assets/Scripts/Core/DayLoop/GamePhase.cs` defines the 3-phase day cycle: `MorningReport` (자동, 전날 결과/사건 보고) → `Work` (플레이어 입력, 행동력 소비) → `DayEnd` (자동, 정산). `GamePhaseExtension.IsAutomatic(GamePhase)` reports whether a phase advances on its own vs. waits for player input (`Work` is the only non-automatic phase).

No FSM driver/controller consuming this enum exists yet — that (plus the action-point economy, "6 행동력" per day per the roadmap) is the next piece of Phase 1.

## Conventions

Full rules: `docs/naming-convention.md` and `docs/folder-structure.md`. Key points that aren't obvious from skimming code:

- Private fields (including `[SerializeField] private`): `_camelCase`. Public members: `PascalCase`. Constants: `SCREAMING_SNAKE_CASE`. `static readonly`: `PascalCase`.
- Interfaces: `I`-prefixed (`IVisitor`). Avoid abbreviations like `Mgr`/`Ctrl` — spell out `Manager`/`Controller`. Established abbreviations (`UI`, `HP`, `AP`, `SDF`) are fine.
- Allman brace style (opening brace on its own line), 4-space indent for C#, enforced via `.editorconfig`.
- Prefabs: `PF_[Name]`; UI prefabs preferably under `Prefabs/UI/` rather than a `PF_UI_` prefix. Materials: `M_[Name]`. Shaders: `SH_[Name]` (file) with `Shader.name` = `MagicAcademy/[Category]/[Name]`. Scenes: `Scene_[Name]`, test scenes get a `Test_` sub-prefix. SO data assets: `[Type]_[Name].asset` (Type = SO class name minus `Def`/`Data` suffix, e.g. `Student_Aria.asset`).
- Git branches: `type/description` kebab-case (`feat`, `fix`, `refactor`, `chore`, `docs`, `design`), optionally with an issue number (`feature/23-admission-screen`); phase integration branches are `phase-[N]`. Commits: `type: 설명 (#이슈번호)` — this repo's actual history uses `type : 설명 #이슈번호` (space before colon, no parens around the issue number) as often as the documented format, so match whichever style is more common in recent `git log` output rather than the doc literally.
- Exceptions to naming/folder rules must be recorded in the exception tables at the bottom of the respective doc, with rationale (existing examples: `TextMesh Pro/` and `Settings/` can't be moved/renamed because they're Unity-auto-generated).
