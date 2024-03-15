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
        if (other.gameObject.CompareTag("Enemy") && turretPlacementScript.IsPlaced())
        {
            Debug.Log("Adding Enemy..." + other.gameObject.name);
            turretShootScript.AddEnemyInRange(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && turretPlacementScript.IsPlaced())
        {
            turretShootScript.RemoveEnemyInRange(other.gameObject);
        }
    }
}
