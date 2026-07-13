# 네이밍 컨벤션

Magic Academy 프로젝트에서 사용하는 파일·폴더·코드 네이밍 규칙을 정의합니다. 새 리소스를 추가하기 전 이 문서를 확인하고, 예외가 필요할 경우 이 문서에 근거를 함께 기록합니다.

## 원칙

- **일관성이 규칙보다 중요합니다.** 규칙이 어색한 경우에도 기존 리소스와 톤을 맞춥니다.
- **약어보다 명확한 이름을 씁니다.** `Mgr`, `Ctrl` 대신 `Manager`, `Controller`를 씁니다. 다만 이미 관례가 있는 것(`UI`, `HP`, `AP`, `SDF`)은 그대로 씁니다.
- **접두사는 카테고리 구분이 필요한 경우에만 사용합니다.** 프리팹·머티리얼·씬은 접두사로 구분하고, 스크립트는 접두사 없이 이름 그 자체로 역할이 드러나게 합니다.

---

## 폴더 구조

전체 폴더 구조는 [폴더 구조 문서](./folder-structure.md)를 참고합니다. 이 문서에서는 네이밍만 다룹니다.

- **자체 제작 폴더**: PascalCase (`Scripts/`, `Prefabs/`, `Art/`)
- **하위 카테고리 폴더**: 복수형 명사 사용 (`Students/`, `Teachers/`, `Encounters/`)
- **외부 에셋 폴더**: 원본 이름 유지 (`ThirdParty/DOTween/`, `TextMesh Pro/`)

---

## C# 스크립트

### 파일명

- **PascalCase**, 확장자 `.cs`
- 파일명 = 파일에 정의된 주요 타입 이름

```
StudentData.cs         // 클래스 StudentData
DayLoopController.cs   // 클래스 DayLoopController
IVisitor.cs            // 인터페이스 IVisitor
GamePhase.cs           // enum GamePhase
```

### 클래스·구조체·인터페이스

- **클래스**: PascalCase (`StudentData`, `EncounterResolver`)
- **인터페이스**: `I` 접두사 + PascalCase (`IVisitor`, `IDayPhaseHandler`)
- **구조체**: PascalCase, 필요 시 `Data`·`Info` 접미사 (`StudentStats`, `CourseAssignmentInfo`)
- **enum**: PascalCase 단수형 (`GamePhase`, `TalentType`, `PersonalityTrait`)

### 메서드·프로퍼티·필드

- **public 메서드/프로퍼티**: PascalCase (`GetSatisfaction()`, `ActionPoints`)
- **private 필드**: `_` 접두사 + camelCase (`_actionPoints`, `_currentPhase`)
- **`[SerializeField] private` 필드**: `_` 접두사 + camelCase (`_studentCard`, `_visitorSlot`)
- **로컬 변수·매개변수**: camelCase (`studentIndex`, `visitorQueue`)
- **상수**: SCREAMING_SNAKE_CASE (`MAX_ACTION_POINTS`, `DEFAULT_DAY_DURATION`)
- **static readonly**: PascalCase (`DefaultTeacher`, `EmptyStudentSet`)

```csharp
public class DayLoopController : MonoBehaviour
{
    private const int MAX_ACTION_POINTS = 6;

    [SerializeField] private DayPhase _currentPhase;
    private int _actionPoints;

    public int ActionPoints => _actionPoints;

    public void ConsumeActionPoints(int amount)
    {
        _actionPoints -= amount;
    }
}
```

### 네임스페이스

- 최상위: `MagicAcademy`
- 어셈블리별로 분리: `MagicAcademy.Core`, `MagicAcademy.Presentation`
- 하위 카테고리 추가 가능: `MagicAcademy.Core.Students`, `MagicAcademy.Presentation.UI`

```csharp
namespace MagicAcademy.Core.Students
{
    public class StudentState { ... }
}
```

---

## ScriptableObject

### 클래스명

- **PascalCase**, `Def`(정의) 또는 `Data` 접미사
- 정적 정의는 `Def`, 런타임 상태와 대칭되는 경우 명확히 구분

```csharp
public class StudentDef : ScriptableObject { ... }     // 정적 정의 (SO)
public class StudentState { ... }                       // 런타임 상태 (plain C#)
```

### 에셋 파일명

- **`[Type]_[Name].asset`** 형식
- Type은 SO 클래스에서 `Def`·`Data` 접미사를 뺀 이름
- Name은 PascalCase, 공백 없음

```
Student_Aria.asset
Student_Marcus.asset
Teacher_ProfessorHelena.asset
Course_FireBasic.asset
Course_HealingAdvanced.asset
Encounter_NightRaid.asset
```

카테고리별 하위 폴더에 넣으면 접두사 없이 이름만 써도 되지만, **에셋을 다른 폴더에서 참조할 때 혼란을 막기 위해 접두사를 유지합니다.**

---

## 프리팹

- **`PF_[Name]`** 형식, PascalCase
- 용도에 따라 하위 폴더로 분류

```
PF_StudentCard.prefab
PF_VisitorSlot.prefab
PF_DocumentItem.prefab
PF_PrincipalDesk.prefab
```

### UI 프리팹

