using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementPointerTrigger : MonoBehaviour
{
    PlacementCursorBehavior placementCursorBehaviorScript;
    void Start()
    {
        placementCursorBehaviorScript = transform.parent.GetComponent<PlacementCursorBehavior>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Hover over the turret's placement hitbox 
        if (other.CompareTag("PlacementRange"))
        {
            // PlacementRange indicator lives in RangeIndicator, which lives in the turret parent class
            GameObject turret = other.gameObject.transform.parent.transform.parent.gameObject;
            placementCursorBehaviorScript.SetHoveredTurret(turret);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlacementRange"))
        {
            GameObject turret = other.gameObject.transform.parent.transform.parent.gameObject;
            if (turret == placementCursorBehaviorScript.GetHoveredTurret())
            {
                placementCursorBehaviorScript.SetHoveredTurret(null);
            }
        }
    }
}

