using UnityEngine;

namespace Bob.Player
{
    /// <summary>
    /// 플레이어 하위 시스템(이동, 회전, 대시, 입력)을 총괄하는 메인 파사드 컴포넌트
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerDash))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerStats _stats = new PlayerStats();

        private PlayerInputHandler _input;
        private PlayerMovement _movement;
        private PlayerDash _dash;

        public PlayerStats Stats => _stats;
        public bool IsInvincible => _dash != null && _dash.IsInvincible;
        public bool IsDashing => _dash != null && _dash.IsDashing;
        public Vector3 LookDirection => _movement != null ? _movement.LookDirection : transform.forward;

        private void Awake()
        {
            BindComponents();
            ConfigureCollider();
        }

        private void BindComponents()
        {
            _input = GetComponent<PlayerInputHandler>();
            _movement = GetComponent<PlayerMovement>();
            _dash = GetComponent<PlayerDash>();

            _input.OnDashRequested += HandleDashRequest;
        }

        private void ConfigureCollider()
        {
            CapsuleCollider col = GetComponent<CapsuleCollider>();
            col.height = 2f;
            col.radius = 0.5f;
            col.center = Vector3.zero; // 캡슐 원점 기준 정렬
        }

        private void OnDestroy()
        {
            if (_input != null)
            {
                _input.OnDashRequested -= HandleDashRequest;
            }
        }

        private void Update()
        {
            if (_input == null || _movement == null) return;

            // 이동 입력을 매 프레임 Movement 컴포넌트로 전달
            _movement.SetMoveInput(_input.MoveInput);

            // 대시 중이 아닐 때 마우스 조준 회전
            if (_dash == null || !_dash.IsDashing)
            {
                _movement.RotateTowards(_input.MouseScreenPosition);
            }
        }

        private void FixedUpdate()
        {
            if (_movement != null)
            {
                _movement.MoveFixed(_stats);
            }
        }

        private void HandleDashRequest()
        {
            if (_dash != null && _input != null)
            {
                _dash.TryDash(_stats, _input.MoveInput);
            }
        }

        public void ApplyStatsModifier(PlayerStats newStats)
        {
            _stats = newStats;
        }
    }
}
