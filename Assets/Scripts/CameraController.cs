using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float rotateSpeed = 50f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        transform.position = target.transform.position;
    }
    void LateUpdate()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotateSpeed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        //Debug.Log(transform.eulerAngles.x);
        if (transform.eulerAngles.x < 338 && transform.eulerAngles.x > 300)
        { transform.eulerAngles = new Vector3(338, transform.eulerAngles.y, 0); }
        if (transform.eulerAngles.x > 45 && transform.eulerAngles.x < 100)
        { transform.eulerAngles = new Vector3(45, transform.eulerAngles.y, 0); }


    }
}