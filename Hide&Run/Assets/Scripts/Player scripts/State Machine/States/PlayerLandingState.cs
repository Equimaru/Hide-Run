using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingState : PlayerBaseState
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

        Debug.Log("Entered in Landing State");
    }

    public override void OnUpdate(PlayerStateManager player)
    {
        
    }
}
