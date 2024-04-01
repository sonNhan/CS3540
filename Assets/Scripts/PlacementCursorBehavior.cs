using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementCursorBehavior : MonoBehaviour
{

    [SerializeField]
    float cursorSensitivity = 10f;
    [SerializeField]
    GameObject turret1; // HACK: need a cleaner way to represent different selectable turrets

    GameObject terrain;
    GameObject currentTurret, highlightedTurret, hoveredTurret, placementPointer, turretParent;
    HighlightedTurretUI highlightedTurretUIScript;
    GameObject levelManager;
    GameController gameControllerScript;
    bool selectedTurret = false;
    float highlightDelay = 0.5f;
    float lastPlacedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager");
        gameControllerScript = levelManager.GetComponent<GameController>();
        terrain = GameObject.Find("DirtGround");
        placementPointer = GameObject.Find("PlacementPointer");
        turretParent = GameObject.Find("Turrets");
        highlightedTurretUIScript = GameObject.Find("UI").GetComponent<HighlightedTurretUI>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePointer();
        RotatePointer();
        PlaceTurret();
        if (selectedTurret)
        {
            MoveTurret();
        }
        HighlightTurret();
    }

    void MovePointer()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float minX = -terrain.transform.localScale.x / 2;
        float minZ = -terrain.transform.localScale.z / 2;
        float maxX = terrain.transform.localScale.x / 2;
        float maxZ = terrain.transform.localScale.z / 2;

        placementPointer.transform.Translate(mouseX * cursorSensitivity, mouseY * cursorSensitivity, 0f);

        float clampX = Mathf.Clamp(placementPointer.transform.position.x, minX, maxX);
        float clampZ = Mathf.Clamp(placementPointer.transform.position.z, minZ, maxZ);

        placementPointer.transform.position = new Vector3(clampX, placementPointer.transform.position.y, clampZ);
    }

    void RotatePointer()
    {
        // Get the rotation of the camera
        Quaternion cameraRotation = Camera.main.transform.rotation;

        // Set the rotation of the cursor to match the rotation of the camera
        placementPointer.transform.rotation = Quaternion.Euler(90f, cameraRotation.eulerAngles.y, cameraRotation.eulerAngles.z);
    }

    void MoveTurret()
    {
        currentTurret.transform.position = placementPointer.transform.position;

        // Check if we are placing on valid terrain, and represent that in the turret's placement indicator
        currentTurret.GetComponent<TurretPlacement>().OnValidTerrain();
    }

    void PlaceTurret()
    {
        // Choose turret
        if (!selectedTurret && Input.GetKeyDown(KeyCode.Alpha1) && gameControllerScript.GetMoney() >= 20)
        {
            if (highlightedTurret != null)
            {
                UnhighlightTurret(highlightedTurret);
            }
            currentTurret = Instantiate(turret1, placementPointer.transform.position, Quaternion.identity);
            placementPointer.GetComponent<Renderer>().enabled = false;
            selectedTurret = true;
            gameControllerScript.AddMoney(-20);
        }
        // TODO: handle other keys for other turrets in the future

        // Confirm selection
        if (selectedTurret && Input.GetMouseButton(0) && currentTurret.GetComponent<TurretPlacement>().Placeable)
        {
            lastPlacedTime = Time.time;

            // Disable rendering of turret's range indicator
            if (Physics.Raycast(currentTurret.transform.position, Vector3.down, out var hit))
            {
                var ground = hit.transform.gameObject;
                if (ground.CompareTag("Placeable"))
                {
                    var script = ground.GetComponent<PlaceableTerrainScript>();
                    script.isPlaceable = false;
                    script.turret = currentTurret;
                    currentTurret.GetComponent<TurretPlacement>().SetPlaced(true);
                    currentTurret.GetComponent<TurretPlacement>().SetTile(script);
                    var position = ground.transform.position;
                    currentTurret.transform.position = new Vector3(position.x, position.y + 0.5f, position.z);

                    Transform rangeIndicators = currentTurret.transform.Find("RangeIndicators");
                    Transform placementIndicator = rangeIndicators.transform.Find("PlacementRange");
                    Transform attackIndicator = rangeIndicators.transform.Find("AttackRange");
                    placementIndicator.GetComponent<Renderer>().enabled = false;
                    attackIndicator.GetComponent<Renderer>().enabled = false;

                    // Put the turret into the turret parent object
                    currentTurret.transform.parent = turretParent.transform;
                    selectedTurret = false;

                    // Render the placement cursor again
                    placementPointer.GetComponent<Renderer>().enabled = true;
                }
            }
        }

        // Cancel selection
        if (selectedTurret && Input.GetKey(KeyCode.X))
        {
            Destroy(currentTurret);
            selectedTurret = false;
            placementPointer.GetComponent<Renderer>().enabled = true;
        }
    }

    // Opens a menu for controlling a selected turret
    void HighlightTurret()
    {
        if (Time.time - lastPlacedTime >= highlightDelay)
        {
            if (!selectedTurret && Input.GetMouseButton(0) && hoveredTurret != null)
            {
                // unhighlight already highlighted turrets
                if (highlightedTurret != null)
                {
                    UnhighlightTurret(highlightedTurret);
                }
                Transform rangeIndicators = hoveredTurret.transform.Find("RangeIndicators");
                Transform placementIndicator = rangeIndicators.transform.Find("PlacementRange");
                Transform attackIndicator = rangeIndicators.transform.Find("AttackRange");
                placementIndicator.GetComponent<Renderer>().enabled = true;
                attackIndicator.GetComponent<Renderer>().enabled = true;
                // highlight the new turret
                highlightedTurret = hoveredTurret;
                Debug.Log(highlightedTurret);
                // enable the context menu for a highlighted turret
                highlightedTurretUIScript.SetUIActive(true, highlightedTurret);
            }
        }
        // Unhighlight a turret if we have a highlighted turret and we click on the ground with nothing
        if (highlightedTurret != null && Input.GetMouseButton(0) && hoveredTurret == null)
        {
            UnhighlightTurret(highlightedTurret);
        }
    }

    public void UnhighlightTurret(GameObject turret)
    {
        // disable the context menu for a highlighted turret
        highlightedTurretUIScript.SetUIActive(false, highlightedTurret);

        Transform rangeIndicators = turret.transform.Find("RangeIndicators");
        Transform placementIndicator = rangeIndicators.transform.Find("PlacementRange");
        Transform attackIndicator = rangeIndicators.transform.Find("AttackRange");
        placementIndicator.GetComponent<Renderer>().enabled = false;
        attackIndicator.GetComponent<Renderer>().enabled = false;
        highlightedTurret = null;
    }

    // Gets the turret that the pointer is currently hovering over
    public void SetHoveredTurret(GameObject turret)
    {
        hoveredTurret = turret;
    }

    public GameObject GetHoveredTurret()
    {
        return hoveredTurret;
    }

    public GameObject GetHighlightedTurret()
    {
        return highlightedTurret;
    }
}
