using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterMovement : MonoBehaviour
{

    [SerializeField] HunterMovement hunter;
    [SerializeField] Transform sightPoint;
    [SerializeField] Player player;

    [SerializeField] LayerMask playerLayer;


    private bool isFoundPlayer;

    private float movementSpeed,
        walkingSpeed = 3f,
        runningSpeed = 5f;
    
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
        if (Physics.Raycast(sightPoint.transform.position, player.transform.position - sightPoint.transform.position, maxSightDistance))
        {
            isFoundPlayer = true;
            Chase();
        }
        else
        {
            isFoundPlayer = false;
            Patrol();
        }
    }


    private void Patrol()
    {

    }

    private void Chase()
    {
        Vector3 lookAtPlayer = player.transform.position - sightPoint.transform.position;
        lookAtPlayer.Normalize();

        if (lookAtPlayer.magnitude == 0) return;

        var hunterTargetAngle = Mathf.Atan2(lookAtPlayer.x, lookAtPlayer.z) * Mathf.Rad2Deg;
        var hunterAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, hunterTargetAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, hunterAngle, 0.0f);

        Vector3 hunterVelocityVector = lookAtPlayer * movementSpeed;

        HunterRb.velocity = new Vector3(hunterVelocityVector.x, HunterRb.velocity.y, hunterVelocityVector.z);
    }

}
