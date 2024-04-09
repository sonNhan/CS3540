using static Utils;
using static Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{

    [Header("Initialization")]
    [SerializeField]
    GameObject projectilePrefab, enemyGoal;
    [SerializeField]
    AudioClip shootSFX;
    [SerializeField]
    Vector3 projectileInstantiatePosition;
    [Header("Stats")]
    [SerializeField]
    int damage = 20;
    [SerializeField]
    float attackRate = 2f, turretRotationSpeed = 10f;
    [Header("Prices")]
    [SerializeField]
    float upgradeCostIncreaseModifier = 1.1f;
    [SerializeField]
    int rangeUpgradeCost = 10, damageUpgradeCost = 10,
        speedUpgradeCost = 10, sellValue = 10;

    Transform goalTransform;
    List<GameObject> enemiesInRange;
    GameObject target, currentProjectile;
    TargetPriority targetPriority = TargetPriority.FIRST;
    float timeSinceAttack = 0.0f;
    ProjectileShoot projectileShootScript;
    GameController gameController;
    GameObject attackRangeIndicator;

    // Start is called before the first frame update
    void Start()
    {
        goalTransform = enemyGoal.transform;
        enemiesInRange = new List<GameObject>();
        currentProjectile = InstantiateProjectile();
        attackRangeIndicator = transform.Find("RangeIndicators").transform.Find("AttackRange").gameObject;
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        LookForTarget(targetPriority);
        if (target != null && enemiesInRange.Contains(target))
        {
            if (timeSinceAttack >= attackRate)
            {
                if (currentProjectile == null)
                {
                    Reload();
                }
                Shoot(target);
            }
        }
        timeSinceAttack += Time.deltaTime;
    }

    GameObject InstantiateProjectile()
    {
        Vector3 projectilePosition = transform.position + transform.rotation * projectileInstantiatePosition;
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
            else if (enemy != null && Vector3.Distance(first.transform.position, goalTransform.position)
                    > Vector3.Distance(enemy.transform.position, goalTransform.position))
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
            else if (enemy != null && Vector3.Distance(last.transform.position, goalTransform.position)
                    < Vector3.Distance(enemy.transform.position, goalTransform.position))
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
            else if (enemy != null && Vector3.Distance(closest.transform.position, transform.position)
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
        timeSinceAttack = 0.0f;
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position);
        projectileShootScript = currentProjectile.GetComponent<ProjectileShoot>();
        projectileShootScript.Shoot(target, damage);
    }

    void Reload()
    {
        GameObject newProjectile = InstantiateProjectile();
        currentProjectile = newProjectile;
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

    public TargetPriority GetTargetPriority()
    {
        return targetPriority;
    }

    public void ChangeTargetPriority(bool right)
    {
        if (right)
        {
            targetPriority = targetPriority.GetNext();
        }
        else
        {
            targetPriority = targetPriority.GetPrev();
        }
    }

    public bool CanAffordUpgrade(UpgradeType upgradeType)
    {
        int money = gameController.GetMoney();
        switch (upgradeType)
        {
            case UpgradeType.RANGE:
                return money >= rangeUpgradeCost;
            case UpgradeType.DAMAGE:
                return money >= damageUpgradeCost;
            case UpgradeType.SPEED:
                return money >= speedUpgradeCost;
            default:
                return false;
        }
    }

    // TODO: maybe make this more modular, i.e. make upgrading damage, range, etc different for 
    // every tower
    public void Upgrade(UpgradeType upgradeType)
    {
        if (CanAffordUpgrade(upgradeType))
        {
            switch (upgradeType)
            {
                case UpgradeType.RANGE:
                    gameController.AddMoney(-rangeUpgradeCost);
                    attackRangeIndicator.transform.localScale *= 1.2f;
                    sellValue += Mathf.RoundToInt(rangeUpgradeCost / 2);
                    rangeUpgradeCost = Mathf.RoundToInt(rangeUpgradeCost * upgradeCostIncreaseModifier);
                    break;
                case UpgradeType.DAMAGE:
                    gameController.AddMoney(-damageUpgradeCost);
                    damage += 5;
                    sellValue += Mathf.RoundToInt(damageUpgradeCost / 2);
                    damageUpgradeCost = Mathf.RoundToInt(damageUpgradeCost * upgradeCostIncreaseModifier);
                    break;
                case UpgradeType.SPEED:
                    gameController.AddMoney(-speedUpgradeCost);
                    attackRate /= 1.2f;
                    sellValue += Mathf.RoundToInt(speedUpgradeCost / 2);
                    speedUpgradeCost = Mathf.RoundToInt(speedUpgradeCost * upgradeCostIncreaseModifier);
                    break;
                default:
                    break;
            }
        }
    }

    public void SellTurret()
    {
        gameController.AddMoney(sellValue);
        Destroy(gameObject);
    }

    public int GetRangeUpgradeCost()
    {
        return rangeUpgradeCost;
    }

    public int GetDamageUpgradeCost()
    {
        return damageUpgradeCost;
    }

    public int GetSpeedUpgradeCost()
    {
        return speedUpgradeCost;
    }

    public int GetSellValue()
    {
        return sellValue;
    }
}
