using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerBaseState
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

        Debug.Log("Entered in Air State");
    }

    public override void OnUpdate(PlayerStateManager player)
    {
        float playerRadius = 0.2f;
        float landingGroundCheckDistance = 0.5f;
        #region Switch to LandingState
        if (Physics.SphereCast(player.transform.position, playerRadius, Vector3.down, out _, landingGroundCheckDistance))
        {
            player.SwitchState(player.landingState);
        }
        #endregion

        else
        {
            Debug.Log("State didn't change (Air)");
        }
    }
}
