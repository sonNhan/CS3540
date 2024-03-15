using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeIndicatorController : MonoBehaviour
{
    Transform turret;
    TurretShoot turretShootScript;

    // Start is called before the first frame update
    void Start()
    {
        turret = transform.parent;
        turretShootScript = turret.GetComponent<TurretShoot>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            turretShootScript.AddEnemyInRange(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            turretShootScript.RemoveEnemyInRange(other.gameObject);
        }
    }
}
