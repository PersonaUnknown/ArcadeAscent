using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle
{
    // All collectibles have 
    public abstract class Collectible : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                OnCollect();
                Destroy(gameObject);
            }
        }

        public abstract void OnCollect();
    }
}
