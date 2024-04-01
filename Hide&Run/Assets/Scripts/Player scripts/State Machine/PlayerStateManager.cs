using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public GameInput gameInput;
    public PlayerAnimator playerAnimator;

    #region PlayerStates
    public PlayerBaseState state;
    public PlayerIdleState idleState = new();
    public PlayerWalkState walkState = new();
    public PlayerRunState runState = new();
    public PlayerCrouchIdleState crouchIdleState = new();
    public PlayerCrouchMoveState crouchMoveState = new();
    public PlayerSlideState slideState = new();
    public PlayerJumpState jumpState = new(); 
    public PlayerInAirState inAirState = new();
    public PlayerLandingState landingState = new();
    #endregion

    void Start()
    {
        state = idleState;

        state.OnEnter(this);
    }


    void FixedUpdate()
    {
        state.OnUpdate(this);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        state = newState;
        state.OnEnter(this);
    }
}
