using System;
using UnityEngine;

namespace Player
{
    public class PlayerStateManager : MonoBehaviour
    {
        public static Action<MoveState> NewMoveState;

        private MoveState _currentMoveState;
        private PlayerMovement _playerMovement;

        private void Start()
        {
            _playerMovement  = GetComponent<PlayerMovement>();
            
            ChangeMoveState(MoveState.Idle);
        }

        private void Update()
        {
            UpdateMoveState();
        }
        
        private void UpdateMoveState()
        {
            //Jump
            if (!_playerMovement.IsGround)
            {
                ChangeMoveState(MoveState.InAir);
                return;
            }
            
            //Movement
            if (_playerMovement.MoveInput.magnitude > 0)
            {
                if (_playerMovement.IsAcceleratedMove)
                {
                    if (_playerMovement.Booster > 1)
                    {
                        ChangeMoveState(MoveState.BoostedRun);
                    }
                    else
                    {
                        ChangeMoveState(MoveState.Run);
                    }
                }
                else
                {
                    ChangeMoveState(MoveState.Move);
                }

                return;
            }

            //Idle
            ChangeMoveState(MoveState.Idle);
        }

        private void ChangeMoveState(MoveState moveState)
        {
            if (_currentMoveState == moveState) return;
            
            Debug.Log($"CurrentMoveState {moveState}");
            NewMoveState?.Invoke(moveState);
            _currentMoveState = moveState;
        }
    }
    
    public enum MoveState
    {
        Idle,
        InAir,
        Move,
        Run,
        BoostedRun
    }
}