using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle
{

    // This script controls how the player character moves based on player input
    public class CharacterMoveController : MonoBehaviour
    {
        // Components
        private CharacterInputController input;
        private Rigidbody2D rb;
        private CharacterStateController state;
        private BoxCollider2D bc;

        // Climbing Movement
        private float defaultClimbSpeed;
        [SerializeField] private float climbSpeed;

        // Wall Jump Movement
        [SerializeField] private float wallJumpLength;
        [SerializeField] private float wallJumpDistance;

        // Vertical Movement
        [SerializeField] private bool isJumping;
        [SerializeField] private int jumpCount;
        [SerializeField] private float jumpTimer; // Counts how long the player has jumped for
        [SerializeField] private float jumpLength; // How long the player should ascend while jumping
        [SerializeField] private float jumpSpeed;
        [SerializeField] private float fallSpeed;
        [SerializeField] private ContactFilter2D collisionFilter;
        List<RaycastHit2D> hits;
        private const int MAX_JUMPS = 1;

        // Horizontal Movement
        private float defaultMoveSpeed;
        [SerializeField] private float moveSpeed;

        // Movement
        [SerializeField] private Vector2 moveDirection;

        private void Awake()
        {
            defaultClimbSpeed = climbSpeed;
            defaultMoveSpeed = moveSpeed;
        }

        private void Start()
        {
            input = GetComponent<CharacterInputController>();
            rb = GetComponent<Rigidbody2D>();
            state = GetComponent<CharacterStateController>();
            bc = GetComponent<BoxCollider2D>();

            hits = new List<RaycastHit2D>();
            isJumping = false;
            jumpCount = GroundCheck() ? MAX_JUMPS : 0;
            jumpTimer = 0;
        }

        private void FixedUpdate()
        {
            if (state.IsOnFoot())
            {
                // Horizontal Movement
                if (input.IsMovingLeft())
                {
                    MoveLeft();
                }
                else if (input.IsMovingRight())
                {
                    MoveRight();
                }
                else
                {
                    moveDirection = new Vector3(0, moveDirection.y, 0);
                }

                // Vertical Movement
                if (input.IsJumping())
                {
                    JumpCheck();
                }

                if (isJumping)
                {
                    moveDirection = new Vector3(moveDirection.x, jumpSpeed * Time.fixedDeltaTime, 0);
                }
                else
                {
                    if (!GroundCheck())
                    {
                        Fall();
                    }
                    else
                    {
                        rb.MovePosition(new Vector2(rb.position.x, RoundToNearestHalf(rb.position.y)));
                        moveDirection = new Vector3(moveDirection.x, 0, 0);
                    }
                }
                Move();
            }
            else if (state.IsClimbing())
            {
                if (input.IsJumping())
                {
                    // Jump off vine
                    isJumping = true;
                    jumpTimer = 0;
                    state.OnFoot();
                    return;
                }

                if (input.IsClimbingUp())
                {
                    // Climb up vine
                    RaycastHit2D[] hits = Physics2D.RaycastAll(rb.position, Vector2.up, 0.25f);
                    bool found = false;
                    for (int i = 0; i < hits.Length; i++)
                    {
                        RaycastHit2D hit = hits[i];
                        if (hit.transform.CompareTag("VineTop"))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        moveDirection = Vector2.zero;
                    }
                    else
                    {
                        moveDirection = new Vector2(0, climbSpeed * Time.fixedDeltaTime);
                    }
                }
                else if (input.IsClimbingDown())
                {
                    // Climb down vine
                    RaycastHit2D[] hits = Physics2D.RaycastAll(rb.position, Vector2.down, 0.25f);
                    bool found = false;
                    for (int i = 0; i < hits.Length; i++)
                    {
                        RaycastHit2D hit = hits[i];
                        if (hit.transform.CompareTag("VineBottom"))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        moveDirection = Vector2.zero;
                    }
                    else
                    {
                        moveDirection = new Vector2(0, -climbSpeed * Time.fixedDeltaTime);
                    }
                }
                else
                {
                    moveDirection = Vector2.zero;
                }

                Move();
            }
        }

        private void Update()
        {
            WallCheck();

            // Vertical Movement
            if (state.IsOnFoot())
            {
                ClimbCheck();
                JumpTimer();
                JumpReset();
            }
            else if (state.IsOnLeftWall())
            {
                if (input.IsJumping() && input.IsMovingRight())
                {
                    state.OnWallJump();
                    StartCoroutine(LeftWallJump());
                }
            }
            else if (state.IsOnRightWall())
            {
                if (input.IsJumping() && input.IsMovingLeft())
                {
                    state.OnWallJump();
                    StartCoroutine(RightWallJump());
                }
            }
        }
        private void MoveLeft()
        {
            if (!LeftWallCheck())
            {
                moveDirection = new Vector3(-moveSpeed * Time.fixedDeltaTime, moveDirection.y, 0);
            }
            else
            {
                // Snap to nearest half
                transform.position = new Vector2(RoundToNearestHalf(rb.position.x), rb.position.y);

                // Can't move right
                moveDirection = new Vector3(0, moveDirection.y, 0);
            }
        }
        private void MoveRight()
        {
            if (!RightWallCheck())
            {
                moveDirection = new Vector3(moveSpeed * Time.fixedDeltaTime, moveDirection.y, 0);
            }
            else
            {
                // Snap to nearest half
                transform.position = new Vector2(RoundToNearestHalf(rb.position.x), rb.position.y);

                // Can't move left
                moveDirection = new Vector3(0, moveDirection.y, 0);
            }
        }
        private void Move()
        {
            if (moveDirection == Vector2.zero)
            {
                return;
            }

            rb.MovePosition(transform.position + (Vector3)moveDirection);
        }
        private void WallCheck()
        {
            if (input.IsMovingLeft() && LeftWallCheck() && !GroundCheck())
            {
                transform.position = new Vector2(RoundToNearestHalf(rb.position.x), rb.position.y);
                state.OnLeftWall();
            }
            else if (input.IsMovingRight() && RightWallCheck() && !GroundCheck())
            {
                transform.position = new Vector2(RoundToNearestHalf(rb.position.x), rb.position.y);
                state.OnRightWall();
            }
        }
        private void ClimbCheck()
        {
            // First check if the player overlaps with vine
            Collider2D[] collisions = Physics2D.OverlapBoxAll(rb.position, bc.size, 0);
            for (int i = 0; i < collisions.Length; i++)
            {
                Collider2D collision = collisions[i];
                if (collision.CompareTag("Vine") && input.IsClimbingUp())
                {
                    transform.position = new Vector2(collision.transform.position.x, transform.position.y);
                    moveDirection = Vector2.zero;
                    state.OnClimb();
                    break;
                }
            }
        }
        private void JumpCheck()
        {
            // Check if can jump
            if (!GroundCheck() || jumpCount <= 0 || isJumping)
            {
                return;
            }

            isJumping = true;
            jumpTimer = 0;
            jumpCount--;
        }
        private void JumpTimer()
        {
            if (!isJumping)
            {
                return;
            }

            if (jumpTimer >= jumpLength || CeilingCheck())
            {
                // Stop ascending    
                isJumping = false;
                jumpTimer = 0;
            }
            else
            {
                jumpTimer += Time.deltaTime;
            }
        }
        private void Fall()
        {
            moveDirection = new Vector3(moveDirection.x, -fallSpeed * Time.fixedDeltaTime, 0);
        }
        private bool GroundCheck()
        {
            int count = rb.Cast(Vector2.down, collisionFilter, hits, 0.1f);
            return count > 0;
        }
        private bool CeilingCheck()
        {
            int count = rb.Cast(Vector2.up, collisionFilter, hits, 0.2f);
            return count > 0;
        }
        private bool LeftWallCheck()
        {
            int count = rb.Cast(Vector2.left, collisionFilter, hits, 0.1f);
            return count > 0;
        }
        private bool RightWallCheck()
        {
            int count = rb.Cast(Vector2.right, collisionFilter, hits, 0.1f);
            return count > 0;
        }
        private void JumpReset()
        {
            if (GroundCheck())
            {
                jumpCount = MAX_JUMPS;
            }
        }
        private IEnumerator LeftWallJump()
        {
            float currX = rb.position.x;
            float currY = rb.position.y;
            float timer = 0;
            while (timer < wallJumpLength)
            {
                if (GroundCheck() || RightWallCheck())
                {
                    state.OnFoot();
                    break;
                }

                ClimbCheck();
                if (state.IsClimbing())
                {
                    break;
                }

                timer += Time.deltaTime;
                float x = currX + wallJumpDistance * timer / wallJumpLength;
                float y = currY + 7 * timer + 3 * timer * timer;
                rb.MovePosition(new Vector3(x, y, 0));
                yield return null;
            }

            isJumping = false;
            jumpTimer = 0;
            moveDirection = Vector2.zero;
            state.OnFoot();
        }
        private IEnumerator RightWallJump()
        {
            float currX = rb.position.x;
            float currY = rb.position.y;
            float timer = 0;
            while (timer < wallJumpLength)
            {
                if (GroundCheck() || LeftWallCheck())
                {
                    state.OnFoot();
                    break;
                }

                ClimbCheck();
                if (state.IsClimbing())
                {
                    break;
                }

                timer += Time.deltaTime;
                float x = currX - wallJumpDistance * timer / wallJumpLength;
                float y = currY + 7 * timer + 3 * timer * timer;

                rb.MovePosition(new Vector3(x, y, 0));
                yield return null;
            }

            isJumping = false;
            jumpTimer = 0;
            moveDirection = Vector2.zero;
            state.OnFoot();
        }
        private float RoundToNearestHalf(float val)
        {
            return Mathf.Round(val * 2) / 2;
        }

        // Speed helper functions
        public void SpeedBoost(float multiplier)
        {
            if (climbSpeed == defaultClimbSpeed)
            {
                climbSpeed *= multiplier;
            }

            if (moveSpeed == defaultMoveSpeed)
            {
                moveSpeed *= multiplier;
            }
        }
        public void ResetSpeed()
        {
            climbSpeed = defaultClimbSpeed;
            moveSpeed = defaultMoveSpeed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                SceneController.instance.OnPlayerDeath();
            }
        }
    }
}
