using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    GameInput gameInput;
    PlayerColliderManager playerColliderManager;
    PlayerAnimator playerAnimator;

    public override void OnEnter(PlayerStateManager player)
    {
        gameInput = GameObject.Find("Game Input").GetComponent<GameInput>();
        playerColliderManager = player.GetComponent<PlayerColliderManager>();
        playerColliderManager.PlayerColliderAdjustment(player);

        playerAnimator = GameObject.Find("Player Visual").GetComponent<PlayerAnimator>();
        playerAnimator.AnimatorBooleansHandle(player);

        Debug.Log("Entered in Jump State");
    }

    public override void OnUpdate(PlayerStateManager player)
    {
        #region Switch to InAirState
        float playerRadius = 0.2f;
        float landingGroundCheckDistance = 1f;
        if (!Physics.CheckSphere(player.transform.position, playerRadius))
        {
            Debug.Log(player.transform.position);
            player.SwitchState(player.inAirState);
        }
        #endregion

        #region Switch to LandingState
        else if (Physics.SphereCast(player.transform.position, playerRadius, Vector3.down, out _, landingGroundCheckDistance))
        {
            Debug.Log(player.transform.position);
            player.SwitchState(player.landingState);
        }
        #endregion
    }
}
