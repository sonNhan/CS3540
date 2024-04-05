using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTileFSM : MonoBehaviour
{
    [SerializeField]
    AudioClip ShopkeeperHelloSFX, ShopKeeperTalkingSFX;

    GameObject chatBox, terrain;
    TerrainController terrainController;
    GameObject placementPointer;
    bool onShopTile;
    Animator animator;
    Material shopTileMaterial;
    Material originalShopTileMaterial;

    enum FSMstates
    {
        WALKING,
        GREET,
        TALK
    }

    FSMstates currentState;

    void Start()
    {
        chatBox = GameObject.Find("UI").transform.Find("Shopkeeper").gameObject;
        chatBox.SetActive(false);
        terrain = GameObject.Find("DirtGround");
        terrainController = terrain.GetComponent<TerrainController>();
        animator = GetComponentInChildren<Animator>();
        currentState = FSMstates.WALKING;
        shopTileMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case FSMstates.WALKING:
                WalkState();
                break;
            case FSMstates.GREET:
                IdleState();
                break;
            case FSMstates.TALK:
                TalkState();
                break;
        }

    }

    void WalkState()
    {
        animator.SetBool("ShouldIdle", false);
        if (onShopTile)
        {
            currentState = FSMstates.GREET;
            AudioSource.PlayClipAtPoint(ShopkeeperHelloSFX, Camera.main.transform.position);
        }
    }

    void IdleState()
    {
        animator.SetBool("ShouldIdle", true);
        if (!onShopTile)
        {
            currentState = FSMstates.WALKING;
        }
        CheckClickOnShopTile();
    }

    void TalkState()
    {
        // on chat close, the shopkeeper goes back to whatever they were doing
        if (!chatBox.activeSelf)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            currentState = FSMstates.WALKING;
            return;
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        animator.SetBool("ShouldIdle", true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlacementPointer"))
        {
            onShopTile = true;
            shopTileMaterial.color = Color.red;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlacementPointer"))
        {
            onShopTile = false;
            shopTileMaterial.color = Color.cyan;
        }
    }

    bool IsOnShopTile()
    {
        // check if when we raycast we hit the shop tile
        RaycastHit hit;
        if (Physics.Raycast(placementPointer.transform.position, Vector3.down, out hit))
        {
            Transform ground = hit.transform;
            if (ground.CompareTag("ShopTile"))
            {
                onShopTile = true;
                return true;
            }
        }
        return false;
    }

    /*
    void IsOnShopTile()
    {
        // check if when we raycast we hit the shop tile
        RaycastHit hit;
        if (Physics.Raycast(placementPoin/home/john/Documents/School/Classes/CS3450/Chronicles of Northwind/Assets/Scriptster.transform.position, Vector3.down, out hit))
        {
            Transform ground = hit.transform;
            //Debug.Log(ground.tag);
            if (ground.CompareTag("ShopTile"))
            {
                onShopTile = true;
                currentState = FSMstates.GREET;
                AudioSource.PlayClipAtPoint(ShopkeeperHelloSFX, Camera.main.transform.position);
            }
        }
    }

    void IsOffShopTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(placementPointer.transform.position, Vector3.down, out hit))
        {
            Transform ground = hit.transform;
            if (!ground.CompareTag("ShopTile"))
            {
                onShopTile = false;
                currentState = FSMstates.WALKING;
            }
        }
    }
    */

    void CheckClickOnShopTile()
    {
        // check if left mouse button is clicked
        if (onShopTile && Input.GetMouseButtonDown(0))
        {
            //OpenShop();
            currentState = FSMstates.TALK;
            chatBox.SetActive(true);
            AudioSource.PlayClipAtPoint(ShopKeeperTalkingSFX, Camera.main.transform.position);
        }
    }

    void OpenShop()
    {
        Debug.Log("Opening the shop");
    }


}
