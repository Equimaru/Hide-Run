using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCrouchIdleState : PlayerBaseState 
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

        Debug.Log("Entered in Crouch Idle State");
    }

    public override void OnUpdate(PlayerStateManager player)
    {
        
    }
}
