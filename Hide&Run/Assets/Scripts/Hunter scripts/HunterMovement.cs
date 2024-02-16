using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterMovement : MonoBehaviour
{

    [SerializeField] private HunterMovement hunter;
    [SerializeField] private Transform sightPoint;
    [SerializeField] private Player player;


    [SerializeField] private LayerMask playerLayer;


    private bool isFoundPlayer;

    private bool isRunning;

    private float movementSpeed,
        walkingSpeed = 4f,
        runningSpeed = 8f;
    
    private float currentVelocity;
    private float smoothTime = 0.1f;

    private Rigidbody HunterRb;

    private void Start()
    {
        HunterRb = GetComponent<Rigidbody>();
        HunterRb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        LookForPlayerPresense();
    }

    private void Update()
    {
        StateHandler();
    }

    public HunterState hunterState;

    public enum HunterState
    {
        patroling,
        chasing
    }

    private void StateHandler()
    {
        if (isFoundPlayer == true)
        {
            hunterState = HunterState.chasing;

            movementSpeed = runningSpeed;
        }
        else
        {
            hunterState = HunterState.patroling;

            movementSpeed = walkingSpeed;
        }
    }

    private void LookForPlayerPresense()
    {
        float maxDistance = 20f;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, maxDistance, playerLayer))
        {
            SearchForPlayer();
        }
    }


    private void SearchForPlayer()
    {
        float maxSightDistance = 20f;
        int count = 0;
        foreach (Vector3 detectPosition in player.detectPoints)
        {
            Debug.DrawRay(sightPoint.transform.position, detectPosition - sightPoint.transform.position, Color.red, 0.02f);
            if (Physics.Raycast(sightPoint.transform.position, detectPosition - sightPoint.transform.position,out RaycastHit hit, maxSightDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    count++;
                }
            }
        }

        if (count > 0)
        {
            isFoundPlayer = true;
            isRunning = true;
            Chase();
        }
        else
        {
            isFoundPlayer = false;
            isRunning = false;
            Patrol();
        }
    }


    private void Patrol()
    {
        Debug.Log("Patroling");
    }

    private void Chase()
    {
        Vector3 lookAtPlayer = player.transform.position - sightPoint.transform.position;
        lookAtPlayer.Normalize();

        HunterRotation(lookAtPlayer);

        Vector3 hunterVelocityVector = lookAtPlayer * movementSpeed;

        HunterRb.velocity = new Vector3(hunterVelocityVector.x, HunterRb.velocity.y, hunterVelocityVector.z);
    }

    private void HunterRotation(Vector3 rotationDirection)
    {
        if (rotationDirection.magnitude == 0) return;

        var targetAngle = Mathf.Atan2(rotationDirection.x, rotationDirection.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    public bool IsRunning()
    {
        return isRunning;
    }
}
