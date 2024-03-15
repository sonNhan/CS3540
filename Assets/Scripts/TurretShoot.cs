using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    public enum TargetingStyle
    {
        FIRST,
        LAST,
        CLOSE
    }

    [SerializeField]
    GameObject projectile;
    [SerializeField]
    Collider attackRangeCollider;
    [SerializeField]
    GameObject enemyGoal;

    List<GameObject> enemiesInRange;
    GameObject target;
    TargetingStyle targetingStyle = TargetingStyle.FIRST;

    // Start is called before the first frame update
    void Start()
    {
        enemiesInRange = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        LookForTarget(targetingStyle);
        if (target != null)
        {
            ShootAtTarget(target);
        }
    }

    void LookForTarget(TargetingStyle targetingStyle)
    {
        foreach (GameObject enemy in enemiesInRange)
        {
            switch (targetingStyle)
            {
                case TargetingStyle.FIRST:
                    target = GetFirstEnemy();
                    break;
                case TargetingStyle.LAST:
                    target = GetLastEnemy();
                    break;
                case TargetingStyle.CLOSE:
                    target = GetClosestEnemy();
                    break;
                default:
                    Debug.Log("Invalid targeting style for turret!");
                    break;
            }
        }
    }

    GameObject GetFirstEnemy()
    {
        GameObject first = null;
        foreach (GameObject enemy in enemiesInRange)
        {
            // The only enemy in range is the first one
            if (first == null)
            {
                first = enemy;
            }
            // The first enemy is the enemy closest to their goal
            else if (Vector3.Distance(first.transform.position, enemyGoal.transform.position)
                    < Vector3.Distance(enemy.transform.position, enemyGoal.transform.position))
            {
                first = enemy;
            }
            else
            {
                continue;
            }
        }
        return first;
    }

    GameObject GetLastEnemy()
    {
        GameObject last = null;
        foreach (GameObject enemy in enemiesInRange)
        {
            // The only enemy in range is the first one
            if (last == null)
            {
                last = enemy;
            }
            // The last enemy is the enemy furthest from their goal
            else if (Vector3.Distance(last.transform.position, enemyGoal.transform.position)
                    > Vector3.Distance(enemy.transform.position, enemyGoal.transform.position))
            {
                last = enemy;
            }
            else
            {
                continue;
            }
        }
        return last;
    }

    GameObject GetClosestEnemy()
    {
        GameObject closest = null;
        foreach (GameObject enemy in enemiesInRange)
        {
            // The only enemy in range is the first one
            if (closest == null)
            {
                closest = enemy;
            }
            else if (Vector3.Distance(closest.transform.position, transform.position)
                    < Vector3.Distance(enemy.transform.position, transform.position))
            {
                closest = enemy;
            }
            else
            {
                continue;
            }
        }
        return closest;
    }

    void ShootAtTarget(GameObject target)
    {
        // TODO
    }

    public void AddEnemyInRange(GameObject enemy)
    {
        enemiesInRange.Add(enemy);
    }

    public void RemoveEnemyInRange(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
    }
}
