using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyForcedMovement : MonoBehaviour
{
    public float speed = 10f;
    GameObject[] waypoints;
    int wayponitIndex;
    private GameController gameController;
    private bool isAlive = true;
    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("EnemyWaypoint");
        wayponitIndex = 0;
        sortWaypoints();
        gameController = GameObject.Find("LevelManager").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<EnemyHealth>().GetCurrentHealth() <= 0)
        {
            Destroy(gameObject, 1);
            gameController.RemoveEnemy(gameObject);
        }
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
            if (isAlive)
            {
                Destroy(gameObject, 1);
                gameController.RemoveEnemy(gameObject);
                isAlive = false;
                gameController.LoseLife(1);
                // Debug.Log($"Lives: {gameController.GetLives()}");
            }
        }
    }

    void sortWaypoints()
    {
        waypoints = waypoints.OrderBy(x => int.Parse(x.name.Split('(', ')')[1])).ToArray();
    }
}
