using System;
using UnityEngine;

namespace Bob.Player
{
    /// <summary>
    /// 무적 판정을 포함한 회피 대시 메커니즘을 담당하는 컴포넌트
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerDash : MonoBehaviour
    {
        public event Action OnDashStarted;
        public event Action OnDashEnded;
        public event Action<bool> OnInvincibilityChanged;

        private Rigidbody _rigidbody;
        private PlayerMovement _movement;

        private float _cooldownTimer;
        private float _dashRemainingTime;
        private Vector3 _dashDirection;

        public bool IsDashing { get; private set; }
        public bool IsInvincible { get; private set; }
        public float CooldownRatio => Mathf.Clamp01(_cooldownTimer / 1f);

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _movement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= Time.deltaTime;
            }

            if (IsDashing)
            {
                _dashRemainingTime -= Time.deltaTime;
                if (_dashRemainingTime <= 0f)
                {
                    EndDash();
                }
            }
        }

        private void FixedUpdate()
        {
            if (IsDashing)
            {
                #if UNITY_6000_0_OR_NEWER
                _rigidbody.linearVelocity = new Vector3(_dashDirection.x, _rigidbody.linearVelocity.y, _dashDirection.z);
                #else
                _rigidbody.velocity = new Vector3(_dashDirection.x, _rigidbody.velocity.y, _dashDirection.z);
                #endif
            }
        }

        public bool TryDash(PlayerStats stats, Vector2 moveInput)
        {
            if (IsDashing || _cooldownTimer > 0f) return false;

            StartDash(stats, moveInput);
            return true;
        }

        private void StartDash(PlayerStats stats, Vector2 moveInput)
        {
            IsDashing = true;
            SetInvincible(true);

            _cooldownTimer = stats.dashCooldown;
            _dashRemainingTime = stats.dashDuration;

            // 이동 입력이 있으면 해당 방향으로, 없으면 현재 바라보는 방향으로 대시
            Vector3 inputDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            _dashDirection = (inputDirection.sqrMagnitude > 0.01f)
                ? inputDirection * stats.dashSpeed
                : _movement.LookDirection * stats.dashSpeed;

            if (_movement != null)
            {
                _movement.IsMovementLocked = true;
            }

            OnDashStarted?.Invoke();
        }

        private void EndDash()
        {
            IsDashing = false;
            SetInvincible(false);

            if (_movement != null)
            {
                _movement.IsMovementLocked = false;
                _movement.SetVelocity(Vector3.zero);
            }

            OnDashEnded?.Invoke();
        }

        private void SetInvincible(bool invincible)
        {
            if (IsInvincible == invincible) return;
            IsInvincible = invincible;
            OnInvincibilityChanged?.Invoke(IsInvincible);
        }
    }
}
