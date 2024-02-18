using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float cameraMoveSpeed = 50f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Moving the camera on the X/Z axis
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


        transform.Translate(xTranslate * Time.deltaTime, yTranslate * Time.deltaTime, zTranslate * Time.deltaTime);
    }
}
