using UnityEngine;

namespace Bob.Player
{
    /// <summary>
    /// 물리 기반 8방향 이동 및 마우스 커서 지향 회전을 담당하는 컴포넌트
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Camera _mainCamera;
        private Vector3 _currentVelocity;
        private Vector3 _targetMoveDirection;

        public bool IsMovementLocked { get; set; }
        public Vector3 LookDirection { get; private set; } = Vector3.forward;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            SetupRigidbody();
            _mainCamera = Camera.main;
        }

        private void SetupRigidbody()
        {
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | 
                                      RigidbodyConstraints.FreezeRotationY | 
                                      RigidbodyConstraints.FreezeRotationZ;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

            // 바닥과의 마찰력으로 멈추는 현상 방지
            PhysicsMaterial frictionless = new PhysicsMaterial("Frictionless_Player")
            {
                dynamicFriction = 0f,
                staticFriction = 0f,
                frictionCombine = PhysicsMaterialCombine.Minimum
            };

            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                col.material = frictionless;
            }
        }

        public void SetMoveInput(Vector2 input)
        {
            _targetMoveDirection = new Vector3(input.x, 0f, input.y);
        }

        public void RotateTowards(Vector3 mouseScreenPos)
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
                if (_mainCamera == null) return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(mouseScreenPos);
            Plane groundPlane = new Plane(Vector3.up, transform.position);

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 direction = hitPoint - transform.position;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.001f)
                {
                    LookDirection = direction.normalized;
                    transform.rotation = Quaternion.LookRotation(LookDirection);
                }
            }
        }

        public void MoveFixed(PlayerStats stats)
        {
            if (IsMovementLocked) return;

            Vector3 targetVelocity = _targetMoveDirection * stats.moveSpeed;
            float accelRate = (_targetMoveDirection.sqrMagnitude > 0.01f) 
                ? stats.acceleration 
                : stats.deceleration;

            _currentVelocity = Vector3.MoveTowards(_currentVelocity, targetVelocity, accelRate * Time.fixedDeltaTime);

            // Unity 6 표준: linearVelocity 적용 (Y축 중력은 유지, X/Z 수평 이동 직접 부여)
            #if UNITY_6000_0_OR_NEWER
            float currentY = _rigidbody.linearVelocity.y;
            _rigidbody.linearVelocity = new Vector3(_currentVelocity.x, currentY, _currentVelocity.z);
            #else
            float currentY = _rigidbody.velocity.y;
            _rigidbody.velocity = new Vector3(_currentVelocity.x, currentY, _currentVelocity.z);
            #endif
        }

        public void SetVelocity(Vector3 velocity)
        {
            _currentVelocity = velocity;
            #if UNITY_6000_0_OR_NEWER
            _rigidbody.linearVelocity = velocity;
            #else
            _rigidbody.velocity = velocity;
            #endif
        }

        public void StopImmediately()
        {
            _currentVelocity = Vector3.zero;
            _targetMoveDirection = Vector3.zero;
            #if UNITY_6000_0_OR_NEWER
            _rigidbody.linearVelocity = new Vector3(0f, _rigidbody.linearVelocity.y, 0f);
            #else
            _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);
            #endif
        }
    }
}
