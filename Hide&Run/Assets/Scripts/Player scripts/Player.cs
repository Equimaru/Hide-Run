using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private Transform head;
    [SerializeField] private Transform rShoulder;
    [SerializeField] private Transform lSoulder;
    [SerializeField] private Transform legs;

    public List<Vector3> detectPoints;

    private void Start()
    {
        detectPoints.Add(head.transform.position);
        detectPoints.Add(rShoulder.transform.position);
        detectPoints.Add(lSoulder.transform.position);
        detectPoints.Add(legs.transform.position);
    }

    private void FixedUpdate()
    {
        detectPoints[0] = head.transform.position;
        detectPoints[1] = rShoulder.transform.position;
        detectPoints[2] = lSoulder.transform.position;
        detectPoints[3] = legs.transform.position;
    }

}
