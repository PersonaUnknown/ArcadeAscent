using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Jungle
{
    // This script handles the logic of the player's score
    public class ScoreController : MonoBehaviour
    {
        public static ScoreController instance;
        public TMP_Text scoreText;
        private int _score;
        private int score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                scoreText.text = "SCORE\n" + _score.ToString();
            }
        }
        private void Awake()
        {
            instance = this;
        }
        public void IncreaseScore(int val)
        {
            score += val;
        }
    }
}