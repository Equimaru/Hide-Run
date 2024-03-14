using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator playerAnimator;

    private string MOVEMENT_SPEED { get; } = "MovementSpeed";
    private string IS_CROUCHING { get; } = "IsCrouching";
    private string IS_IN_THE_AIR { get; } = "IsInTheAir";

    private void Update()
    {
        playerAnimator.SetFloat(MOVEMENT_SPEED, playerMovement.CurrentPlayerSpeed());
        playerAnimator.SetBool(IS_CROUCHING, playerMovement.IsCrouching());
    }

}
