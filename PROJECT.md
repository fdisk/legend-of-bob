# 🎮 Project Specification: Legend of Bob (밥의 전설)

> **Google Antigravity IDE & AI Agent 협업 프로토타입 개발 가이드**

---

## 1. 📌 프로젝트 개요 및 비전

* **게임명:** Legend of Bob (밥의 전설)
* **장르:** Action Roguelite, Top-Down Shooter, Trial Runner
* **엔진 및 언어:** Unity (URP 3D / 2.5D Top-Down View) / C#
* **플랫폼:** Steam (PC)
* **핵심 컨셉:**
  - 아무것도 없는 생존자 'Bob'이 재난과 군사용 AI가 폭주한 한국 산악지대에서 생존하는 짧고 강렬한 트라이얼.
  - 오픈월드 탐험 대신 **방(Room) 단위의 빠른 클리어**와 **출구 선택(2지선다)** 집중.
  - 엘리트 처치 시 터지는 **인게임 즉발 잭팟 가챠(화력 폭발 도파민)** + 룸 클리어 시의 **균등한 기본 보상(안정적 성장)**.

---

## 2. 🎯 프로토타입 목표 (Phase 1 MVP Scope)

> **목표:** "3~5분 내에 단일 런(Run)의 핵심 재미 루프를 완결성 있게 체감할 수 있는 프로토타입 구축"

### 필수 구현 범위
1. **탑다운 플레이어 컨트롤러:**
   - 8방향 이동 (WASD)
   - 마우스 커서 방향 조준 및 발사 (좌클릭 사격)
   - 스페이스바 회피 대시 (무적 판정 i-frame 포함)
2. **룸 시스템 (Room Lifecycle):**
   - 방 입장 ➜ 문 잠금 ➜ 전투/기믹 시작 ➜ 적 전멸/기믹 돌파 ➜ 문 개방 및 2택 출구 활성화
3. **전투 및 엘리트 스폰:**
   - 일반 근접/원거리 돌연변이 적
   - 방당 1마리 이상의 엘리트 몬스터 (강화 패턴 및 높은 체력)
4. **즉발 잭팟 가챠 시스템 (In-game Instant Gacha):**
   - 엘리트 처치 시 상자/오브가 즉발 드롭
   - 상호작용 또는 접촉 시 3장의 강화 카드 중 1택 (화력 대폭 강화/특수 탄환 등)
5. **다음 방 전이 (2지선다 게이트):**
   - 게이트 A: 기믹(탄막 회피) / 보상(무기 파츠)
   - 게이트 B: 기믹(산소 카운트다운) / 보상(액티브 모듈)

---

## 3. 🤖 AI Agent 협업 헌장 (Architecture Rules)

Google Antigravity IDE로 AI Agent가 코드를 작성할 때 **반드시 준수해야 하는 핵심 원칙**입니다.

### ① Code-First & Procedural Binding (인스펙터 의존 최소화)
* **규칙:** Unity Inspector 창의 수동 드래그 앤 드롭 링크에 의존하지 않습니다.
* **이유:** AI Agent는 유니티 에디터 GUI를 직접 클릭할 수 없으므로, 코드로 온전히 연결되어야 동작 보증이 가능합니다.
* **지침:**
  - 프리팹이나 에셋이 없는 초기 단계에서는 Unity 기본 프리미티브(Cube, Capsule, Sphere)를 코드로 생성하여 테스트 가능한 상태(Mocking/Fallback)를 유지합니다.
  - UI, 오디오, 컴포넌트 바인딩은 C# 스크립트 내부에서 동적으로 `AddComponent`, `GetComponent`, 또는 팩토리 메서드를 통해 연결합니다.

### ② Event-Driven Decoupling (시스템 간 직접 참조 금지)
* **규칙:** 시스템 간 강결합(`Player`가 직접 `RoomManager`를 호출하거나 `GachaSystem`을 참조하는 구조)을 엄격히 금지합니다.
* **지침:**
  - C# `Action`, `Func`, 또는 이벤트 브로커(`Action<T>`) 기반 발행-구독(Pub/Sub) 모델을 사용합니다.
  - 예시:
    ```csharp
    // 전투 시스템이 발생시킴
    CombatEvents.OnEliteKilled?.Invoke(elitePosition);

    // 가챠 시스템이 독립적으로 감지
    CombatEvents.OnEliteKilled += SpawnGachaChest;
    ```

### ③ Clean Separation & Small Files (파일 크기 제약)
* **규칙:** 단일 책임 원칙(SRP)을 철저히 지키며, **단일 C# 파일은 250줄 이하**로 유지합니다.
* **지침:** 데이터(ScriptableObject/Data Class), 로직(Controller/Service), 뷰(MonoBehaviour/Presenter)를 분리합니다.

---

## 4. 📁 권장 프로젝트 디렉터리 구조

프로젝트 에셋(`Assets/`) 구조는 아래와 같이 체계적으로 관리합니다:

