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
    private string IS_STANDING { get; } = "IsStanding";
    private string IS_SLIDING { get; } = "IsSliding";
    private string IS_IN_AIR { get; } = "IsInAir";
    private string IS_LANDING { get; } = "IsLanding";
    private string JUMP_PERFORMED { get; } = "JumpPerformed";
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

    //public bool IsPlayerSliding()
    //{
    //    if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(RUNNING_SLIDE))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

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

        #region IsStanding
        if (player.state == player.idleState || player.state == player.walkState || player.state == player.runState)
        {
            playerAnimator.SetBool(IS_STANDING, true);
        }
        else
        {
            playerAnimator.SetBool(IS_STANDING, false);
        }
        #endregion

        #region IsSliding
            if (player.state == player.slideState)
        {
            playerAnimator.SetTrigger(IS_SLIDING);
        }
        #endregion

        #region IsInAir
        if (player.state == player.inAirState)
        {
            playerAnimator.SetBool(IS_IN_AIR, true);
        }
        else
        {
            playerAnimator.SetBool(IS_IN_AIR, false);
        }
        #endregion

        #region IsLanding
        if (player.state == player.landingState)
        {
            playerAnimator.SetTrigger(IS_LANDING);
        }
        #endregion

        #region JumpPerformed
        if (player.state == player.jumpState)
        {
            playerAnimator.SetTrigger(JUMP_PERFORMED);
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
