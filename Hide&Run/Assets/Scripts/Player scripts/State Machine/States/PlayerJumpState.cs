using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    GameInput gameInput;
    PlayerColliderManager playerColliderManager;
    PlayerAnimator playerAnimator;

    bool jumpInProgress;

    public override void OnEnter(PlayerStateManager player)
    {
        gameInput = GameObject.Find("Game Input").GetComponent<GameInput>();
        playerColliderManager = player.GetComponent<PlayerColliderManager>();
        playerColliderManager.PlayerColliderAdjustment(player);

        playerAnimator = GameObject.Find("Player Visual").GetComponent<PlayerAnimator>();
        playerAnimator.AnimatorBooleansHandle(player);

        playerAnimator.JumpEnded += OnJumpEnded;

        jumpInProgress = true;
        Debug.Log("Entered in Jump State");
    }

    public override void OnUpdate(PlayerStateManager player)
    {
        if (jumpInProgress) return;

        float playerRadius = 0.2f;
        float landingGroundCheckDistance = 0.3f;
        #region Switch to InAirState
        if (!Physics.CheckSphere(player.transform.position, playerRadius))
        {
            player.SwitchState(player.inAirState);
        }
        #endregion

        #region Switch to LandingState
        else if (Physics.SphereCast(player.transform.position, playerRadius, Vector3.down, out _, landingGroundCheckDistance))
        {
            player.SwitchState(player.landingState);
        }
        #endregion

        #region Switch to IdleState
        else if (gameInput.GetInputVectorNormalized() == Vector2.zero && !Physics.CheckSphere(player.transform.position, playerRadius))
        {
            player.SwitchState(player.idleState);
        }
        #endregion
    }

    public void OnJumpEnded(object sourse, EventArgs e)
    {
        jumpInProgress = false;
        Debug.Log("Event worked");
    }
}
