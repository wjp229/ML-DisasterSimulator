using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    private float mouseSensivility = 100f;
    private float rotY;
    private float rotX;
    private float rotXLimit = 80f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 rot = this.transform.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * Time.deltaTime * mouseSensivility;
        rotX += mouseY * Time.deltaTime * mouseSensivility;

        rotX = Mathf.Clamp(rotX, -rotXLimit, rotXLimit);

        Quaternion mouseRot = Quaternion.Euler(rotX, rotY, 0);
        transform.localRotation = mouseRot;
    }
}
