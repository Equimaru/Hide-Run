using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Animator animator;

    private string IS_WALKING { get; } = "IsMoving";
    private string IS_RUNNING { get; } = "IsRunning";
    private string IS_ATTACKING { get; } = "IsAttacking";
    private string IS_IN__THE_AIR { get; } = "IsInTheAir";
    //private string HORIZONTAL_LOCAL_MOVEMENT { get; } = "horizontalLocalMovement";
    //private string VERTICAL_LOCAL_MOVEMENT { get; } = "verticalLocalMovement";

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsMoving());
        animator.SetBool(IS_RUNNING, player.IsRunning());
        //animator.SetBool(IS_ATTACKING, player.IsAttacking());
        //animator.SetFloat(VERTICAL_LOCAL_MOVEMENT, player.InputVector().y);
        //animator.SetFloat(HORIZONTAL_LOCAL_MOVEMENT, player.InputVector().x);
    }

    public bool AttackAnimationIsPlaying()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return true;
        }
        else return false;
    }

    public void AttackAttemptTriggered()
    {

    }
}
