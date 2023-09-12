using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jungle
{
    // Stores the necessary information of scene names of Jungle Escape
    public static class SceneList
    {
        private const string TITLE = "Jungle_Title";
        private const int NON_LEVEL_COUNT = 1;
        public static string GetTitle()
        {
            return TITLE;
        }

        public static string GetLevel(int levelNumber)
        {
            return "Jungle_S" + levelNumber.ToString();
        }

        public static int GetNumLevels()
        {
            return SceneManager.sceneCountInBuildSettings - NON_LEVEL_COUNT;
        }
    }
}
