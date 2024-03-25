using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Player player;


    //Detect points
    public List<Vector3> detectPoints;

    [SerializeField] private Transform head;
    [SerializeField] private Transform rShoulder;
    [SerializeField] private Transform lSoulder;
    [SerializeField] private Transform legs;

    



    private void Start()
    {
        DetectPointsInitialization(head, rShoulder, lSoulder, legs);
    }

    private void FixedUpdate()
    {
        DetectPointsTracking(head, rShoulder, lSoulder, legs);
    }


    private void DetectPointsInitialization(Transform point_1, Transform point_2, Transform point_3, Transform point_4)
    {
        detectPoints.Add(point_1.transform.position);
        detectPoints.Add(point_2.transform.position);
        detectPoints.Add(point_3.transform.position);
        detectPoints.Add(point_4.transform.position);
    }

    private void DetectPointsTracking(Transform point_1, Transform point_2, Transform point_3, Transform point_4)
    {
        detectPoints[0] = point_1.transform.position;
        detectPoints[1] = point_2.transform.position;
        detectPoints[2] = point_3.transform.position;
        detectPoints[3] = point_4.transform.position;
    }
}
