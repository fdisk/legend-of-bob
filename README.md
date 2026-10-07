# Legend of Bob (밥의 전설)

> **"아무것도 아닌 자(Bob)가 전설이 되는 생존의 시련. 재난과 오작동 AI로 무너진 한국 산악지대에서 펼쳐지는 짧고 강렬한 탑다운 로그라이트 슈터."**  
> *"A survival trial where a nobody becomes a legend. A bite-sized, high-octane top-down roguelite shooter set in post-apocalyptic Korea, driven by faulty military AI and sheer survival."*

---

## 📌 Project Overview / 프로젝트 개요

* **Platform / 플랫폼:** Steam (PC)
* **Engine & Language / 엔진 및 언어:** Unity (URP 3D/2.5D Top-Down) / C#
* **Development Model / 개발 방식:** 1-Person AI Agent Collaboration (Google Antigravity & Gemini)
* **Genre / 장르:** Action Roguelite, Top-Down Shooter, Trial Runner
* **Target Session / 세션 호흡:** 부담 없이 짧게 완결되는 런(Run) 단위 플레이

---

## 💡 Core Concepts / 핵심 콘셉트

* **1. 단절성 (Run 내부)**  
  캐릭터 레벨, 획득한 스킬 칩, 무기 개조 상태는 해당 런이 끝나면 완전 초기화됩니다.
* **2. 연속성 (Meta 거점)**  
  런 클리어 또는 사망 시 회수한 '연구 데이터'로 다음 런에 등장할 신규 카드/무기 풀(Pool)을 영구 해금합니다.
* **3. 보상 밸런스**  
  클리어 보상은 티어 편차를 최소화해 불쾌감을 줄이고, 방 내부에서 엘리트 처치 시 터지는 즉발 가챠에서 도파민을 극대화합니다.

---

## 🎮 Core Game Loop / 핵심 게임 루프

탐험형 오픈월드를 배제하고, 짧고 강렬한 방(Room) 단위의 돌파와 전략적 선택에 집중하는 **세케마의 시련(Sekhema Trials) 스타일 노드 구조**를 따릅니다.

```text
[방 입장 / Enter Room]
       │
       ├─► [전투 및 기믹 돌파 / Combat & Challenge]
       │      └─► 엘리트 처치 & 상자 개봉 ──► ★ [인게임 즉발 잭팟 가챠] ★ (화력 폭발)
       │
       ├─► [방 클리어 / Room Clear] ──► [균등한 기본 부품 보상] (안정적 성장 베이스)
       │
       └─► [출구 게이트 2택 / Gate Choice]
              ├─ 경로 A: [기믹: 탄막 레이저] / [다음 보상: 무기 개조 파츠]
              └─ 경로 B: [기믹: 산소 카운트다운] / [다음 보상: 액티브 스킬 모듈]
```

---

## ⚠️ Room Gimmicks & Challenges / 방 기믹 및 챌린지

| Gimmick / 기믹 | Description (KR) | Description (EN) |
| :--- | :--- | :--- |
| **Bullet Hell (탄막)** | 오작동한 방공 터렛의 탄막을 대시 무적 판정으로 회피 | Dodge barrage patterns using i-frame dashes |
| **Time Trial (타임어택)** | 산소 고갈/구역 붕괴 타이머 안에 탈출구 주파 | Race against an oxygen countdown timer |
| **Platforming (발판)** | 무너지는 채굴 발판과 낙하 함정 돌파 | Traverse collapsing bridges and moving hazards |
| **Wave Defense (방어)** | 밀려드는 돌연변이를 상대로 빌드 화력 쏟아붓기 | Annihilate waves of mutants with overclocked builds |

---

## 📚 Documentation & Specifications

프로젝트 개발 명세 및 AI Agent 협업 헌장은 아래 전용 문서에서 확인하실 수 있습니다:

* 🎯 **프로토타입 개발 및 아키텍처 명세서:** [`PROJECT.md`](./PROJECT.md)
* 🤖 **Antigravity AI Agent 작업 지침 및 코딩 규칙:** [`AGENTS.md`](./AGENTS.md)