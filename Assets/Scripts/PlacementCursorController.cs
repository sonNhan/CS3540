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
    GameObject currentTurret;
    GameObject placementPointer;
    GameObject turretParent;
    bool selectedTurret = false;
    bool confirmedSelection = false;

    // Start is called before the first frame update
    void Start()
    {
        terrain = GameObject.Find("DirtGround");
        placementPointer = GameObject.Find("PlacementPointer");
        turretParent = GameObject.Find("Turrets");
    }

    // Update is called once per frame
    void Update()
    {
        MovePointer();
        if (selectedTurret)
        {
            MoveTurret();
        }
        RotatePointer();
        PlaceTurret();
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

    }

    void PlaceTurret()
    {
        // Choose turret
        if (!selectedTurret && Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentTurret = Instantiate(turret1, placementPointer.transform.position, Quaternion.identity);
            placementPointer.GetComponent<Renderer>().enabled = false;
            selectedTurret = true;
        }
        // TODO: handle other keys for other turrets in the future

        // Confirm selection
        if (selectedTurret && Input.GetMouseButton(0))
        {
            // Disable rendering of turret's range indicator
            Transform rangeIndicators = currentTurret.transform.Find("RangeIndicators");
            Transform placementIndicator = rangeIndicators.transform.Find("PlacementRange");
            Transform attackIndicator = rangeIndicators.transform.Find("AttackRange");
            placementIndicator.GetComponent<Renderer>().enabled = false;
            attackIndicator.GetComponent<Renderer>().enabled = false;

            currentTurret.transform.parent = turretParent.transform;
            confirmedSelection = true;
            selectedTurret = false;
            placementPointer.GetComponent<Renderer>().enabled = true;
        }

        // Cancel selection
    }
}