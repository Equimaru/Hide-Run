using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameInput gameInput;

    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private Transform freeLookCamera;

    [SerializeField] private Transform groundCheckPoint;

    [SerializeField] private LayerMask ground;

    private float movementSpeed,
        crouchingSpeed = 2f,
        walkingSpeed = 4f,
        runningSpeed = 7f;

    private float jumpForce = 2f;

    private float playerRadius = 0.4f;
    private float currentVelocity;
    private float smoothTime = 0.1f;

    private bool isMoving,
        isRunning,
        isCrouching,
        isGrounded;

    private Rigidbody rb;

    private Vector3 cameraRelativeMoveDir;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
    }


    private void Update()
    {
        
        StateHandler();
    }

    private void FixedUpdate()
    {
        CharacterRotation();
        MovementHandler();
    }


    public MovementState state;
    public enum MovementState
    {
        idle,
        walking,
        running,
        crouch_idle,
        crouch_moving,
        air
    }

    private void StateHandler()
    {
        if (gameInput.GetInputVectorNormalized() == Vector2.zero && gameInput.GetCrouchInput() == true)
        {
            state = MovementState.crouch_idle;

            isCrouching = true;
            isMoving = false;
            isRunning = false;
        }
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetCrouchInput() == true)
        {
            state = MovementState.crouch_moving;
            movementSpeed = crouchingSpeed;

            isCrouching = true;
            isMoving = true;
            isRunning = false;
        }
        else if (gameInput.GetInputVectorNormalized() == Vector2.zero)
        {
            state = MovementState.idle;

            isCrouching = false;
            isMoving = false;
            isRunning = false;
        }
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetRunInput() == false)
        {
            state = MovementState.walking;
            movementSpeed = walkingSpeed;

            isCrouching = false;
            isMoving = true;
            isRunning = false;
        }
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetRunInput() == true)
        {
            state = MovementState.running;
            movementSpeed = runningSpeed;

            isCrouching = false;
            isMoving = true;
            isRunning = true;
        }
        

        //Debug.Log(state.ToString());
    }

    private Vector3 CamRelativeMovementDir()
    {
        Vector2 inputVector = gameInput.GetInputVectorNormalized();

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        cameraRelativeMoveDir = cameraForward * moveDir.z + cameraRight * moveDir.x;
        cameraRelativeMoveDir.y = 0;
        return cameraRelativeMoveDir;
    }

    public Vector2 InputVector()
    {
        return gameInput.GetInputVectorNormalized();
    }

   

    private void MovementHandler()
    {

        //Check if grounded
        {
            float groundCheckDistance = 0.2f;
            if (Physics.SphereCast(groundCheckPoint.transform.position, playerRadius, Vector3.down, out _, groundCheckDistance))
            {
                isGrounded = true;
                //Debug.Log("Is grounded");
            }
            else
            {
                isGrounded = false;
                //Debug.Log("Isn't grounded");
            }
        }

        //Camera relative movement
        {
            if (isGrounded)
            {
                Vector3 cameraRelativeMoveDir = CamRelativeMovementDir();
                
                Vector3 velocityVector = cameraRelativeMoveDir * movementSpeed;

                rb.velocity = new Vector3(velocityVector.x, rb.velocity.y, velocityVector.z);

                if (gameInput.GetJumpInput())
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
            }

        }

    }


    private void CharacterRotation()
    {
        Vector3 cameraRelativeMoveDir = CamRelativeMovementDir();

        if (cameraRelativeMoveDir.magnitude == 0) return;

        var targetAngle = Mathf.Atan2(cameraRelativeMoveDir.x, cameraRelativeMoveDir.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheckPoint.transform.position, playerRadius);
    }

    public bool IsMoving()
    {
        return isMoving;
    }
    public bool IsRunning()
    {
        return isRunning;
    }
    public bool IsCrouching()
    {
        return isCrouching;
    }
}
