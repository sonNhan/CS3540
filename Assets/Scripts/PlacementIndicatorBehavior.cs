using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementIndicatorBehavior : MonoBehaviour
{
    Transform turret;

    // Start is called before the first frame update
    void Start()
    {
        turret = transform.parent.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlacementRange"))
        {
            turret.GetComponent<TurretPlacement>().NotColliding = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlacementRange"))
        {
            turret.GetComponent<TurretPlacement>().NotColliding = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlacementRange"))
        {
            turret.GetComponent<TurretPlacement>().NotColliding = true;
        }
    }
}
