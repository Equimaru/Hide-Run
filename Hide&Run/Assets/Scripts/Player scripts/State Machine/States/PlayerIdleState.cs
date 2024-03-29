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
        #region Switch to InAir
        float groundCheckDistance = 1.1f;
        float playerRadius = 0.4f;
        if (!Physics.SphereCast(player.transform.position, playerRadius, Vector3.down, out _, groundCheckDistance))
        {
            player.SwitchState(player.inAirState);
        }
        #endregion

        #region Switch to Walk
        if (gameInput.GetInputVectorNormalized() != Vector2.zero && !gameInput.GetRunInput())
        {
            player.SwitchState(player.walkState);
        }
        #endregion
    }
}
