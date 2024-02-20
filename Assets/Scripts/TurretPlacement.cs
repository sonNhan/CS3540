using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlacement : MonoBehaviour
{

    public bool Placeable
    {
        get { return placeable; }
        set { placeable = value; }
    }

    bool placeable;
    Color placeableColor;
    Color unplaceableColor;

    // Start is called before the first frame update
    void Start()
    {
        placeableColor = new Color(0f, 0f, 1f, 0.3f);
        unplaceableColor = new Color(1f, 1f, 1f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnValidTerrain()
    {
        RaycastHit hit;
        // Use the placement indicator/collision box to raycast downwards and see if the terrain below is 
        // valid for placement
        Transform rangeIndicators = transform.Find("RangeIndicators");
        Transform placementIndicator = rangeIndicators.transform.Find("PlacementRange");
        if (Physics.Raycast(placementIndicator.position, Vector3.down, out hit))
        {
            // Check if the hit GameObject is placeable terrain
            Transform ground = hit.transform.parent;
            if (ground.CompareTag("Placeable"))
            {
                placementIndicator.GetComponent<Renderer>().material.color = placeableColor;
                placeable = true;
            }
            else if (ground.CompareTag("Unplaceable"))
            {
                placementIndicator.GetComponent<Renderer>().material.color = unplaceableColor;
                placeable = false;
            }
        }
    }

}
