using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public bool jump;

    private bool runImputActive;

    private InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    private void OnEnable()
    {
        inputActions.Player.Run.started += i => runImputActive = true;
        inputActions.Player.Run.canceled += i => runImputActive = false;
    }

    public Vector2 GetInputVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public bool GetRunInput()
    {
        return runImputActive;
    }

   

    public bool GetJumpInput()
    {
        inputActions.Player.Jump.performed += i => jump = true;
        bool _jump = jump;
        jump = false;
        return _jump;
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
