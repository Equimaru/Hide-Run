using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] private Player player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator playerAnimator;
    #endregion

    private float playerSpeedForAnimation,
        playerMaxSpeedForAnimation,
        playerAccelerationForAnimation;

    #region Animator string variables
    private string MOVEMENT_SPEED { get; } = "MovementSpeed";
    private string IS_CROUCHING { get; } = "IsCrouching";
    private string IS_FAST_ENOUGH { get; } = "IsFastEnough";
    private string IS_GROUNDED { get; } = "IsGrounded";
    private string IS_ON_FOOT { get; } = "IsOnFoot";
    private string IS_LANDING { get; } = "IsLanding";
    private string JUMP_PERFORMED { get; } = "JumpPerformed";
    private string RUNNING_SLIDE { get; } = "Running Slide";
    private string JUMPING_UP { get; } = "Jumping Up";
    private string FALLING_IDLE { get; } = "Falling Idle";
    #endregion

    private void Awake()
    {
        playerSpeedForAnimation = 0;
        playerAccelerationForAnimation = 2f;
    }

    private void FixedUpdate()
    {
        playerMaxSpeedForAnimation = playerMovement.maxMovementSpeed * 10;

        playerAnimator.SetFloat(MOVEMENT_SPEED, PlayerSpeedForAnimation());

        playerAnimator.SetBool(IS_CROUCHING, playerMovement.IsCrouching());
        playerAnimator.SetBool(IS_GROUNDED, playerMovement.IsGrounded());
        playerAnimator.SetBool(IS_LANDING, playerMovement.IsLanding());
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
        
        return Mathf.Round(playerSpeedForAnimation);
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


    public void AnimatorBooleansHandle(PlayerStateManager player)
    {
        #region IsOnFoot
        if (player.state == player.slideState || player.state == player.inAirState || player.state == player.landingState)
        {
            playerAnimator.SetBool(IS_ON_FOOT, false);
        }
        else
        {
            playerAnimator.SetBool(IS_ON_FOOT, true);
        }
        #endregion

        #region IsCrouching
        if (player.state == player.crouchIdleState || player.state == player.crouchMoveState)
        {
            playerAnimator.SetBool(IS_CROUCHING, true);
        }
        else
        {
            playerAnimator.SetBool(IS_CROUCHING, false);
        }
        #endregion

        #region IsGrounded
        if (player.state == player.inAirState || player.state == player.landingState)
        {
            playerAnimator.SetBool(IS_GROUNDED, false);
        }
        else
        {
            playerAnimator.SetBool(IS_GROUNDED, true);
        }
        #endregion

        #region IsLanding
        if (player.state == player.landingState)
        {
            playerAnimator.SetBool(IS_LANDING, true);
        }
        else
        {
            playerAnimator.SetBool(IS_LANDING, false);
        }
        #endregion

        #region JumpPerformed
        if (player.state == player.jumpState)
        {
            playerAnimator.SetBool(JUMP_PERFORMED, true);
        }
        else
        {
            playerAnimator.SetBool(JUMP_PERFORMED, false);
        }
        #endregion
    }
}
