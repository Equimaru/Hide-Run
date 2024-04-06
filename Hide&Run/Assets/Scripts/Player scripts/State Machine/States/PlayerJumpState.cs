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

    private LayerMask ground;

    public override void OnEnter(PlayerStateManager player)
    {
        gameInput = GameObject.Find("Game Input").GetComponent<GameInput>();
        playerColliderManager = player.GetComponent<PlayerColliderManager>();
        playerColliderManager.PlayerColliderAdjustment(player);

        playerAnimator = GameObject.Find("Player Visual").GetComponent<PlayerAnimator>();
        playerAnimator.AnimatorBooleansHandle(player);

        ground = LayerMask.NameToLayer("Ground");

        playerAnimator.JumpEnded += OnJumpEnded;

        jumpInProgress = true;
        Debug.Log("Entered in Jump State");
    }

    public override void OnUpdate(PlayerStateManager player)
    {
        if (jumpInProgress) return;

        float playerRadius = 0.2f;
        float landingGroundCheckDistance = 0.5f;

        #region Switch to LandingState
        if (Physics.SphereCast(player.transform.position, playerRadius, Vector3.down, out _, landingGroundCheckDistance))
        {
            player.SwitchState(player.landingState);
        }
        #endregion

        #region Switch to InAirState
        else if (!Physics.CheckSphere(player.transform.position, playerRadius, ground))
        {
            player.SwitchState(player.inAirState);
        }
        #endregion

        else
        {
            Debug.Log("State didn't change (Jump)");
        }

    }

    public void OnJumpEnded(object sourse, EventArgs e)
    {
        jumpInProgress = false;
        Debug.Log("Event worked");
    }
}
