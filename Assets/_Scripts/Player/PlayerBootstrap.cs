using UnityEngine;

namespace Bob.Player
{
    /// <summary>
    /// 프리팹 에셋이 없는 초기 프로토타입 단계에서 빈 씬에 캡슐 플레이어, 조명, 바닥, 카메라를 동적으로 생성하는 헬퍼
    /// </summary>
    public class PlayerBootstrap : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInitializeIfMissing()
        {
            if (FindAnyObjectByType<PlayerController>() != null) return;

            GameObject bootstrapObj = new GameObject("[Prototype_Bootstrap]");
            bootstrapObj.AddComponent<PlayerBootstrap>();
        }

        private void Start()
        {
            EnsureLighting();
            EnsureGround();
            GameObject player = CreatePrototypePlayer();
            SetupCamera(player.transform);
        }

        private void EnsureLighting()
        {
            if (FindAnyObjectByType<Light>() != null) return;

            GameObject lightObj = new GameObject("Directional Light");
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            lightObj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private void EnsureGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "[Prototype_Ground]";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(5f, 1f, 5f); // 50m x 50m 아레나
        }

        private GameObject CreatePrototypePlayer()
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player_Bob_Prototype";
            // 바닥에 파묻히지 않도록 높이 1.1f에서 스폰
            player.transform.position = new Vector3(0f, 1.1f, 0f);

            // 시선(조준) 방향 식별용 작은 노즈 큐브 부착
            GameObject nose = GameObject.CreatePrimitive(PrimitiveType.Cube);
            nose.name = "Aim_Indicator";
            nose.transform.SetParent(player.transform);
            nose.transform.localPosition = new Vector3(0f, 0.5f, 0.6f);
            nose.transform.localScale = new Vector3(0.3f, 0.2f, 0.5f);

            // 렌더러 컬러 구분
            Renderer rend = player.GetComponent<Renderer>();
            if (rend != null) rend.material.color = new Color(0.2f, 0.6f, 1.0f); // Bob 블루

            // 플레이어 컨트롤러 부착 (RequireComponent들이 함께 자동 추가됨)
            player.AddComponent<PlayerController>();

            return player;
        }

        private void SetupCamera(Transform target)
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                GameObject camObj = new GameObject("Main Camera");
                cam = camObj.AddComponent<Camera>();
                camObj.tag = "MainCamera";
            }

            cam.transform.position = new Vector3(0f, 14f, -9f);
            cam.transform.rotation = Quaternion.Euler(58f, 0f, 0f);

            SimpleCameraFollow follow = cam.gameObject.AddComponent<SimpleCameraFollow>();
            follow.target = target;
        }
    }

    /// <summary>
    /// 프로토타입용 탑다운 카메라 부드러운 추적 헬퍼
    /// </summary>
    public class SimpleCameraFollow : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 14f, -9f);
        public float smoothSpeed = 10f;

        private void LateUpdate()
        {
            if (target == null) return;
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
