using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle
{
    // This script keeps track of the player's state
    public class CharacterStateController : MonoBehaviour
    {
        public enum State
        {
            OnFoot,
            LeftWall,
            RightWall,
            WallJump,
            ClimbingVine
        }
        [SerializeField] private State state;
        private void Awake()
        {
            state = State.OnFoot;
        }
        public State GetCharacterState()
        {
            return state;
        }
        public void OnFoot()
        {
            state = State.OnFoot;
        }
        public void OnLeftWall()
        {
            state = State.LeftWall;
        }
        public void OnRightWall()
        {
            state = State.RightWall;
        }
        public void OnWallJump()
        {
            state = State.WallJump;
        }
        public void OnClimb()
        {
            state = State.ClimbingVine;
        }
        public bool IsOnFoot()
        {
            return state == State.OnFoot;
        }
        public bool IsOnLeftWall()
        {
            return state == State.LeftWall;
        }
        public bool IsOnRightWall()
        {
            return state == State.RightWall;
        }
        public bool IsClimbing()
        {
            return state == State.ClimbingVine;
        }
        public bool IsWallJumping()
        {
            return state == State.WallJump;
        }
    }
}
