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

    //Used in PlayerAnimator as reference
    public float maxMovementSpeed;
    public float movementSpeedAcceleration = 0.25f;

    private float currentMovementSpeed,
        crouchingSpeed = 2f,
        walkingSpeed = 4f,
        runningSpeed = 7f;

    private float jumpForce = 2f;

    private float playerRadius = 0.4f;
    private float currentVelocity,
       currentVelocityVector;
    private float smoothTime = 0.1f;

    private bool isCrouching,
        isGrounded;

    private Rigidbody rb;

    private Vector3 cameraRelativeMoveDir;

    private void Awake()
    {
        currentMovementSpeed = 0f;
    }
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
        }
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetCrouchInput() == true)
        {
            state = MovementState.crouch_moving;
            maxMovementSpeed = crouchingSpeed;

            isCrouching = true;
        }
        else if (gameInput.GetInputVectorNormalized() == Vector2.zero)
        {
            state = MovementState.idle;

            isCrouching = false;
        }
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetRunInput() == false)
        {
            state = MovementState.walking;
            maxMovementSpeed = walkingSpeed;

            isCrouching = false;
        }
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetRunInput() == true)
        {
            state = MovementState.running;
            maxMovementSpeed = runningSpeed;

            isCrouching = false;
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
            Vector3 currentVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            currentMovementSpeed = currentVelocity.magnitude;
            if (isGrounded)
            {
                Vector3 cameraRelativeMoveDir = CamRelativeMovementDir();

                if (gameInput.GetInputVectorNormalized() != Vector2.zero)
                {
                    if (currentVelocity.magnitude < maxMovementSpeed)
                    {
                        currentMovementSpeed += movementSpeedAcceleration;
                    }
                    else if (currentVelocity.magnitude > maxMovementSpeed)
                    {
                        currentMovementSpeed -= movementSpeedAcceleration;
                    }
                }
                else
                {
                    if (currentMovementSpeed != 0)
                    {
                        currentMovementSpeed -= movementSpeedAcceleration;
                    }
                }


                Vector3 velocityVector = (cameraRelativeMoveDir + currentVelocity.normalized).normalized * currentMovementSpeed;

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

    public float CurrentPlayerSpeed()
    {
        //Debug.Log(currentMovementSpeed);
        return currentMovementSpeed;
    }
    public bool IsCrouching()
    {
        return isCrouching;
    }

    public Vector3 GetCameraRelatedMovementDir()
    {
        return cameraRelativeMoveDir;
    }
}
