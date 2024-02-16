using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAnimator : MonoBehaviour
{

    [SerializeField] private HunterMovement hunterMovement;
    [SerializeField] private Animator hunterAnimator;

    private string IS_RUNNING { get; } = "IsRunning";


    private void Update()
    {
        hunterAnimator.SetBool(IS_RUNNING, hunterMovement.IsRunning());
    }
}
