using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementIndicatorDetector : MonoBehaviour
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
        Debug.Log("Trigger enter");
        turret.GetComponent<TurretPlacement>().Placeable = false;
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger stay");
        turret.GetComponent<TurretPlacement>().Placeable = false;
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exit");
        turret.GetComponent<TurretPlacement>().Placeable = true;
    }

}
