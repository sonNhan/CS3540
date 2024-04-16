using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Flying enemies don't need to follow the road, they'll fly 
// directly towards the end!
public class EnemyFlyingMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 10f;

    Transform goal;
    GameController gameController;
    bool isAlive = true;
    void Start()
    {
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("EnemyWaypoint");
        goal = waypoints[waypoints.Length - 1].transform;
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
        else if (Vector3.Distance(transform.position, goal.position) >= 0f)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, goal.position, step);
            transform.LookAt(goal);
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

}