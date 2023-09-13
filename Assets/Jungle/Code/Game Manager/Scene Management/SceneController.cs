using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jungle
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController instance;
        private void Awake()
        {
            instance = this;
        }

        // On death, redirect to one of two scenes
        public void OnPlayerDeath()
        {
            CharacterLifeController.DecreaseLives(1);
            if (CharacterLifeController.GetCurrLives() <= 0)
            {
                BackToStart(); // TODO: Add a possible gameover screen
                return;
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // From title screen to game
        public void StartGame()
        {
            CharacterLifeController.InitLives();
            StartCoroutine(LoadGame());
        }
        private IEnumerator LoadGame()
        {
            int sceneIndex = SceneUtility.GetBuildIndexByScenePath(SceneList.GetLevel(1));
            if (sceneIndex < 0)
            {
                yield break;
            }
            
            StageDisplayController.SetCurrLevel("Stage 1");
            SceneManager.LoadScene(SceneList.GetLevel(1));
        }

        // From level to level
        public void StartLevel(int level)
        {
            StartCoroutine(LoadLevel(level));
        }
        private IEnumerator LoadLevel(int level)
        {
            int sceneIndex = SceneUtility.GetBuildIndexByScenePath(SceneList.GetLevel(level));
            if (sceneIndex < 0)
            {
                yield break;
            }

            StageDisplayController.SetCurrLevel("Stage " + level.ToString());
            SceneManager.LoadScene(SceneList.GetLevel(level));
        }

        // From level to game over / start of game
        public void BackToStart()
        {
            ScoreController.instance.ResetScore();
            StartCoroutine(LoadStart());
        }

        private IEnumerator LoadStart()
        {
            int sceneIndex = SceneUtility.GetBuildIndexByScenePath(SceneList.GetTitle());
            if (sceneIndex < 0)
            {
                yield break;
            }

            SceneManager.LoadSceneAsync(SceneList.GetTitle());
        }
    }
}
