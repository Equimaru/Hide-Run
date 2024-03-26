using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private Transform freeLookCamera;
    [SerializeField] private Transform groundCheckPoint;

    [SerializeField] private LayerMask ground;
    #endregion

    #region Parameters for colliders
    [SerializeField] private Transform standingCenterPoint;
    [SerializeField] private Transform crouchingCenterPoint;
    [SerializeField] private Transform slidingCenterPoint;

    private float standingHeight = 1.82f;
    private float crouchingHeight = 1.2f;
    private float slidingHeight = 0.8f;

    private float standingRadius = 0.34f;
    private float crouchingRadius = 0.5f;
    private float slidingRadius = 0.4f;
    #endregion
    private CapsuleCollider playerCapsuleCollider;
    private Rigidbody rb;

    #region PlayerAnimator related variables
    public float maxMovementSpeed;
    public float movementSpeedAcceleration = 0.25f;
    public bool jumpPerformed;
    #endregion

    #region Movement variables
    private float movementSpeedDecceleration,
        movementSpeedDeccelerationOnFoot = 0.2f,
        movementSpeedDeccelerationSliding = 0.01f;

    private float currentMovementSpeed,
        crouchingSpeed = 2f,
        walkingSpeed = 4f,
        runningSpeed = 7f;
    #endregion

    #region Boolean variables
    private bool isCrouching,
        isGrounded,
        isOnFoot,
        isLanding;
    #endregion

    private float jumpForce = 6f;

    private float playerRadius = 0.4f;
    private float currentVelocity;
    private float smoothTime = 0.1f;

    private Vector3 cameraRelativeMoveDir;

    private void Awake()
    {
        currentMovementSpeed = 0f;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 

        playerCapsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        StateHandler();
    }

    private void FixedUpdate()
    {
        CharacterColliderAdjustment();
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
        sliding,
        air
    }

    private void StateHandler()
    {
        if (playerAnimator.IsPlayerSliding())
        {
            state = MovementState.sliding;

            isOnFoot = false;
        }
        else if (gameInput.GetInputVectorNormalized() == Vector2.zero && gameInput.GetCrouchInput() == true)
        {
            state = MovementState.crouch_idle;

            isCrouching = true;
            isOnFoot = true;
        }
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetCrouchInput() == true)
        {
            state = MovementState.crouch_moving;
            maxMovementSpeed = crouchingSpeed;

            isCrouching = true;
            isOnFoot = true;
        }
        else if (gameInput.GetInputVectorNormalized() == Vector2.zero)
        {
            state = MovementState.idle;

            isCrouching = false;
            isOnFoot = true;
        }
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetRunInput() == false)
        {
            state = MovementState.walking;
            maxMovementSpeed = walkingSpeed;

            isCrouching = false;
            isOnFoot = true;
        }
        else if (gameInput.GetInputVectorNormalized() != Vector2.zero && gameInput.GetRunInput() == true)
        {
            state = MovementState.running;
            maxMovementSpeed = runningSpeed;

            isCrouching = false;
            isOnFoot = true;
        }
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
            }
            else
            {
                isGrounded = false;
            }
        }

        //Check for landing
        {
            float landingGroundCheckDistance = 1f;
            if (Physics.SphereCast(groundCheckPoint.transform.position, playerRadius, Vector3.down, out _, landingGroundCheckDistance) && !isGrounded)
            {
                isLanding = true;
            }
            else isLanding = false;
        }

        //Camera relative movement
        {
            Vector3 currentVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            currentMovementSpeed = currentVelocity.magnitude;

            Vector3 cameraRelativeMoveDir = CamRelativeMovementDir();
            

            if (gameInput.GetInputVectorNormalized() != Vector2.zero && isOnFoot)
            {
                CharacterAcceleration(currentVelocity);
            }
            else
            {
                CharacterBraking();
            }

            Vector3 velocityVector = (cameraRelativeMoveDir.normalized + currentVelocity.normalized).normalized * currentMovementSpeed;

            rb.velocity = new Vector3(velocityVector.x, rb.velocity.y, velocityVector.z);


            if (gameInput.GetJumpInput())
            {
                gameInput.jump = false;
                jumpPerformed = true;
            }
        }
    }


    private void CharacterAcceleration(Vector3 velocity)
    {
        if (isGrounded && isOnFoot)
        {
            if (velocity.magnitude < maxMovementSpeed)
            {
                currentMovementSpeed += movementSpeedAcceleration;
            }
            else if (velocity.magnitude > maxMovementSpeed)
            {
               currentMovementSpeed -= movementSpeedAcceleration;
            }
        }
    }


    //Need rework with state machine?
    private void CharacterBraking()
    {
        if (isOnFoot)
        {
            movementSpeedDecceleration = movementSpeedDeccelerationOnFoot;
        }
        else
        {
            movementSpeedDecceleration = movementSpeedDeccelerationSliding;
        }
        if (isGrounded)
        {
            if (-movementSpeedDecceleration < currentMovementSpeed && currentMovementSpeed < movementSpeedDecceleration)
            {
                currentMovementSpeed = 0;
            }
            else if (-movementSpeedDecceleration > currentMovementSpeed)
            {
                currentMovementSpeed += movementSpeedDecceleration;
            }
            else
            {
                currentMovementSpeed -= movementSpeedDecceleration;
            }
        }
    }

    public void CharacterJump()
    {
        if (isGrounded && isOnFoot)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

    private void CharacterColliderAdjustment()
    {
        Transform center;
        float height, radius;

        if (state == MovementState.crouch_idle || state == MovementState.crouch_moving)
        {
            center = crouchingCenterPoint;
            height = crouchingHeight;
            radius = crouchingRadius;
        }
        else if(state == MovementState.idle || state == MovementState.walking || state == MovementState.running)
        {
            center = standingCenterPoint;
            height = standingHeight;
            radius = standingRadius;
        }
        else if(state == MovementState.sliding)
        {
            center = slidingCenterPoint;
            height = slidingHeight;
            radius = slidingRadius;
        }
        else
        {
            center = slidingCenterPoint;
            height = slidingHeight;
            radius = slidingRadius;
        }
        playerCapsuleCollider.center = center.transform.position - playerMovement.transform.position;
        playerCapsuleCollider.height = height;
        playerCapsuleCollider.radius = radius;
    }



    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheckPoint.transform.position, playerRadius);
    }

    #region Data transfering to other scripts
    public float CurrentPlayerSpeed()
    {
        return currentMovementSpeed;
    }

    public bool IsCrouching()
    {
        return isCrouching;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsOnFoot()
    {
        return isOnFoot;
    }

    public bool IsLanding()
    {
        return isLanding;
    }

    public bool JumpPerformed()
    {
        bool performed = jumpPerformed;
        jumpPerformed = false;
        return performed;
    }

    public bool IsPlayerFastEnough()
    {
        if (currentMovementSpeed > 6.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector3 GetCameraRelatedMovementDir()
    {
        return cameraRelativeMoveDir;
    }
    #endregion
}
