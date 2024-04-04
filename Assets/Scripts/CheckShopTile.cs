using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckShopTile : MonoBehaviour
{
    GameObject terrain;
    TerrainController terrainController;
    GameObject placementPointer;
    bool onShopTile;
    GameObject shopTile;
    Animator animator;
    enum FSMstates {
        WALKING,
        IDLE
    }
    FSMstates currentState;

    void Start()
    {
        init();
    }

    private void init()
    {
        terrain = GameObject.Find("DirtGround");
        terrainController = terrain.GetComponent<TerrainController>();
        placementPointer = GameObject.Find("PlacementPointer");
        shopTile = GameObject.FindGameObjectWithTag("ShopTile");
        animator = shopTile.GetComponentInChildren<Animator>();
        currentState = FSMstates.WALKING;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case FSMstates.WALKING:
                WalkState();
                break;
            case FSMstates.IDLE:
                IdleState();
                break;
        }
        IsOnShopTile();
        CheckClickOnShopTile();
    }

    void WalkState()
    {
        animator.SetBool("ShouldIdle", false);
        IsOnShopTile();
    }

    void IdleState()
    {
        animator.SetBool("ShouldIdle", true);
        IsOffShopTile();
        CheckClickOnShopTile();
    }

    void IsOnShopTile()
    {
        // check if when we raycast we hit the shop tile
        RaycastHit hit;
        if (Physics.Raycast(placementPointer.transform.position, Vector3.down, out hit))
        {
            Transform ground = hit.transform;
            //Debug.Log(ground.tag);
            if (ground.CompareTag("ShopTile"))
            {
                onShopTile = true;
                currentState = FSMstates.IDLE;
            }
        }
    }
    
    void IsOffShopTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(placementPointer.transform.position, Vector3.down, out hit))
        {
            Transform ground = hit.transform;
            if (!ground.CompareTag("ShopTile")) {
                onShopTile = false;
                currentState = FSMstates.WALKING;
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
