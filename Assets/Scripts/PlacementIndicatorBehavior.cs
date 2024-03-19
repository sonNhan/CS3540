using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementIndicatorBehavior : MonoBehaviour
{
    Transform turret;
    TurretPlacement turretPlacementScript;

    // Start is called before the first frame update
    void Start()
    {
        turret = transform.parent.transform.parent;
        turretPlacementScript = turret.GetComponent<TurretPlacement>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlacementRange"))
        {
            turretPlacementScript.NotColliding = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlacementRange"))
        {
            turretPlacementScript.NotColliding = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlacementRange"))
        {
            turretPlacementScript.NotColliding = true;
        }
    }
}
