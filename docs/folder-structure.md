# 폴더 구조

Magic Academy 프로젝트의 폴더 구조와 배치 원칙을 정의합니다. 새 리소스를 추가하기 전 이 문서를 확인하고, 예외가 필요할 경우 문서 하단의 예외 표에 근거를 함께 기록합니다.

## 원칙

- **자체 코드는 `_Project/`에, 외부 에셋은 `ThirdParty/`에 격리합니다.** 두 영역이 섞이면 Unity 업데이트나 에셋 재임포트 시 참조가 꼬입니다.
- **폴더 이름으로 카테고리를 표현합니다.** 접두사·접미사에 의존하지 않고 폴더 계층으로 리소스의 소속을 명확히 합니다.
- **자동 생성 폴더는 이동하지 않습니다.** Unity가 만드는 `TextMesh Pro/`, `Settings/` 등은 원위치를 유지합니다.

---

## 저장소 루트 구조

```
Magic-Academy/                        ← Git 저장소 루트
├── MagicAcademy/                     ← Unity 프로젝트 루트
│   ├── Assets/
│   ├── Packages/
│   ├── ProjectSettings/
│   └── UserSettings/                 ← .gitignore 처리
├── docs/                             ← 개발 문서
│   ├── naming-convention.md
│   └── folder-structure.md           ← 이 문서
├── .github/                          ← GitHub 관련 설정 (필요 시)
├── .gitignore
├── .gitattributes
└── README.md
```

Unity 프로젝트를 저장소 루트에 두지 않고 `MagicAcademy/` 하위 폴더에 둔 이유는 `docs/`, `.github/` 등 프로젝트 외부 문서를 저장소 루트에 나란히 배치하기 위함입니다.

---

## Assets 폴더 구조

```
Assets/
├── _Project/                         ← 자체 제작 코드/에셋
├── ThirdParty/                       ← 외부 에셋 격리
├── Plugins/                          ← 네이티브 플러그인
├── TextMesh Pro/                     ← TMP 자동 생성 (이동 금지)
└── Settings/                         ← URP 등 프로젝트 세팅 (자동 생성)
```

### `_Project/` — 자체 제작 영역

언더스코어(`_`) 접두사로 알파벳 정렬 최상단에 고정합니다. 유료 에셋을 임포트해도 이 폴더가 항상 상단에 있어 개발 시 왕복이 줄어듭니다.

```
_Project/
├── Scripts/
│   ├── Core/                         ← MagicAcademy.Core.asmdef (순수 C#)
│   │   ├── Students/
│   │   ├── Teachers/
│   │   ├── Courses/
│   │   ├── Encounters/
│   │   ├── DayLoop/
│   │   ├── Resources/                ← 골드·평판·재료·행동력
│   │   └── Simulation/               ← 헤드리스 시뮬레이터
│   └── Presentation/                 ← MagicAcademy.Presentation.asmdef
│       ├── UI/
│       │   ├── AdmissionScreen/
│       │   ├── VisitorPanel/
│       │   └── DailyReport/
│       ├── Scene/                    ← 씬 컨트롤러
│       ├── Input/
│       └── Bootstrap/                ← 시작 시 초기화
├── Data/                             ← ScriptableObject 인스턴스
│   ├── Students/
│   ├── Teachers/
│   ├── Courses/
│   ├── Encounters/
│   ├── Organizations/
│   └── PrincipalSkills/
├── Prefabs/
│   ├── UI/
│   │   ├── Cards/                    ← 학생 카드, 방문자 카드
│   │   ├── Panels/                   ← 큰 UI 패널
│   │   └── Elements/                 ← 버튼, 슬롯 등 부품
│   └── Scene/                        ← 씬에 배치하는 프리팹
├── Art/
│   ├── Sprites/
│   │   ├── Characters/
│   │   ├── UI/
│   │   ├── Desk/                     ← 교장실 배경 요소
│   │   └── Icons/
│   ├── UI/                           ← UI 전용 이미지 (프리팹 아님)
│   ├── Fonts/                        ← 커스텀 TMP SDF 에셋
│   │   └── Source/                   ← 폰트 원본 (.ttf, .otf)
│   ├── Shaders/                      ← .shader / .shadergraph
│   ├── Animations/
│   │   ├── Characters/
│   │   └── UI/
│   └── Audio/
│       ├── BGM/
│       ├── SFX/
│       └── Voice/                    ← 사용 시
├── Scenes/
│   ├── Main.unity
│   ├── PrincipalOffice.unity
│   └── Test/                         ← 개발용 테스트 씬 (배포 제외)
└── Settings/                         ← 자체 설정 파일
    ├── Input/                        ← Input Action Assets
    └── Rendering/                    ← 커스텀 렌더 데이터
```

#### Scripts 폴더 규칙

- **Core**는 도메인 개념별로 폴더를 나눕니다 (학생, 교사, 수업, 사건, 하루 루프 등).
- **Presentation**은 화면·UI 단위로 나눕니다 (입학 심사, 방문자 패널, 일일 보고 등).
- **Core 폴더 내부는 Unity 참조 없음**을 asmdef가 강제합니다.
- Core와 Presentation의 대응이 항상 1:1은 아닙니다. 하나의 Core 도메인이 여러 UI에 걸릴 수 있고, 그 반대도 가능합니다.

