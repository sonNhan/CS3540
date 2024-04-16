using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyForcedMovement : MonoBehaviour, EnemyMovement
{
    public float speed = 10f;
    GameObject[] waypoints;
    int waypointIndex;
    private GameController gameController;
    private bool isAlive = true;


    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("EnemyWaypoint");
        waypointIndex = 0;
        gameController = GameObject.Find("LevelManager").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<EnemyHealth>().GetCurrentHealth() <= 0)
        {
            Destroy(gameObject, 1);
        }
        else if (waypointIndex < waypoints.Length)
        {
            Vector3 targetPosition = waypoints[waypointIndex].transform.position;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            transform.LookAt(targetPosition);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            if (isAlive)
            {
                Destroy(gameObject);
                isAlive = false;
                gameController.LoseLife(1);
                // Debug.Log($"Lives: {gameController.GetLives()}");
            }
        }
    }

    public float GetDistanceToGoal()
    {
        float totalDistance = 0f;
        foreach (var waypoint in waypoints)
        {
            totalDistance += Vector3.Distance(transform.position, waypoint.transform.position);
        }
        return totalDistance;
    }

}
