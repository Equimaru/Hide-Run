using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    GameInput gameInput;

    public override void OnEnter(PlayerStateManager player)
    {
        player.GetComponent<PlayerColliderManager>().PlayerColliderAdjustment(player);
        player.GetComponent<PlayerAnimator>().AnimatorBooleansHandle(player);

        gameInput = player.GetComponent<GameInput>();
    }

    public override void OnUpdate(PlayerStateManager player)
    {
        #region Switch to InAirState
        float groundCheckDistance = 1.1f;
        float playerRadius = 0.4f;
        if (!Physics.SphereCast(player.transform.position, playerRadius, Vector3.down, out _, groundCheckDistance))
        {
            player.SwitchState(player.inAirState);
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

        #region Switch to CrouchIdleState
        else if (gameInput.GetInputVectorNormalized() == Vector2.zero && gameInput.GetCrouchInput())
        {
            player.SwitchState(player.crouchIdleState);
        }
        #endregion

        #region Switch to CrouchMoveState
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetCrouchInput())
        {
            player.SwitchState(player.crouchMoveState);
        }
        #endregion

        #region Switch to SlideState
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetCrouchInput())
        {
            player.SwitchState(player.slideState);
        }
        #endregion

        #region Switch to JumpState
        else if (gameInput.GetJumpInput())
        {
            player.SwitchState(player.crouchMoveState);
        }
        #endregion
        else
        {
            Debug.Log("State didn't change");
        }
    }
}
