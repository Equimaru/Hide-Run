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

    
    private Rigidbody rb;

    #region PlayerAnimator related variables
    public float maxMovementSpeed = 4f;
    public float movementSpeedAcceleration = 0.25f;
    public bool jumpPerformed;
    #endregion

    #region Movement variables
    private float movementSpeedDecceleration = 0.2f,
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

    private float playerRadius = 0.2f;
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

    public void OldMovementHandler(PlayerStateManager player)
    {

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
        
        if (velocity.magnitude < maxMovementSpeed)
        {
            currentMovementSpeed += movementSpeedAcceleration;
        }
        else if (velocity.magnitude > maxMovementSpeed)
        {
            currentMovementSpeed -= movementSpeedAcceleration;
        }
        
    }


    //Does it need rework with state machine?
    private void CharacterBraking()
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

    public void CharacterJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void CharacterRotation()
    {
        Vector3 cameraRelativeMoveDir = CamRelativeMovementDir();

        if (cameraRelativeMoveDir.magnitude == 0) return;

        var targetAngle = Mathf.Atan2(cameraRelativeMoveDir.x, cameraRelativeMoveDir.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    



    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(playerMovement.transform.position, playerRadius);
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
