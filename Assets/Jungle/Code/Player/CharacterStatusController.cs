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
        private float invulTimer;

        // Constants
        const float SPEEDUP_DURATION = 5f;
        const float SPEEDUP_MULTIPLIER = 1.5f;
        const float INVUL_DURATION = 5f;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            moveController = GetComponent<CharacterMoveController>();

            // Timers
            speedupTimer = 0;
            invulTimer = 0;
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

            if (invulTimer > 0)
            {
                invulTimer -= Time.deltaTime;
            }
            else
            {
                invulTimer = 0;
                GetComponent<SpriteRenderer>().color = Color.red; // DEBUG
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

        public void OnInvulnerability()
        {
            invulTimer = INVUL_DURATION;
            GetComponent<SpriteRenderer>().color = Color.yellow; // DEBUG
        }

        public bool IsInvulnerable()
        {
            return invulTimer > 0;
        }
    }
}
