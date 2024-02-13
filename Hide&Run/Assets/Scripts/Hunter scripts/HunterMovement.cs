using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterMovement : MonoBehaviour
{

    [SerializeField] HunterMovement hunter;
    [SerializeField] Transform sightPoint;
    [SerializeField] Player player;

    [SerializeField] LayerMask playerLayer;


    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        LookForPlayer();
    }


    private void LookForPlayer()
    {
        
        float maxSightDistance = 20f;
        if (Physics.Raycast(sightPoint.transform.position, player.transform.position - sightPoint.transform.position, maxSightDistance, playerLayer))
        {
            
        }
    }

    //private void CharacterRotation()
    //{
    //    Vector3 cameraRelativeMoveDir = CamRelativeMovementDir();

    //    if (cameraRelativeMoveDir.magnitude == 0) return;

    //    var targetAngle = Mathf.Atan2(cameraRelativeMoveDir.x, cameraRelativeMoveDir.z) * Mathf.Rad2Deg;
    //    var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
    //    transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    //}
}
