using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle
{
    // Increases the player's score when collected
    public class ScoreCollectible : Collectible
    {
        ScoreController scoreController;
        public int itemValue;
        private void Start()
        {
            scoreController = ScoreController.instance;
        }
        public override void OnCollect()
        {
            scoreController.IncreaseScore(itemValue);
        }
    }
}
