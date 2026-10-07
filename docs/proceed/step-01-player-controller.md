# 📋 [Step 01] 플레이어 컨트롤러 & 무적 대시 프로토타입

* **상태:** 🟡 진행 중 (In Progress)
* **담당:** Antigravity AI Agent & 개발자
* **다음 단계:** 검증 완료 후 `docs/history/step-01-player-controller.md`로 이동

---

## 1. 🎯 목표 (Goal)

인스펙터 의존 없이 빈 씬에서도 즉시 실행 가능한 **탑다운 플레이어 컨트롤러의 기본 이동 및 회피 메커니즘**을 구현합니다.

1. **8방향 탑다운 이동 (WASD):** 가감속 및 대각선 속도 정규화.
2. **마우스 커서 방향 회전:** 카메라 레이캐스트를 통한 360도 마우스 지향 회전.
3. **무적 대시 (Space):** 대시 순간 속도 증가, 쿨다운 타이머, 무적 상태(`IsInvincible`) 플래그 제공.
4. **Code-First 부트스트랩 (Fallback):** 3D 에셋이나 프리팹이 없어도 Primitive Capsule과 테스트 바닥을 코드로 동적 생성해 즉시 조작 가능.

---

## 2. 📁 생성 대상 파일 목록 (SRP & 250줄 이하 준수)

```text
Assets/_Scripts/Player/
├── PlayerStats.cs          # 이동속도, 대시 쿨다운, 무적 시간 등 데이터
├── PlayerInputHandler.cs   # WASD 및 마우스 입력 수집
├── PlayerMovement.cs       # Rigidbody/Transform 기반 이동 및 조준 회전
├── PlayerDash.cs           # 대시 물리 및 무적 판정 타이머
├── PlayerController.cs     # 플레이어 메인 파사드 (컴포넌트 조율)
└── PlayerBootstrap.cs      # 빈 씬에서 Capsule/바닥 자동 생성 및 셋업 (테스트용)
```

---

## 3. ✅ 상세 작업 체크리스트 (Checklist)

- [ ] **[Task 1.1] PlayerStats.cs 작성**
  - 이동 속도, 대시 거리, 대시 지속 시간, 쿨다운 수치 정의
- [ ] **[Task 1.2] PlayerInputHandler.cs 작성**
  - Input.GetAxisRaw 수집 및 마우스 화면 좌표 전달
- [ ] **[Task 1.3] PlayerMovement.cs 작성**
  - 물리/이동 계산, 카메라 평면(Plane) 교차점을 통한 360도 회전
- [ ] **[Task 1.4] PlayerDash.cs 작성**
  - Space 입력 시 순간 가속, 대시 중 무적(`IsInvincible = true`) 처리, 쿨타임 관리
- [ ] **[Task 1.5] PlayerController.cs 작성**
  - 컴포넌트 자동 바인딩(`RequireComponent`), 상태 이벤트 연동
- [ ] **[Task 1.6] PlayerBootstrap.cs 작성**
  - 빈 씬 실행 시 테스트 바닥(Plane) 및 캡슐 플레이어 자동 인스턴스화
- [ ] **[Task 1.7] 빌드 및 인게임 테스트 검증**
  - 이동 부드러움, 마우스 에임 정확도, 대시 쿨타임 및 무적 상태 로그 확인

---

## 4. ⏪ 롤백 전략 (Rollback Strategy)

작업 도중 오류가 발생하거나 이전 상태로 되돌리고 싶을 때 사용하는 롤백 절차입니다.

### ① 변경점만 취소하고 깨끗이 되돌리기 (작업 중 롤백)
```bash
# Assets/_Scripts/Player 폴더의 변경/추가 파일 되돌리기
git restore Assets/_Scripts/Player/
git clean -fd Assets/_Scripts/Player/
```

### ② Step 1 완료 전/후 커밋 단위 롤백
```bash
# 가장 최근 커밋 상태로 강제 되돌리기
git reset --hard HEAD
```

---

## 5. 🏁 완료 기준 (Definition of Done)
1. 빈 씬에서 실행 시 캡슐 캐릭터가 마우스 커서를 바라보고 WASD로 이동한다.
2. Space바 입력 시 지정된 방향으로 빠르게 돌진하고 대시 중 무적 상태가 된다.
3. 쿨다운이 끝날 때까지 재대시가 방지된다.
4. 모든 스크립트가 250줄 이하이며 `Bob.Player` 네임스페이스를 준수한다.
5. **검증 완료 후 이 문서를 `docs/history/step-01-player-controller.md`로 이동한다.**
