using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementCursorBehavior : MonoBehaviour
{

    [SerializeField]
    float cursorSensitivity = 10f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MoveCursor();
        RotateCursor();
    }

    void MoveCursor()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // The cursor (which is a quad), is rotated 90 degrees on the X axis, so the Z axis is pointing down.
        transform.Translate(mouseX * cursorSensitivity, mouseY * cursorSensitivity, 0.0f);
    }

    void RotateCursor()
    {
        // Get the rotation of the camera
        Quaternion cameraRotation = Camera.main.transform.rotation;

        // Set the rotation of the cursor to match the rotation of the camera
        transform.rotation = Quaternion.Euler(90f, cameraRotation.eulerAngles.y, cameraRotation.eulerAngles.z);
    }
}
