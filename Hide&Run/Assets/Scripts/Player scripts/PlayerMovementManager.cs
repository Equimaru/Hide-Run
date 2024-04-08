using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementManager : MonoBehaviour
{
    [SerializeField] private Transform freeLookCamera;
    private Vector3 cameraRelativeMoveDir;
    private float currentVelocity;
    private float smoothTime = 0.1f;

    public float maxMovementSpeed;
    public float currentMovementSpeed,
        idleSpeed = 0,
        crouchSpeed = 2f,
        walkSpeed = 4f,
        runSpeed = 7f;
    public float movementSpeedAcceleration = 0.25f;

    private float jumpForce = 6f;

    private GameInput gameInput;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        gameInput = GameObject.Find("Game Input").GetComponent<GameInput>();
    }


    public Vector3 CamRelativeMovementDir()
    {
        Vector2 inputVector = gameInput.GetInputVectorNormalized();

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        cameraRelativeMoveDir = cameraForward * moveDir.z + cameraRight * moveDir.x;
        cameraRelativeMoveDir.y = 0;
        return cameraRelativeMoveDir;
    }

    public void CharacterRotation()
    {
        Vector3 cameraRelativeMoveDir = CamRelativeMovementDir();

        if (cameraRelativeMoveDir.magnitude == 0) return;

        var targetAngle = Mathf.Atan2(cameraRelativeMoveDir.x, cameraRelativeMoveDir.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    public void MovementHandler(PlayerStateManager player)
    {
        if (player.state == player.idleState)
        {
            maxMovementSpeed = idleSpeed;
        }
        else if (player.state == player.walkState)
        {
            maxMovementSpeed = walkSpeed;
        }
        else if (player.state == player.runState)
        {
            maxMovementSpeed = runSpeed;
        }
        else if (player.state == player.crouchIdleState)
        {
            maxMovementSpeed = idleSpeed;
        }
        else if (player.state == player.crouchMoveState)
        {
            maxMovementSpeed = crouchSpeed;
        }
        else if (player.state == player.slideState)
        {
            maxMovementSpeed = runSpeed;
        }

        Vector3 currentVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        float currentMovementSpeed = currentVelocity.magnitude;

        Vector3 cameraRelativeMoveDir = CamRelativeMovementDir();

        if (currentVelocity.magnitude < maxMovementSpeed)
        {
            currentMovementSpeed += movementSpeedAcceleration;
        }
        else if (currentVelocity.magnitude > maxMovementSpeed)
        {
            currentMovementSpeed -= movementSpeedAcceleration;
        }

        Vector3 velocityVector = (cameraRelativeMoveDir.normalized + currentVelocity.normalized).normalized * currentMovementSpeed;
        rb.velocity = new Vector3(velocityVector.x, rb.velocity.y, velocityVector.z);
    }

    public void CharacterJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public Vector3 GetCameraRelatedMovementDir()
    {
        return cameraRelativeMoveDir;
    }
}