```text
Assets/
├── _Scripts/
│   ├── Core/                 # 게임 매니저, 씬 흐름, 전역 이벤트
│   │   ├── GameManager.cs
│   │   └── Events/           # GameEvents, CombatEvents 등
│   ├── Player/               # 플레이어 입력, 이동, 조준, 대시
│   │   ├── PlayerController.cs
│   │   ├── PlayerCombat.cs
│   │   └── PlayerStats.cs
│   ├── Combat/               # 투사체, 피격 판정, 데미지 계산
│   │   ├── Projectile.cs
│   │   ├── IDamageable.cs
│   │   └── DamageInfo.cs
│   ├── Enemies/              # 적 스폰, 일반 몬스터, 엘리트 AI
│   │   ├── EnemyBase.cs
│   │   └── EliteEnemy.cs
│   ├── Rooms/                # 방 라이프사이클, 기믹, 출구 게이트
│   │   ├── RoomManager.cs
│   │   ├── RoomGate.cs
│   │   └── Gimmicks/
│   ├── Gacha/                # 인게임 즉발 가챠, 업그레이드 카드 풀
│   │   ├── GachaChest.cs
│   │   └── UpgradeCardData.cs
│   └── UI/                   # HUD, 데미지 텍스트, 가챠 선택 팝업
│       ├── HUDController.cs
│       └── GachaUI.cs
├── Prefabs/                  # 런타임 인스턴스화용 프리팹
└── Materials/                # 프로토타입용 머티리얼 및 색상
```

---

## 5. 🚀 단계별 구현 로드맵 (Actionable Steps)

* [ ] **Step 1: Player Controller & Movement**
  - WASD 탑다운 이동, 마우스 조준 회전, 무적 판정 대시 구현.
* [ ] **Step 2: Weapon & Combat Baseline**
  - 투사체 발사, 마우스 좌클릭 연사, 기본 적(더미) 피격 판정 및 데미지 플로팅.
* [ ] **Step 3: Room Arena & Wave System**
  - 닫힌 아레나 생성, 적 스폰, 방 클리어 판정.
* [ ] **Step 4: Elite Enemy & Instant Gacha Drop**
  - 엘리트 처치 이벤트 발행, 즉발 가챠 상자 드롭 및 3택 업그레이드 적용.
* [ ] **Step 5: Exit Gate 2-Choice & Next Room Transition**
  - 클리어 시 A/B 게이트 활성화 및 방 루프 사이클 완성.

---

## 6. 💻 개발 환경 설정 가이드 (macOS & Windows 11)

이 프로젝트는 **macOS**와 **Windows 11** 크로스 플랫폼 환경에서 협업 개발됩니다.

### ① Unity Hub & Unity CLI
* **Unity Hub:** macOS / Windows 11 양쪽 OS에 설치 필수.
* **Unity CLI 환경 (연동 완료):**
  - **설치 경로:** `~/.unity/bin/unity` (환경 설정: `~/.unity/env` ➜ `~/.zshrc`에 자동 등록)
  - **현재 버전:** `1.0.0-beta.12`
  - **Antigravity 내장 터미널 단축키:** `Ctrl + \`` (또는 `Cmd + J`)
  - **셸 갱신 (터미널에서 미인식 시):** `source ~/.zshrc`
* **주요 CLI 치트시트 (터미널 기반 조작):**
  - `unity --version` : CLI 버전 확인
  - `unity editors` : 현재 머신에 설치된 Unity 에디터 목록 조회
  - `unity install [version]` : 터미널에서 특정 에디터 다운로드 및 설치
  - `unity projects` : Hub에 등록된 Unity 프로젝트 목록 조회
  - `unity open .` : 현재 디렉터리의 프로젝트를 해당 Unity 에디터로 즉시 실행
  - `unity doctor` : Unity CLI 환경 진단 및 이슈 체크
  - `unity setup` : AI 코딩 에이전트 지원 설정

### ② Unity 에디터 버전 통일 (중요)
* **권장 버전:** **Unity 6 (6000.x LTS)** 또는 **Unity 2022.3 LTS**
* **템플릿:** **3D (URP - Universal Render Pipeline)**
* **주의:** macOS와 Windows 11 머신 간에 **정확히 동일한 Unity 버전**(예: `6000.0.35f1`)을 설치해야 프로젝트 설정 및 메타(.meta) 파일 충돌을 방지할 수 있습니다.
* **필요 빌드 모듈:**
  - macOS: `macOS Build Support`
  - Windows 11: `Windows Build Support (IL2CPP / Mono)`

### ③ 크로스 플랫폼 Git 설정
* 개행 문자(`CRLF` vs `LF`) 차이로 인한 변경점 오염을 막기 위해 루트 디렉터리에 [`.gitattributes`](./.gitattributes)를 적용합니다.

---

## 7. 📑 프로토타입 작업 관리 및 롤백 워크플로우

개발 진행 상황을 투명하게 추적하고, 문제 발생 시 안전하게 롤백할 수 있도록 문서 기반 워크플로우를 운영합니다.

```text
[작업 기획 및 착수] ──► docs/proceed/step-XX-[name].md (진행 중 / 체크리스트 & 롤백 가이드)
                                │
                        (구현 및 검증 완료)
                                │
                                ▼
                       docs/history/step-XX-[name].md (완료 이력 아카이브)
```

### ① 진행 중 작업 관리 (`docs/proceed/`)
* 각 Step 시작 시 `docs/proceed/step-XX-[name].md` 문서를 생성합니다.
* 목표, 생성할 파일 목록, 체크리스트, Step 전용 **롤백 전략**을 명시합니다.
* 작업 진행 중 태스크 완료 여부를 실시간으로 갱신합니다.

### ② 완료 이력 보관 (`docs/history/`)
* 해당 Step의 구현 및 검증(Definition of Done)이 끝나면 해당 문서를 `docs/history/`로 이동합니다.
* 작업 완료 상태(`🟢 완료`) 및 결과 로그를 최종 기록합니다.

### ③ 단계별 롤백 (Rollback) 전략
* 작업 도중 문제가 발생하거나 방향 전환이 필요할 경우, 각 작업 문서에 기재된 롤백 명령어로 안전하게 이전 상태로 되돌립니다.
* 작업 단위별 Git 커밋을 원칙으로 하여 특정 Step 전후 시점으로 즉각 복구할 수 있게 유지합니다.

