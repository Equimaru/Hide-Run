using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderManager : MonoBehaviour
{
    #region Parameters for colliders
    [SerializeField] private Transform standingCenterPoint;
    [SerializeField] private Transform crouchingCenterPoint;
    [SerializeField] private Transform slidingCenterPoint;

    private float standingHeight = 1.82f;
    private float crouchingHeight = 1.2f;
    private float slidingHeight = 0.8f;

    private float standingRadius = 0.34f;
    private float crouchingRadius = 0.5f;
    private float slidingRadius = 0.4f;
    #endregion

    private CapsuleCollider playerCapsuleCollider;

    private void Start()
    {
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void PlayerColliderAdjustment(PlayerStateManager player)
    {
        Transform center;
        float height, radius;
        
        if (player.state == player.crouchIdleState || player.state == player.crouchMoveState)
        {
            center = crouchingCenterPoint;
            height = crouchingHeight;
            radius = crouchingRadius;
        }
        else if (player.state == player.slideState)
        {
            center = slidingCenterPoint;
            height = slidingHeight;
            radius = slidingRadius;
        }
        else
        {
            center = standingCenterPoint;
            height = standingHeight;
            radius = standingRadius;
        }

        playerCapsuleCollider.center = center.transform.position - player.transform.position;
        playerCapsuleCollider.height = height;
        playerCapsuleCollider.radius = radius;
    }
}
