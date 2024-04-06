using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    GameInput gameInput;
    PlayerMovementManager playerMovementManager;
    PlayerColliderManager playerColliderManager;
    PlayerAnimator playerAnimator;

    public override void OnEnter(PlayerStateManager player)
    {
        gameInput = GameObject.Find("Game Input").GetComponent<GameInput>();
        playerColliderManager = player.GetComponent<PlayerColliderManager>();
        playerColliderManager.PlayerColliderAdjustment(player);

        playerMovementManager = player.GetComponent<PlayerMovementManager>();

        playerAnimator = GameObject.Find("Player Visual").GetComponent<PlayerAnimator>();
        playerAnimator.AnimatorBooleansHandle(player);

        Debug.Log("Entered in Run State");
    }

    public override void OnUpdate(PlayerStateManager player)
    {
        #region Switch to InAirState
        float playerRadius = 0.2f;
        if (!Physics.CheckSphere(player.transform.position, playerRadius))
        {
            Debug.Log(player.transform.position);
            player.SwitchState(player.inAirState);
        }
        #endregion

        #region Switch to JumpState
        else if (gameInput.GetJumpInput())
        {
            player.SwitchState(player.jumpState);
        }
        #endregion

        #region Switch to SlideState
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetRunInput() && gameInput.GetCrouchInput())
        {
            player.SwitchState(player.slideState);
        }
        #endregion

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

        #region Switch to IdleState
        else if (gameInput.GetInputVectorNormalized() == Vector2.zero)
        {
            player.SwitchState(player.idleState);
        }
        #endregion

        else
        {
            Debug.Log("State didn't change (Run)");
        }

        playerMovementManager.CharacterRotation();
        playerMovementManager.MovementHandler(player);
    }
}
