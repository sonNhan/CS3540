using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    public enum TargetPriority
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
    [SerializeField]
    float attackSpeed = 2f, turretRotationSpeed = 10f;

    List<GameObject> enemiesInRange;
    GameObject target;
    TargetPriority targetPriority = TargetPriority.FIRST;
    float timeSinceAttack = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemiesInRange = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        LookForTarget(targetPriority);
        if (target != null && enemiesInRange.Contains(target))
        {
            ShootAtTarget(target);
        }
        timeSinceAttack += Time.deltaTime;
    }

    void LookForTarget(TargetPriority targetPriority)
    {
        foreach (GameObject enemy in enemiesInRange)
        {
            switch (targetPriority)
            {
                case TargetPriority.FIRST:
                    target = GetFirstEnemy();
                    break;
                case TargetPriority.LAST:
                    target = GetLastEnemy();
                    break;
                case TargetPriority.CLOSE:
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
            // The only enemy in range is the first one
            if (first == null || !enemiesInRange.Contains(first))
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
            // The only enemy in range is the first one
            if (last == null || !enemiesInRange.Contains(last))
            {
                last = enemy;
            }
            // The last enemy is the enemy furthest from their goal
            else if (Vector3.Distance(last.transform.position, enemyGoal.transform.position)
                    < Vector3.Distance(enemy.transform.position, enemyGoal.transform.position))
            {
                last = enemy;
            }
        }
        return last;
    }

    GameObject GetClosestEnemy()
    {
        GameObject closest = target;
        foreach (GameObject enemy in enemiesInRange)
        {
            // The only enemy in range is the first one
            if (closest == null || !enemiesInRange.Contains(closest))
            {
                closest = enemy;
            }
            else if (Vector3.Distance(closest.transform.position, transform.position)
                    > Vector3.Distance(enemy.transform.position, transform.position))
            {
                closest = enemy;
            }
        }
        return closest;
    }

    void ShootAtTarget(GameObject target)
    {
        // Aim at the target
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
        // Shoot at the target
        if (timeSinceAttack >= attackSpeed)
        {
            timeSinceAttack = 0.0f;
        }
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

    public void ChangeTargetPriority(TargetPriority targetPriority)
    {
        this.targetPriority = targetPriority;
    }
}
