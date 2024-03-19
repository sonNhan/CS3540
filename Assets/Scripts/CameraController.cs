using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float cameraMoveSpeed = 50f;
    [SerializeField]
    float mouseSensitivity = 50f;

    Vector3 startingPosition;
    Quaternion startingRotation;
    float pitch;
    float yaw;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        pitch = transform.rotation.eulerAngles.x;
        yaw = transform.localRotation.eulerAngles.y;

        // Hide and capture hardware cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Input handling for camera move and rotation
        CameraMove();
        CameraRotate();
    }

    // Handles movement of the camera on all axises.
    void CameraMove()
    {
        float xTranslate = Input.GetAxis("Horizontal") * cameraMoveSpeed;
        float zTranslate = Input.GetAxis("Vertical") * cameraMoveSpeed;
        float yTranslate = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            yTranslate = cameraMoveSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            yTranslate = -cameraMoveSpeed;
        }

        Vector3 translation = new Vector3(xTranslate * Time.deltaTime, yTranslate * Time.deltaTime, zTranslate * Time.deltaTime);

        transform.Translate(translation);
    }

    // Handles rotation of the camera utilizing the mouse
    void CameraRotate()
    {
        // Only allow camera rotation when right clicking
        if (Input.GetMouseButton(1))
        {
            float xRotate = Input.GetAxis("Mouse X") * mouseSensitivity;
            float yRotate = Input.GetAxis("Mouse Y") * mouseSensitivity;

            yaw += xRotate;

            pitch -= yRotate;
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}
