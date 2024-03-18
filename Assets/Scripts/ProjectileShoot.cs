using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    [SerializeField]
    float projectileSpeed = 100f;
    [SerializeField]
    float projectileLifespan = 5f;

    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            transform.position += targetDirection * projectileSpeed * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // TODO: deal damage to enemy (probably through another script)
            Destroy(gameObject);
        }

    }

    public void Shoot(GameObject enemy)
    {
        target = enemy;
        // Once shot, projectile has a lifespan
        Destroy(gameObject, projectileLifespan);
    }
}
