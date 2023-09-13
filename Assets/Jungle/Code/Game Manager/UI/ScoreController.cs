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
        private static int score;
        private void Awake()
        {
            instance = this;
        }
        public void IncreaseScore(int val)
        {
            score += val;
        }
        public void ResetScore()
        {
            score = 0;
        }
        private void Update()
        {
            scoreText.text = "SCORE\n" + score.ToString();
        }
    }
}