using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Jungle
{

    // Keeps track of the number of lives the player starts out with
    public class CharacterLifeController : MonoBehaviour
    {
        public TMP_Text lifeCountText;

        // These values are flexible to game design
        private static int _currPlayerLives;
        private static int currPlayerLives
        {
            get
            {
                return _currPlayerLives;
            }
            set
            {
                _currPlayerLives = Mathf.Clamp(value, 0, MAX_PLAYER_LIVES);
            }
        }

        const int MAX_PLAYER_LIVES = 9;
        const int START_PLAYER_LIVES = 3;
    
        public static int GetCurrLives()
        {
            return currPlayerLives;
        }

        public static void DecreaseLives(int val)
        {
            currPlayerLives -= val;
        }

        public static void IncreaseLives(int val)
        {
            currPlayerLives += val;
        }

        public static void InitLives()
        {
            currPlayerLives = START_PLAYER_LIVES;
        }

        private void Update()
        {
            lifeCountText.text = "x " + currPlayerLives.ToString();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DecreaseLives(1);
            }
        }
    }
}
