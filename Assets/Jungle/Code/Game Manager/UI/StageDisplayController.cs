using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Jungle
{
    public class StageDisplayController : MonoBehaviour
    {
        public TMP_Text stageDisplay;
        private static string currLevel = "Stage 1";
        public static void SetCurrLevel(string newLevel)
        {
            currLevel = newLevel;
        }

        private void Update()
        {
            if (stageDisplay)
            {
                stageDisplay.text = currLevel;
            }
        }
    }
}
