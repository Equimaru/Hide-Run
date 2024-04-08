using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSlideState : PlayerBaseState
{
    GameInput gameInput;
    PlayerMovementManager playerMovementManager;
    PlayerColliderManager playerColliderManager;
    PlayerAnimator playerAnimator;

    private bool slideInProgress;

    public override void OnEnter(PlayerStateManager player)
    {
        gameInput = GameObject.Find("Game Input").GetComponent<GameInput>();
        playerColliderManager = player.GetComponent<PlayerColliderManager>();
        playerColliderManager.PlayerColliderAdjustment(player);

        playerMovementManager = player.GetComponent<PlayerMovementManager>();

        playerAnimator = GameObject.Find("Player Visual").GetComponent<PlayerAnimator>();
        playerAnimator.AnimatorBooleansHandle(player);

        playerAnimator.SlideEnded += OnSlideEnded;

        slideInProgress = true;
        Debug.Log("Entered in Slide State");
    }

    public override void OnUpdate(PlayerStateManager player)
    {
        if (slideInProgress)
        {
            Debug.Log("State didn't change (Slide)");
            return;
        }

        #region Switch to CrouchMoveState
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetCrouchInput())
        {
            player.SwitchState(player.crouchMoveState);
        }
        #endregion

        #region Switch to WalkState
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && !gameInput.GetRunInput())
        {
            player.SwitchState(player.walkState);
        }
        #endregion

        #region Switch to RunState
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetRunInput())
        {
            player.SwitchState(player.runState);
        }
        #endregion

        else
        {
            Debug.Log("State didn't change (Slide)");
        }
    }

    public void OnSlideEnded(object sourse, EventArgs e)
    {
        slideInProgress = false;
        Debug.Log("Event worked (Slide)");
    }
}
