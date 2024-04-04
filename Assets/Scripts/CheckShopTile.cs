using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckShopTile : MonoBehaviour
{
    GameObject terrain;
    TerrainController terrainController;
    GameObject placementPointer;
    bool onShopTile;
    void Start()
    {
        terrain = GameObject.Find("DirtGround");
        terrainController = terrain.GetComponent<TerrainController>();
        placementPointer = GameObject.Find("PlacementPointer");
    }

    // Update is called once per frame
    void Update()
    {
        IsOnShopTile();
        CheckClickOnShopTile();
    }

    void IsOnShopTile()
    {
        // check if when we raycast we hit the shop tile
        RaycastHit hit;
        if (Physics.Raycast(placementPointer.transform.position, Vector3.down, out hit))
        {
            Transform ground = hit.transform;
            if (ground.CompareTag("ShopTile"))
            {
                onShopTile = true;
            }
        }
    }

    void CheckClickOnShopTile()
    {
        // check if left mouse button is clicked
        if (onShopTile && Input.GetMouseButtonDown(0))
        {
            OpenShop();
        }
    }

    void OpenShop()
    {
        Debug.Log("Opening the shop");
    }
}
