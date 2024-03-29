using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingState : PlayerBaseState
{
    public override void OnEnter(PlayerStateManager player)
    {
        player.GetComponent<PlayerColliderManager>().PlayerColliderAdjustment(player);
        player.GetComponent<PlayerAnimator>().AnimatorBooleansHandle(player);
    }

    public override void OnUpdate(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }
}
