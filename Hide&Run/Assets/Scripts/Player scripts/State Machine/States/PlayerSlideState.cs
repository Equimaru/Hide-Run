using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : PlayerBaseState
{
    public override void OnEnter(PlayerStateManager player)
    {
        player.GetComponent<PlayerColliderManager>().PlayerColliderAdjustment(player);
        player.GetComponent<PlayerAnimator>().AnimatorBooleansHandle(player);

        Debug.Log("Entered in Slide State");
    }

    public override void OnUpdate(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }
}
