using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle
{
    public class InvulCollectible : Collectible
    {
        public override void OnCollect()
        {
            CharacterStatusController.instance.OnInvulnerability();
        }
    }
}
