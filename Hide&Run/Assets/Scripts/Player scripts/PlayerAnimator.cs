using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] private Player player;
    [SerializeField] private Animator playerAnimator;
    #endregion

    private PlayerMovementManager playerMovementManager;

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

    public delegate void JumpEndedEventHandler(object soure, EventArgs args);
    public delegate void SlideEndedEventHandler(object soure, EventArgs args);
    public delegate void GettingUpSlideEventHandler(object soure, EventArgs args);

    public event JumpEndedEventHandler JumpEnded;
    public event SlideEndedEventHandler SlideEnded;
    public event GettingUpSlideEventHandler GettingUpSlide;

    private void Awake()
    {
        playerSpeedForAnimation = 0;
        playerAccelerationForAnimation = 2f;
    }

    private void Start()
    {
        playerMovementManager = GameObject.Find("Player").GetComponent<PlayerMovementManager>();
    }

    private void FixedUpdate()
    {
        playerMaxSpeedForAnimation = playerMovementManager.maxMovementSpeed * 10;

        playerAnimator.SetFloat(MOVEMENT_SPEED, PlayerSpeedForAnimation());
    }

    private float PlayerSpeedForAnimation()
    {
        if (playerMovementManager.GetCameraRelatedMovementDir() != Vector3.zero)
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

    private void JumpEvent()
    {
        playerMovementManager.CharacterJump();
    }

    private void EndJumpEvent()
    {
        OnJumpEnded();
    }

    private void EndSlideEvent()
    {
        OnSlideEnded();
    }

    private void GettingUpSlideEvent()
    {
        OnGettingUpSlide();
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
            playerAnimator.SetTrigger(JUMP_PERFORMED);
        }

        #endregion

        #region IsFastEnough
        if (player.state == player.slideState)
        {
            playerAnimator.SetTrigger(IS_FAST_ENOUGH);
        }

        #endregion
    }

    protected virtual void OnJumpEnded()
    {
        JumpEnded?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnSlideEnded()
    {
        SlideEnded?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnGettingUpSlide()
    {
        GettingUpSlide?.Invoke(this, EventArgs.Empty);
    }
}
