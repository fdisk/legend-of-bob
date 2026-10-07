# 🤖 Antigravity Agent Instructions: Legend of Bob

이 파일은 Google Antigravity IDE에서 AI Agent가 이 프로젝트의 코드를 작성하고 협업할 때 자동으로 준수해야 하는 최상위 프로젝트 지침입니다.

---

## 📌 프로젝트 핵심 참조 문서
* 상세 기획 및 개발 명세서: [`PROJECT.md`](./PROJECT.md)
* 게임 소개 및 개요: [`README.md`](./README.md)

---

## 🎯 AI Agent 코딩 규칙 (Mandatory Rules)

1. **Code-First & Procedural Binding (인스펙터 의존 금지)**
   - Unity Inspector 창을 통한 수동 드래그 앤 드롭 연결을 전제하지 마십시오.
   - UI 요소, 오디오, 프리팹 인스턴스화 및 컴포넌트 바인딩은 C# 코드 내에서 명시적/동적으로 처리합니다.
   - 프리팹 에셋이 아직 없는 상태에서도 실행 및 검증이 가능하도록 프로토타입 Primitive(큐브, 캡슐 등) Fallback 메커니즘을 구비합니다.

2. **Event-Driven Decoupling (이벤트 기반 분리)**
   - 매니저나 시스템 클래스를 직접 참조(`Player` -> `RoomManager` 등)하지 않습니다.
   - C# `Action`, `event`, 또는 정적 이벤트 버스(`Events.OnEliteKilled`)를 통해 메시지를 발행/구독(Pub/Sub)합니다.

3. **SRP & 작은 파일 유지 (250줄 이하)**
   - 단일 파일당 **최대 250줄 이하**를 엄격히 유지합니다.
   - 책임이 늘어나면 즉시 서브 컴포넌트나 헬퍼 클래스로 분리합니다.

4. **네임스페이스 및 구조화**
   - 모든 스크립트는 `Bob.*` 루트 네임스페이스를 따릅니다.
     - 예: `Bob.Core`, `Bob.Player`, `Bob.Combat`, `Bob.Rooms`, `Bob.Gacha`, `Bob.UI`

5. **사용자 언어 규칙**
   - 모든 대화 및 설명은 한국어로 진행합니다.
