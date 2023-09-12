using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle
{
    // This script handles affecting the player's stats and resetting them back to normal
    public class CharacterStatusController : MonoBehaviour
    {
        // Components
        public static CharacterStatusController instance;
        CharacterMoveController moveController;

        // Timers
        private float speedupTimer;

        // Constants
        const float SPEEDUP_DURATION = 5f;
        const float SPEEDUP_MULTIPLIER = 1.5f;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            moveController = GetComponent<CharacterMoveController>();

            // Speed
            speedupTimer = 0;
        }

        private void Update()
        {
            if (speedupTimer > 0)
            {
                speedupTimer -= Time.deltaTime;
            }
            else
            {
                speedupTimer = 0;
                EndSpeedBoost();
            }
        }

        public void OnSpeedBoost()
        {
            speedupTimer = SPEEDUP_DURATION;
            moveController.SpeedBoost(SPEEDUP_MULTIPLIER);
        }
        public void EndSpeedBoost()
        {
            moveController.ResetSpeed();
        }
    }
}
