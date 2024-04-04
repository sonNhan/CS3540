using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    [SerializeField]
    float projectileSpeed = 100f;
    [SerializeField]
    float projectileLifespan = 5f;
    [SerializeField]
    bool homing = false;

    GameObject target;
    bool targetSet; // if a target for this projectile has been set ever.
    bool shot = false; // required in case an enemy hitbox brushes on the projectile model in the turret
    int projectileDamage;
    Vector3 targetDirection;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (homing)
            {
                targetDirection = (target.transform.position - transform.position).normalized;
            }
            transform.position += targetDirection * projectileSpeed * Time.deltaTime;
        }
        else if (target == null && targetSet)
        {   // if our target disappears for whatever reason, just let the projectile
            // fly forward until it expires
            targetSet = false;
            transform.position += transform.forward * projectileSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && shot)
        {
            // TODO: deal damage to enemy (probably through another script)
            other.GetComponent<EnemyHealth>().TakeDamage(projectileDamage);
            Destroy(gameObject);
        }

    }

    public void Shoot(GameObject enemy, int damage)
    {
        shot = true;
        target = enemy;
        projectileDamage = damage;
        targetSet = true;
        targetDirection = (target.transform.position - transform.position).normalized;
        // Once shot, projectile has a lifespan
        Destroy(gameObject, projectileLifespan);
    }
}
