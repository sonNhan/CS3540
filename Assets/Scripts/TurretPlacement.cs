using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlacement : MonoBehaviour
{

    public bool Placeable
    {
        get { return placeable; }
    }
    public bool NotColliding
    {
        get { return notColliding; }
        set { notColliding = value; }
    }

    bool placeable = false;
    bool validTerrain = false;
    bool notColliding = true;
    bool placed = false;
    Color placeableColor;
    Color unplaceableColor;
    PlaceableTerrainScript tile;

    // Start is called before the first frame update
    void Start()
    {
        placeableColor = new Color(0f, 0f, 1f, 0.3f);
        unplaceableColor = new Color(1f, 1f, 1f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        placeable = validTerrain && notColliding;
        UpdateColor();
    }

    void UpdateColor()
    {
        Transform rangeIndicators = transform.Find("RangeIndicators");
        Transform placementIndicator = rangeIndicators.transform.Find("PlacementRange");
        if (placeable)
        {
            placementIndicator.GetComponent<Renderer>().material.color = placeableColor;
        }
        else
        {
            placementIndicator.GetComponent<Renderer>().material.color = unplaceableColor;
        }
    }

    void OnDestroy()
    {
        tile.isPlaceable = true;
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
            Transform ground = hit.transform;
            if (ground.CompareTag("Placeable"))
            {
                validTerrain = true;
            }
            else if (ground.CompareTag("Unplaceable"))
            {
                validTerrain = false;
            }
        }
    }

    public bool IsPlaced()
    {
        return placed;
    }

    public void SetPlaced(bool flag)
    {
        placed = flag;
    }

    public void SetTile(PlaceableTerrainScript terrainScript)
    {
        tile = terrainScript;
    }

}
