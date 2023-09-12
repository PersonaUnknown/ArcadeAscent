using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jungle
{
    public class TestWarpZone : MonoBehaviour
    {
        public int options;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                switch (options)
                {
                    case 0:
                        SceneController.instance.BackToStart();
                        break;
                    case 1:
                        SceneController.instance.StartLevel(2);
                        break;
                    case 2:
                        SceneController.instance.StartLevel(3);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
