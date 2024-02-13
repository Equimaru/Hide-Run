using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    private InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }


    public Vector2 GetInputVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public bool GetRunInput()
    {
        if (inputActions.Player.Run.ReadValue<float>() != 0f)
        {
            return true;
        }
        else return false;
    }

    //public bool GetAttackInput()
    //{
    //    if (inputActions.Player.Attack.ReadValue<float>() != 0f)
    //    {
    //        return true;
    //    }
    //    else return false;
    //}

    public bool GetJumpInput()
    {
        if (inputActions.Player.Jump.ReadValue<float>() != 0f)
        {
            return true;
        }
        return false;
    }

    
}