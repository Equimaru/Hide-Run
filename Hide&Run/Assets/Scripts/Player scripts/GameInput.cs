using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public bool jump;

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
        return false;
    }


    public bool GetJumpInput()
    {
        inputActions.Player.Jump.performed += i => jump = true;
        
        return jump;
    }

    public bool GetCrouchInput()
    {
        if (inputActions.Player.Crouch.ReadValue<float>() != 0f)
        {
            return true;
        }
        return false;
    }

    
}
