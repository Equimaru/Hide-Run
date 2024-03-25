using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator playerAnimator;

    private float playerSpeedForAnimation,
        playerMaxSpeedForAnimation,
        playerAccelerationForAnimation;

    private bool jumpToFallTranstionStarted;

    private string MOVEMENT_SPEED { get; } = "MovementSpeed";
    private string IS_CROUCHING { get; } = "IsCrouching";
    private string IS_FAST_ENOUGH { get; } = "IsFastEnough";
    private string IS_GROUNDED { get; } = "IsGrounded";
    private string IS_ON_FOOT { get; } = "IsOnFoot";
    private string JUMP_PERFORMED { get; } = "JumpPerformed";
    private string RUNNING_SLIDE { get; } = "Running Slide";
    private string JUMPING_UP { get; } = "Jumping Up";
    private string FALLING_IDLE { get; } = "Falling Idle";

    private void Awake()
    {
        playerSpeedForAnimation = 0;
        playerAccelerationForAnimation = 0.1f;

        jumpToFallTranstionStarted = false;
    }

    private void FixedUpdate()
    {
        playerMaxSpeedForAnimation = playerMovement.maxMovementSpeed;

        playerAnimator.SetFloat(MOVEMENT_SPEED, PlayerSpeedForAnimation());

        playerAnimator.SetBool(IS_CROUCHING, playerMovement.IsCrouching());
        playerAnimator.SetBool(IS_GROUNDED, playerMovement.IsGrounded());
        playerAnimator.SetBool(IS_ON_FOOT, playerMovement.IsOnFoot());
        playerAnimator.SetBool(IS_FAST_ENOUGH, playerMovement.IsPlayerFastEnough());
        playerAnimator.SetBool(JUMP_PERFORMED, playerMovement.JumpPerformed());
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

    void JumpEvent()
    {
        playerMovement.CharacterJump();
    }
}