- UI 프리팹은 `PF_UI_[Name]` 또는 `Prefabs/UI/` 폴더에 배치
- 폴더로 구분하는 쪽을 권장 (파일명이 짧아짐)

```
Prefabs/
├── UI/
│   ├── PF_StudentCard.prefab
│   ├── PF_VisitorSlot.prefab
│   └── PF_DailyReportPanel.prefab
└── Scene/
    └── PF_PrincipalDesk.prefab
```

---

## 씬

- **`Scene_[Name]`** 형식, PascalCase

```
Scene_Main.unity
Scene_PrincipalOffice.unity
Scene_Test_DayLoop.unity      // 테스트/디버그 씬은 Test_ 접두사 추가
```

---

## 아트 리소스

### 스프라이트

- **PascalCase**, 카테고리별 하위 폴더 사용
- 필요 시 상태·방향 접미사 사용

```
Art/Sprites/
├── Characters/
│   ├── Aria_Idle.png
│   ├── Aria_Talk.png
│   └── ProfessorHelena_Idle.png
├── UI/
│   ├── Icon_ActionPoint.png
│   ├── Icon_Gold.png
│   └── Button_Approve.png
└── Desk/
    ├── Desk_Base.png
    └── Window_Day.png
```

### 머티리얼

- **`M_[Name]`** 형식

```
M_DeskWood.mat
M_WindowGlass.mat
```

### 셰이더

- **`SH_[Name]`** 형식 (파일명)
- Shader 이름 문자열은 `MagicAcademy/[Category]/[Name]` (Inspector에 표시되는 경로)

```
SH_WindowLight.shader     // 파일명
"MagicAcademy/2D/WindowLight"  // Shader.name
```

---

## 폰트 (TextMeshPro)

### 원본 폰트

- 폰트 파일명은 원본 유지 (라이선스 추적 편의)
- `_Project/Art/Fonts/Source/`에 배치

```
Pretendard-Regular.ttf
Pretendard-Bold.ttf
```

### SDF 에셋

- **`Font_[Name]_SDF`** 형식
- 웨이트 구분 필요 시 접미사 추가

```
Font_Pretendard_Regular_SDF.asset
Font_Pretendard_Bold_SDF.asset
```

### 머티리얼 프리셋

- **`Font_[Name]_[Preset]`** 형식

```
Font_Pretendard_Regular_Outline.asset
Font_Pretendard_Bold_Shadow.asset
```

---

## 오디오

- **PascalCase**, 카테고리별 하위 폴더

```
Art/Audio/
├── BGM/
│   ├── BGM_MainTheme.wav
│   └── BGM_NightAmbient.wav
├── SFX/
│   ├── SFX_DocumentStamp.wav
│   ├── SFX_VisitorKnock.wav
│   └── SFX_DayEnd.wav
└── Voice/
    └── (필요 시)
```

---

## 애니메이션

- **PascalCase**, 대상 카테고리별 폴더

```
Art/Animations/
├── Characters/
│   ├── Aria_Idle.anim
│   └── Aria_Talk.anim
└── UI/
    ├── Panel_FadeIn.anim
    └── Button_Press.anim
```

**AnimatorController**: `AC_[Name]` 형식

```
AC_Aria.controller
AC_UIPanel.controller
```

---

## 데이터 파일

### JSON 세이브 파일

- **snake_case**, 확장자 `.json`
- 저장 위치는 `Application.persistentDataPath`

```
save_slot_1.json
save_slot_2.json
autosave.json
```

### 로컬라이제이션 (예정)

- **`[locale]_[category].csv`** 또는 `.json`

```
ko_KR_ui.csv
en_US_ui.csv
```

---

## Git

### 브랜치

- **type/설명** 형식, kebab-case
- 타입: `feature`, `fix`, `refactor`, `chore`, `docs`, `design`
- Phase 통합 브랜치: `phase-[N]`

```
develop_project.v2
phase-0
feature/day-loop-fsm
feature/23-admission-screen        # 이슈 번호 포함 가능
fix/action-point-negative
refactor/split-core-assembly
```

### 커밋 메시지

- **type: 설명 (#이슈번호)** 형식
- 타입: `feat`, `fix`, `refactor`, `chore`, `docs`, `design`

```
feat: implement day loop FSM (#12)
fix: prevent negative action points on refund (#15)
refactor: split core into separate asmdef (#5)
chore: add TMP Korean font SDF (#6)
docs: add naming convention document (#7)
```

---

## 예외와 근거 기록

이 문서의 규칙과 다른 네이밍이 필요한 경우, 아래 표에 근거와 함께 기록합니다.

| 리소스 | 예외 이유 | 결정 시점 |
|--------|-----------|-----------|
| `TextMesh Pro/` | Unity가 자동 생성하는 폴더로 이름 변경 시 참조 깨짐 | Phase 0 |
| `Sirenix/` (Odin) | 에셋 내부에 경로가 하드코딩되어 있음 | (도입 시 기록) |

---

## 관련 문서

- [폴더 구조](./folder-structure.md) *(작성 예정)*
- [유료 에셋 관리 정책](./third-party-asset-policy.md) *(작성 예정)*
- [Git 브랜치 및 커밋 규칙](./git-convention.md) *(작성 예정)*
