using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator playerAnimator;

    private string IS_WALKING { get; } = "IsMoving";
    private string IS_RUNNING { get; } = "IsRunning";
    private string IS_CROUCHING { get; } = "IsCrouching";
    private string IS_IN_THE_AIR { get; } = "IsInTheAir";

    private void Update()
    {
        playerAnimator.SetBool(IS_WALKING, playerMovement.IsMoving());
        playerAnimator.SetBool(IS_RUNNING, playerMovement.IsRunning());
        playerAnimator.SetBool(IS_CROUCHING, playerMovement.IsCrouching());
    }

}
