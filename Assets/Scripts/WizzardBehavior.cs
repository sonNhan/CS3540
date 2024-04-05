using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizzardBehavior : MonoBehaviour
{
    public float distanceUntilNextTarget = 1f;
    GameObject[] wayPoints;
    GameObject targetPoint;
    int targetIndex;
    bool increasing;
    void Start()
    {
        wayPoints = GameObject.FindGameObjectsWithTag("WizzardWaypoint");
        targetIndex = 0;
        targetPoint = wayPoints[targetIndex];
        increasing = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistanceToTarget();
        LookAtTarget();
    }

    void LookAtTarget()
    {
        // get the direction
        Vector3 directionToTarget = (targetPoint.transform.position - transform.position).normalized;
        directionToTarget.y = 0;
        // apply rotation
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void CheckDistanceToTarget()
    {
        if (targetPoint == null)
        {
            Start();
        }
        // get distance
        float distanceToTarget = Vector3.Distance(transform.position, targetPoint.transform.position);
        // based on distance determine if we should go to the next point
        if (distanceToTarget <= distanceUntilNextTarget)
        {
            SelectNextPoint();
        }
    }

    void SelectNextPoint()
    {
        if (increasing)
        {
            targetIndex++;
            if (targetIndex >= wayPoints.Length - 1) 
            {
                increasing = false;
            }
        }
        else
        {
            targetIndex--;
            if (targetIndex <= 0)
            {
                increasing = true;
            }
        }
        targetPoint = wayPoints[targetIndex];
    }
}
