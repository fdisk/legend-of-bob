using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bob.Player
{
    /// <summary>
    /// Unity New Input System(UnityEngine.InputSystem) 기반 플레이어 입력 수집 컴포넌트
    /// </summary>
    public class PlayerInputHandler : MonoBehaviour
    {
        public event Action<Vector2> OnMoveInputChanged;
        public event Action OnDashRequested;

        public Vector2 MoveInput { get; private set; }
        public Vector3 MouseScreenPosition { get; private set; }
        public bool IsInputBlocked { get; set; }

        private void Update()
        {
            if (IsInputBlocked)
            {
                MoveInput = Vector2.zero;
                return;
            }

            ProcessMovementInput();
            ProcessAimInput();
            ProcessActionInput();
        }

        private void ProcessMovementInput()
        {
            Vector2 rawInput = Vector2.zero;
            Keyboard keyboard = Keyboard.current;

            if (keyboard != null)
            {
                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) rawInput.y += 1f;
                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) rawInput.y -= 1f;
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) rawInput.x -= 1f;
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) rawInput.x += 1f;
            }

            MoveInput = rawInput.sqrMagnitude > 1f ? rawInput.normalized : rawInput;
            OnMoveInputChanged?.Invoke(MoveInput);
        }

        private void ProcessAimInput()
        {
            Mouse mouse = Mouse.current;
            if (mouse != null)
            {
                Vector2 mousePos = mouse.position.ReadValue();
                MouseScreenPosition = new Vector3(mousePos.x, mousePos.y, 0f);
            }
        }

        private void ProcessActionInput()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
            {
                OnDashRequested?.Invoke();
            }
        }
    }
}
