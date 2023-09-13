using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle
{
    public class EnemyMoveController : MonoBehaviour
    {
        Rigidbody2D rb;
        List<RaycastHit2D> hits;
        public ContactFilter2D filter;

        // Vertical Movement
        float fallDirection;
        [SerializeField] private float fallSpeed;
        [SerializeField] private float vertCheck;

        // Horizontal Movement
        float moveDirection;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float horizCheck;
        private bool facingLeft;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            hits = new List<RaycastHit2D>();
            fallDirection = 0;
            facingLeft = false;
        }

        private void FixedUpdate()
        {
            GroundCheck();
            HorizCheck();
            Move();
        }

        private void GroundCheck()
        {
            int count = rb.Cast(Vector2.down, filter, hits, vertCheck);
            if (count > 0)
            {
                fallDirection = 0;
                return;
            }

            // Fall
            fallDirection = -1;
        }

        private void HorizCheck()
        {
            int count = facingLeft ? rb.Cast(Vector2.left, filter, hits, horizCheck) : rb.Cast(Vector2.right, filter, hits, horizCheck);
            if (count > 0)
            {
                facingLeft = !facingLeft;
            }

            moveDirection = facingLeft ? -1 : 1;
        }

        private void Move()
        {
            Vector2 newDirection = new Vector2(moveDirection, fallDirection).normalized;

            // Vertical and Horizontal
            rb.MovePosition((Vector2)transform.position + fallSpeed * Time.fixedDeltaTime * newDirection);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                SceneController.instance.OnPlayerDeath();
            }
        }
    }
}