#### Data 폴더 규칙

- ScriptableObject 인스턴스만 배치합니다. 정의(SO 클래스)는 `Scripts/Core/` 아래에 둡니다.
- 카테고리별 하위 폴더는 SO 클래스의 카테고리와 일치시킵니다.

#### Prefabs 폴더 규칙

- **Cards / Panels / Elements**로 세분화해 큰 UI(Panel)와 작은 UI 조각(Element)을 분리합니다.
- **Scene** 폴더는 씬에 직접 배치하는 프리팹(교장 데스크 등)을 담습니다.

#### Art 폴더 규칙

- 리소스 종류별로 최상위 폴더를 나눕니다 (Sprites, Fonts, Shaders, Animations, Audio).
- 하위는 카테고리별 (Characters, UI, Desk 등).
- **폰트 원본**(`.ttf`, `.otf`)과 **SDF 에셋**은 분리합니다. 원본은 `Fonts/Source/`, SDF는 `Fonts/` 직속.

### `ThirdParty/` — 외부 에셋 격리

유료 및 무료 외부 에셋을 임포트한 뒤 이 폴더로 이동합니다.

```
ThirdParty/
├── DOTween/                          ← 예시
├── UniTask/
└── (기타 에셋)
```

**이동 워크플로우:**

1. Asset Store에서 임포트 (기본 위치 `Assets/`)
2. 임포트 직후 커밋 (`chore: import [asset-name]`)
3. 에셋을 `ThirdParty/<에셋이름>/`으로 이동
4. 컴파일 에러 확인
5. 이동 후 커밋 (`chore: move [asset-name] to ThirdParty`)

**이동 예외:**

- **TextMeshPro** — Unity가 자동 생성, 이동 시 참조 깨짐
- **Odin Inspector (Sirenix)** — 폴더 이름이 하드코딩되어 있음
- **UniTask** — `Plugins/` 아래가 관례
- **셰이더 include 경로가 하드코딩된 에셋** — 원위치 유지 또는 코드 수정 후 이동

자세한 정책은 [유료 에셋 관리 정책](./third-party-asset-policy.md) *(작성 예정)* 문서를 참고합니다.

### `Plugins/` — 네이티브 플러그인 및 관례상 위치 에셋

- 네이티브 플러그인 (`.dll`, `.so`, `.dylib`)
- UniTask 등 `Plugins/` 아래가 관례인 에셋

### `TextMesh Pro/` — TMP 자동 생성

- Unity의 TMP Essentials가 자동으로 생성합니다.
- **이동·이름 변경 금지.**
- 자체 제작 폰트 SDF 에셋은 여기 넣지 말고 `_Project/Art/Fonts/`에 배치합니다.

### `Settings/` — Unity 자동 생성 세팅

- URP Renderer, Volume Profile 등 Unity가 URP 템플릿과 함께 만드는 폴더.
- 자체 설정은 `_Project/Settings/`에 별도로 둡니다.

---

## Packages 폴더

- Unity 패키지 매니저가 관리합니다. 수동으로 편집하지 않습니다.
- `manifest.json`, `packages-lock.json`은 커밋 대상입니다.

---

## ProjectSettings / UserSettings

- **ProjectSettings/**: 프로젝트 전반 설정 (Player, Graphics, Input 등). 커밋 대상.
- **UserSettings/**: 사용자별 로컬 설정. `.gitignore` 대상.

---

## docs/ 폴더

저장소 루트의 `docs/` 폴더는 개발 문서를 담습니다. 위키의 시스템 설계 문서와는 성격이 다릅니다.

- **위키**: 게임 시스템 설계 (학생 시스템, 사건 시스템 등)
- **docs/**: 개발 규칙과 프로세스 (네이밍, 폴더 구조, Git 규칙 등)

```
docs/
├── naming-convention.md
├── folder-structure.md               ← 이 문서
├── third-party-asset-policy.md       (작성 예정)
└── git-convention.md                 (작성 예정)
```

---

## 커밋되지 않는 폴더 (.gitignore)

다음 폴더는 `.gitignore`로 제외합니다:

- `Library/` — Unity 캐시
- `Temp/`, `Obj/`, `Logs/` — 임시 파일
- `Build/`, `Builds/` — 빌드 산출물
- `UserSettings/` — 사용자별 설정
- `.vs/`, `.idea/`, `*.csproj`, `*.sln` — IDE 자동 생성 (필요 시 유지)

---

## 예외와 근거 기록

이 문서의 구조와 다른 배치가 필요한 경우, 아래 표에 근거와 함께 기록합니다.

| 리소스 | 예외 이유 | 결정 시점 |
|--------|-----------|-----------|
| `Assets/TextMesh Pro/` | Unity가 자동 생성하는 폴더로 이동 시 참조 깨짐 | Phase 0 |
| `Assets/Settings/` | URP 템플릿이 자동 생성. 자체 설정은 `_Project/Settings/`로 분리 | Phase 0 |

---

## 관련 문서

- [네이밍 컨벤션](./naming-convention.md)
- [유료 에셋 관리 정책](./third-party-asset-policy.md) *(작성 예정)*
- [Git 브랜치 및 커밋 규칙](./git-convention.md) *(작성 예정)*
