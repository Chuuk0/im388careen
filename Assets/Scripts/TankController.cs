using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public float speed = 90f;
    public float torque = 15f;
    private Rigidbody rb;
    private float m_zMove;
    private float m_rotate;
    public float turretSpeed = 60f;
    public float barrelSpeed = 60f;
    public float turretCompensation = 5;
    private bool grounded = true;
    private bool canMove = true;

    public PhysicMaterial physicsMat;
    private MeshCollider mCollider;

    //Objects
    private GameObject turret;
    private GameObject barrel;
    private GameObject groundDetector;
    public GameObject cameraOBJ;
    public GameObject bullet;


    // Start is called before the first frame update
    void Start()
    {
        m_zMove = Input.GetAxis("zMove");
        m_rotate = Input.GetAxis("Rotate");
        rb = GetComponent<Rigidbody>();
        groundDetector = GameObject.Find("GroundDetector");
        turret = GameObject.Find("Turret");
        barrel = GameObject.Find("Turret/Barrel");
        mCollider = GetComponent<MeshCollider>();

        mCollider.material.dynamicFriction = 0.4f;
        mCollider.material.staticFriction = 0.2f;
    }

    private void Awake()
    {

    }

    void Update()
    {

        if (Input.GetKeyDown("space"))
        {
            mCollider.material.dynamicFriction = 1f;
            mCollider.material.staticFriction = 1f;
            canMove = false;
        }
        if (Input.GetKeyUp("space"))
        {
            mCollider.material.dynamicFriction = 0.4f;
            mCollider.material.staticFriction = 0.2f;
            canMove = true;
        }

        if (Input.GetButtonDown("Fire"))
        {
            GameObject instance = Instantiate(bullet, new Vector3(barrel.transform.position.x,barrel.transform.position.y, barrel.transform.position.z), barrel.transform.rotation);
        }
        m_zMove = Input.GetAxis("zMove");
        m_rotate = Input.GetAxis("Rotate");
        turretMove();
    }

    private void FixedUpdate()
    {
        if (grounded == true && canMove == true)
        {
            Move();
            Turn();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            grounded = true;
            //Debug.Log("grounded");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            grounded = false;
            //Debug.Log("airborn");
        }
    }


    private void Turn()
    {
        float turn = m_rotate * torque * Time.deltaTime; // Determine the number of degrees to be turned based on the input, speed and time between frames.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f); // Make this into a rotation in the y axis.
        rb.MoveRotation(rb.rotation * turnRotation); // Apply this rotation to the rigidbody's rotation.
    }

    private void Move()
    {
        //Vector3 movement = transform.forward * m_zMove * speed * Time.deltaTime;  //alternative moving method
        //rb.MovePosition(rb.position + movement);

        rb.AddRelativeForce(0f, 0, m_zMove * speed * 100 * Time.deltaTime, ForceMode.Force);
    }

    void turretMove()
    {
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

    
}
