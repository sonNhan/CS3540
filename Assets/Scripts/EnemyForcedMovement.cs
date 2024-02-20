using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyForcedMovement : MonoBehaviour
{
    public float speed = 10f;
    GameObject[] waypoints;
    int wayponitIndex;
    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("EnemyWaypoint");
        wayponitIndex = 0;
        sortWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        if (wayponitIndex < waypoints.Length)
        {
            Vector3 targetPosition = waypoints[wayponitIndex].transform.position;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            transform.LookAt(targetPosition);
            
            if (transform.position == targetPosition)
            {
                wayponitIndex++;
            }
        }
        else 
        {
            Destroy(gameObject, 1);
        }
    }

    void sortWaypoints() {
        waypoints = waypoints.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray();
        //Debug.Log("Waypoints sorted.");
        for (int i = 0; i < waypoints.Length; i++)
        {
            //Debug.Log("Waypoint " + i + ": " + waypoints[i].transform.position);
        }

    }
}
