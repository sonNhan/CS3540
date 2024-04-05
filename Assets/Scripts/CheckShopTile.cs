using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckShopTile : MonoBehaviour
{
    [SerializeField]
    AudioClip ShopkeeperHelloSFX, ShopKeeperTalkingSFX;

    GameObject terrain;
    TerrainController terrainController;
    GameObject placementPointer;
    bool onShopTile;
    GameObject shopTile;
    Animator animator;
    GameObject chatBox;

    enum FSMstates
    {
        WALKING,
        GREET,
        TALK
    }

    FSMstates currentState;

    void Start()
    {
        init();
    }

    void init()
    {
        chatBox = GameObject.Find("UI").transform.Find("Shopkeeper").gameObject;
        chatBox.SetActive(false);
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
        if (shopTile != null)
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
        else
        {
            init();
        }
    }

    void WalkState()
    {
        animator.SetBool("ShouldIdle", false);
        if (IsOnShopTile())
        {
            currentState = FSMstates.GREET;
            AudioSource.PlayClipAtPoint(ShopkeeperHelloSFX, Camera.main.transform.position);
        }
    }

    void IdleState()
    {
        animator.SetBool("ShouldIdle", true);
        if (!IsOnShopTile())
        {
            onShopTile = false;
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

    public void CloseConversation()
    {
        chatBox.SetActive(false);
    }
}
