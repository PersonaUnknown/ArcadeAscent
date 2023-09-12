using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle
{
    public class SpeedCollectible : Collectible
    {
        CharacterStatusController playerStatus;
        private void Start()
        {
            playerStatus = CharacterStatusController.instance;
        }

        public override void OnCollect()
        {
            playerStatus.OnSpeedBoost();
        }
    }
}
