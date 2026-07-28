# Phase 1 회고 — 하루 루프 FSM

Phase 1(이슈 #18~#26)에서 만든 것과 그 과정에서 굳어진 설계 방향을 짧게 남깁니다.

## 만든 것

- `GamePhase` — `MorningReport → Work → DayEnd → (다음날) MorningReport` 3페이즈 enum (#19)
- `IDayPhaseHandler` + `MorningReportHandler`/`WorkPhaseHandler`/`DayEndHandler` + `DayLoopFSM` (#20)
- `ActionPointBudget` — 행동력 자원, `Spend`/`Refill`/`CanSpend` (#21, #22)
- `DayCounter` — 경과일 카운트 + 년/월/일 달력 환산 (#23)
- `GameSimulator` + `ISimulationLogger`/`SimulationLog` — Unity 없이 도는 헤드리스 진입점 (#24)
- `DayLoopDebugPanel` + `UnityConsoleSimulationLogger` — 임시 UI로 눈으로 확인 (#25)

## 굳어진 설계 방향

- **핸들러당 하나의 책임.** 스위치문 기반 FSM 대신 페이즈별 클래스(`IDayPhaseHandler` 구현체)로 나눴습니다. `DayLoopFSM`은 순서 관리와 `Enter/Tick/Exit/CanTransitionToNext` 호출만 하고, 각 페이즈의 전환 조건은 해당 핸들러 안에만 존재합니다.
- **자원은 그 자원을 쓰는 핸들러가 소유.** `ActionPointBudget`은 `WorkPhaseHandler`가, `DayCounter`는 `DayEndHandler`가 들고 있습니다. `DayLoopFSM`은 행동력도 날짜도 모릅니다 — 나중에 자원이 늘어나도 FSM은 손댈 필요가 없습니다.
- **예상 가능한 실패는 예외가 아니라 반환값으로.** `ActionPointBudget.Spend()`는 잔량 부족·잘못된 값 같은 "일어날 수 있는" 실패를 `SpendResult`로 돌려줍니다. 예외는 진짜 프로그래밍 오류(정의되지 않은 `GamePhase` 등)에만 씁니다.
- **헤드리스가 먼저, UI는 그 위에 얇게.** `GameSimulator` 하나로 Unity 없이 하루 루프 전체가 돌아가고, `DayLoopDebugPanel`은 그 위에서 클릭을 `GameSimulator` 메서드 호출로, 상태를 텍스트로 바꾸는 것 말고는 아무 로직도 갖지 않습니다.

## 알려진 임시 처리 (Phase 2+에서 정리 예정)

- `GameSimulator.Step()`/`RunDays()`가 Work 페이즈에서 매번 행동력 1을 소비하는 건 실제 운영 결정(수업 배정 등)이 아직 없어서 넣은 최소 정책입니다. 실제 액션이 생기면 이 정책부터 교체해야 합니다.
- `DayEndHandler`는 정산 로직이 없어 진입 즉시 전환 가능 상태입니다.
- `DayLoopDebugPanel`은 이름 그대로 임시 UI입니다. Phase 2에서 UI 아키텍처가 확정되면 교체됩니다.

## 다음

Phase 2 — 학생 데이터(SO 10종) + 입학 심사, UI 아키텍처 확정.
