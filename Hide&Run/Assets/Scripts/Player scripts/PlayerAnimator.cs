using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator playerAnimator;

    private float playerSpeedForAnimation,
        playerMaxSpeedForAnimation,
        playerAccelerationForAnimation;

    private string MOVEMENT_SPEED { get; } = "MovementSpeed";
    private string IS_CROUCHING { get; } = "IsCrouching";
    private string IS_FAST_ENOUGH { get; } = "IsFastEnough";
    private string IS_IN_THE_AIR { get; } = "IsInTheAir";
    private string RUNNING_SLIDE { get; } = "Running Slide";

    private void Awake()
    {
        playerSpeedForAnimation = 0;
        playerAccelerationForAnimation = 0.05f;
    }

    private void FixedUpdate()
    {
        playerMaxSpeedForAnimation = playerMovement.maxMovementSpeed;
        playerAnimator.SetFloat(MOVEMENT_SPEED, PlayerSpeedForAnimation());
        playerAnimator.SetBool(IS_CROUCHING, playerMovement.IsCrouching());
        playerAnimator.SetBool(IS_FAST_ENOUGH, playerMovement.IsCrouching());
        Debug.Log(PlayerSpeedForAnimation());
    }

    private void Update()
    {
        
    }


    private float PlayerSpeedForAnimation()
    {
        if (playerMovement.GetCameraRelatedMovementDir() != Vector3.zero)
        {
            if (playerSpeedForAnimation < playerMaxSpeedForAnimation)
            {
                playerSpeedForAnimation += playerAccelerationForAnimation;
            }
            else if (playerSpeedForAnimation > playerMaxSpeedForAnimation)
            {
                playerSpeedForAnimation -= playerAccelerationForAnimation;
            }
        }
        else
        {
            if (playerSpeedForAnimation > 0)
            {
                playerSpeedForAnimation -= playerAccelerationForAnimation;
            }
        }
        
        return playerSpeedForAnimation;
    }

    public bool IsPlayerSliding()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(RUNNING_SLIDE))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
