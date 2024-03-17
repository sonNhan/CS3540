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
        if (target != null && enemiesInRange.Contains(target))
        {
            Debug.Log("Shooting at target...");
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
        GameObject first = target;
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy == null)
            {
                continue;
            }
            // The only enemy in range is the first one
            if (first == null)
            {
                first = enemy;
            }
            // The first enemy is the enemy closest to their goal
            else if (Vector3.Distance(first.transform.position, enemyGoal.transform.position)
                    > Vector3.Distance(enemy.transform.position, enemyGoal.transform.position))
            {
                first = enemy;
            }
        }
        return first;
    }

    GameObject GetLastEnemy()
    {
        GameObject last = target;
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy == null)
            {
                continue;
            }
            // The only enemy in range is the first one
            if (last == null)
            {
                last = enemy;
            }
            // The last enemy is the enemy furthest from their goal
            else if (Vector3.Distance(last.transform.position, enemyGoal.transform.position)
                    < Vector3.Distance(enemy.transform.position, enemyGoal.transform.position))
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
        GameObject closest = target;
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy == null)
            {
                continue;
            }
            // The only enemy in range is the first one
            if (closest == null)
            {
                closest = enemy;
            }
            else if (Vector3.Distance(closest.transform.position, transform.position)
                    > Vector3.Distance(enemy.transform.position, transform.position))
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
        transform.LookAt(target.transform, Vector3.up);
    }

    public void AddEnemyInRange(GameObject enemy)
    {
        if (enemy != null && !enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    public void RemoveEnemyInRange(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
    }
}
