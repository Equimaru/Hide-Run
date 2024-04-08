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
        playerMovementManager.SetPlayerSlideVelocityMagnitude(7f);

        playerAnimator = GameObject.Find("Player Visual").GetComponent<PlayerAnimator>();
        playerAnimator.AnimatorBooleansHandle(player);

        playerAnimator.SlideEnded += OnSlideEnded;
        playerAnimator.GettingUpSlide += OnGettingUpSlide;

        slideInProgress = true;
        Debug.Log("Entered in Slide State");
    }

    public override void OnUpdate(PlayerStateManager player)
    {
        if (slideInProgress)
        {
            playerMovementManager.MovementHandler(player);
            Debug.Log("State didn't change (Slide)");
            return;
        }

        #region Switch to CrouchMoveState
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetCrouchInput())
        {
            player.SwitchState(player.crouchMoveState);
        }
        #endregion

        #region Switch to RunState
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetRunInput())
        {
            player.SwitchState(player.runState);
        }
        #endregion

        #region Switch to WalkState
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero)
        {
            player.SwitchState(player.walkState);
        }
        #endregion

        #region Switch to IdleState
        else if (gameInput.GetInputVectorNormalized() == Vector2.zero)
        {
            player.SwitchState(player.idleState);
        }
        #endregion
    }

    public void OnSlideEnded(object sourse, EventArgs e)
    {
        slideInProgress = false;
        Debug.Log("Event worked (Slide)");
    }

    //Replace to MovementManager similar to all velocity changes
    public void OnGettingUpSlide(object sourse, EventArgs e)
    {
        Debug.Log("Cyberbulling");
        playerMovementManager.SetPlayerSlideVelocityMagnitude(2f);
    }
}
