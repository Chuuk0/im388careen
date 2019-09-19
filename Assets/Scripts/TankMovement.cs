using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    private Rigidbody rb;

    public float speed = 90f;
    public float torque = 15f;
    private float m_zMove;
    private float m_rotate;
    public float turretSpeed = 60f;
    public float barrelSpeed = 60f;
    private float turretCompensation = 5;
    private bool canShoot = true;

    //Gameobjects
    private GameObject turret;
    private GameObject barrel;
    private GameObject groundDetector;
    public GameObject cameraOBJ;
    public GameObject bullet;
    private GameObject cof;

    private CameraController cameraScript;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_zMove = Input.GetAxis("zMove");
        m_rotate = Input.GetAxis("Rotate");
        rb = GetComponent<Rigidbody>();
        turret = GameObject.Find("Turret");
        barrel = GameObject.Find("Turret/Barrel");
        cof = GameObject.Find("CenterOfMass");
        rb.centerOfMass = cof.transform.localPosition;

        cameraScript = cameraOBJ.GetComponent<CameraController>();
    }

    public void FixedUpdate()
    {
        m_zMove = Input.GetAxis("zMove");
        m_rotate = Input.GetAxis("Rotate");

        turretMove();
        Turn();
        Movement();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire") && canShoot == true)
        {
            GameObject instance = Instantiate(bullet, new Vector3(turret.transform.position.x, barrel.transform.position.y, turret.transform.position.z), barrel.transform.rotation);
            canShoot = false;
            StartCoroutine("rateOfFire");
        }
    }


    void turretMove()
    {
        turretCompensation = Mathf.Lerp(2, 6, cameraScript.cameraDistance);

        Vector3 turretCurrentAngle = turret.transform.localEulerAngles;
        Vector3 turretTargetAngle = cameraOBJ.transform.localEulerAngles;
        turretTargetAngle = new Vector3(cameraOBJ.transform.localEulerAngles.x, cameraOBJ.transform.localEulerAngles.y - transform.eulerAngles.y, cameraOBJ.transform.localEulerAngles.z); //subtracts tanks rotation from the rotation of the turret so that it does not follow its y rotation.
        turretCurrentAngle = new Vector3(0, Mathf.LerpAngle(turretCurrentAngle.y, turretTargetAngle.y, Time.deltaTime * turretSpeed), 0);
        turret.transform.localEulerAngles = new Vector3(0, turretCurrentAngle.y, 0);


        Vector3 barrelCurrentAngle = barrel.transform.localEulerAngles;
        Vector3 barrelTargetAngle = cameraOBJ.transform.localEulerAngles;
        barrelCurrentAngle = new Vector3(Mathf.LerpAngle(barrelCurrentAngle.x, barrelTargetAngle.x - turretCompensation, Time.deltaTime * barrelSpeed), 0, 0);

        if (barrelCurrentAngle.x > 16 && barrelCurrentAngle.x < 46) //locks barrel from angling down too far
        {
            barrelCurrentAngle.x = 16;
        }
        barrel.transform.localEulerAngles = new Vector3(barrelCurrentAngle.x, 0, 0);
    }

    private void Turn()
    {
        float turn = m_rotate * torque * Time.deltaTime; // Determine the number of degrees to be turned based on the input, speed and time between frames.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f); // Make this into a rotation in the y axis.
        rb.MoveRotation(rb.rotation * turnRotation); // Apply this rotation to the rigidbody's rotation.
    }

    private void Movement()
    {

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = m_zMove * speed;
                axleInfo.rightWheel.motorTorque = m_zMove * speed;
            }
        }
    }

    IEnumerator rateOfFire()
    {
        yield return new WaitForSeconds(0.5f);
        canShoot = true;

    }

}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
}