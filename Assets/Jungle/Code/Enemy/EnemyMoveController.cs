using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveController : MonoBehaviour
{
    Rigidbody2D rb;
    List<RaycastHit2D> hits;
    public ContactFilter2D filter;
    Vector2 fallDirection;

    [SerializeField] private float fallSpeed;
    [SerializeField] private float vertCheck;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hits = new List<RaycastHit2D>();
        fallDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move();
    }

    private void GroundCheck()
    {
        int count = rb.Cast(Vector2.down, filter, hits, vertCheck);
        if (count > 0)
        {
            fallDirection = Vector2.zero;
            return;
        }

        // Fall
        fallDirection = Vector2.down;
    }

    private void Move()
    {
        // Vertical
        rb.MovePosition((Vector2)transform.position + fallSpeed * Time.fixedDeltaTime * fallDirection);
    }
}
