﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float rotateSpeed = 50f;
    private GameObject Camera;
    public Vector2 minDistance = new Vector2(3, -5);
    public Vector2 maxDistance;
    public float cameraSpeed = 1;
    public float cameraDistance;
    private Camera cameraGO;
    private Vector3 cameraPos = new Vector3(0, 0, 0);
    private bool canScroll = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Camera = GameObject.Find("Camera");
        cameraGO = Camera.GetComponent<Camera>();

    }

    private void Update()
    {
        transform.position = target.transform.position;

        if (canScroll == true)
        {
            CameraScroll();
        }

        cameraDistance = (Camera.transform.localPosition.y - minDistance.x) / (maxDistance.x - minDistance.x);


        if (Input.GetButtonDown("Zoom"))
        {
            cameraPos = Camera.transform.localPosition;
            Camera.transform.localPosition = new Vector3(0, minDistance.x, minDistance.y);
            cameraGO.fieldOfView = 30;
            canScroll = false;
        }
        if (Input.GetButtonUp("Zoom"))
        {
            Camera.transform.localPosition = cameraPos;
            cameraGO.fieldOfView = 75;
            canScroll = true;
        }
    }

    void CameraScroll()
    {
        new Vector3(0, minDistance.x, minDistance.y);
        if (Input.mouseScrollDelta.y < 0)
        {
            Camera.transform.localPosition = Vector3.MoveTowards(Camera.transform.localPosition, new Vector3(0, minDistance.x, minDistance.y), cameraSpeed * Time.deltaTime);
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            Camera.transform.localPosition = Vector3.MoveTowards(Camera.transform.localPosition, new Vector3(0, maxDistance.x, maxDistance.y), cameraSpeed * Time.deltaTime);
        }
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