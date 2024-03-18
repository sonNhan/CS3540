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
    GameObject projectilePrefab;
    [SerializeField]
    AudioClip shootSFX;
    [SerializeField]
    Collider attackRangeCollider;
    [SerializeField]
    GameObject enemyGoal;
    [SerializeField]
    float attackSpeed = 2f, turretRotationSpeed = 10f;

    List<GameObject> enemiesInRange;
    GameObject target, currentProjectile;
    TargetPriority targetPriority = TargetPriority.FIRST;
    float timeSinceAttack = 0.0f;
    ProjectileShoot projectileShootScript;
    // Where the projectile should be, how it should be oriented and scaled upon being
    // instantiated on the turret
    float projectileXPos, projectileYPos, projectileZPos;
    float projectileXRot, projectileYRot, projectileZRot;
    float projectileXScale, projectileYScale, projectileZScale;

    // Start is called before the first frame update
    void Start()
    {
        enemiesInRange = new List<GameObject>();
        currentProjectile = InstantiateProjectile();
    }

    // Update is called once per frame
    void Update()
    {
        LookForTarget(targetPriority);
        if (target != null && enemiesInRange.Contains(target))
        {
            Shoot(target);
        }
        timeSinceAttack += Time.deltaTime;
    }

    GameObject InstantiateProjectile()
    {
        Vector3 projectilePosition = transform.position + transform.rotation * projectilePrefab.transform.position;
        Quaternion projectileRotation = Quaternion.Euler(transform.rotation.x + projectilePrefab.transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + projectilePrefab.transform.rotation.eulerAngles.y,
                transform.rotation.eulerAngles.z + projectilePrefab.transform.rotation.eulerAngles.z);
        GameObject projectile = Instantiate(projectilePrefab, projectilePosition, projectileRotation);
        projectile.transform.parent = transform;
        return projectile;
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

    void Shoot(GameObject target)
    {
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
        if (timeSinceAttack >= attackSpeed)
        {
            timeSinceAttack = 0.0f;
            AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position);
            projectileShootScript = currentProjectile.GetComponent<ProjectileShoot>();
            projectileShootScript.Shoot(target);
            GameObject newProjectile = InstantiateProjectile();
            currentProjectile = newProjectile;
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
