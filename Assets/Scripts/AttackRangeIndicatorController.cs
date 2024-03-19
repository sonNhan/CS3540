using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeIndicatorController : MonoBehaviour
{
    GameObject turret;
    TurretShoot turretShootScript;
    TurretPlacement turretPlacementScript;

    // Start is called before the first frame update
    void Start()
    {
        turret = transform.parent.transform.parent.gameObject;
        turretShootScript = turret.GetComponent<TurretShoot>();
        turretPlacementScript = turret.GetComponent<TurretPlacement>();
    }

    void OnTriggerEnter(Collider other)
    {
        AddEnemy(other);
    }

    // Needed when a turret is placed down whilst units are already there
    void OnTriggerStay(Collider other)
    {
        AddEnemy(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && turretPlacementScript.IsPlaced())
        {
            turretShootScript.RemoveEnemyInRange(other.gameObject);
        }
    }

    void AddEnemy(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && turretPlacementScript.IsPlaced())
        {
            turretShootScript.AddEnemyInRange(other.gameObject);
        }
    }
}
