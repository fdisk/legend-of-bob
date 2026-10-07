using System;
using UnityEngine;

namespace Bob.Player
{
    /// <summary>
    /// 플레이어의 기본 이동, 대시, 생존 관련 수치 데이터
    /// </summary>
    [Serializable]
    public class PlayerStats
    {
        [Header("Movement")]
        [Tooltip("기본 이동 속도")]
        public float moveSpeed = 8.5f;

        [Tooltip("가속도")]
        public float acceleration = 50f;

        [Tooltip("감속도")]
        public float deceleration = 60f;

        [Header("Dash")]
        [Tooltip("대시 속도")]
        public float dashSpeed = 22f;

        [Tooltip("대시 지속 시간 (초)")]
        public float dashDuration = 0.22f;

        [Tooltip("대시 재사용 대기 시간 (초)")]
        public float dashCooldown = 0.9f;

        [Header("Survival")]
        [Tooltip("최대 체력")]
        public float maxHealth = 100f;

        /// <summary>
        /// 기본값으로 복제 생성
        /// </summary>
        public PlayerStats Clone()
        {
            return (PlayerStats)MemberwiseClone();
        }
    }
}
