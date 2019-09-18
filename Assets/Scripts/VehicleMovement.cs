using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    private GameObject frontLeftWheel, frontRightWheel, backLeftWheel, backRightWheel, centerOfMass, forcePos, rotTarget;
    private Rigidbody rb;
    private float FLdis, FRdis, BLdis, BRdis = 0f;
    private float m_zMove, m_rotate;

    public float rayDis = 5f;
    public float speed = 60f;
    public float torque = 60f;
    public float suspensionStiffness = 1f;
    public bool autoBrake = true;
    public float autoBrakeSpeed = 0.5f;
    public float gripForce = 1;


    // Start is called before the first frame update
    void Start()
    {
        m_zMove = Input.GetAxis("zMove");
        m_rotate = Input.GetAxis("Rotate");
        frontLeftWheel = GameObject.Find("Front Left Wheel");
        frontRightWheel = GameObject.Find("Front Right Wheel");
        backLeftWheel = GameObject.Find("Back Left Wheel");
        backRightWheel = GameObject.Find("Back Right Wheel");
        centerOfMass = GameObject.Find("Center Of Mass");
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        m_zMove = Input.GetAxis("zMove");
        m_rotate = Input.GetAxis("Rotate");

        FLdis = wheelSuspension(frontLeftWheel);
        FRdis = wheelSuspension(frontRightWheel);
        BLdis = wheelSuspension(backLeftWheel);
        BRdis = wheelSuspension(backRightWheel);

        Rotate();

        if (Input.GetKeyDown("space"))
        {
            rb.AddExplosionForce(80, new Vector3(transform.position.x, transform.position.y, transform.position.z-0.5f), 10, 300, ForceMode.Impulse);
        }

       //Debug.Log(FLdis + ", " + FRdis + ", " + BLdis +", " + BRdis);
    }

    private void FixedUpdate()
    {
        if(FLdis == 0 && FRdis == 0 && BLdis == 0 && BRdis == 0)
        {
            //Debug.Log("airborn");
        }
        else
        {
            Movement();
        }

        
        /*
        if (m_zMove == 0 && m_rotate == 0 && (FLdis != 0 && FRdis != 0 && BLdis != 0 && BRdis != 0) && autoBrake == true)
        {
            Vector3 currentVelocity = rb.velocity;
            rb.velocity = Vector3.Lerp(currentVelocity, new Vector3(0, 0, 0), autoBrakeSpeed* Time.deltaTime);
        }
        */
    }

    public float wheelSuspension(GameObject wheelOBJ)
    {
        float compressionRatio;
        RaycastHit Wheel;
        if (Physics.Raycast(wheelOBJ.transform.position, transform.TransformDirection(-transform.up), out Wheel, rayDis))
        {
            compressionRatio = (((((Wheel.distance * 10) * 2) / 10) * -1) + 1);
        }
        else { compressionRatio = 0; }
        Vector3 currentVelocity = rb.GetPointVelocity(wheelOBJ.transform.position);
        Vector3 currentForce = Vector3.ProjectOnPlane(currentVelocity, transform.up);
        Vector3 newForce = transform.up * compressionRatio * suspensionStiffness;
        Vector3 deltaForce = newForce - currentForce;

        float wheelOffset = rayDis;
        if (Wheel.distance != 0)
        {
            wheelOffset = Wheel.distance;
        }

        rb.AddForceAtPosition(newForce*Time.deltaTime*100, wheelOBJ.transform.position, ForceMode.Force);


        Debug.DrawRay(wheelOBJ.transform.position, -transform.up * wheelOffset, Color.red);

        Vector3 impactPoint = Wheel.point;
        Vector3 impactNormal = Vector3.Reflect(impactPoint, Wheel.normal);

        //rotTarget.transform.up = Vector3.Lerp(rotTarget.transform.up, Wheel.normal, Time.deltaTime * gripForce);
        //rotTarget.transform.Rotate(0, transform.eulerAngles.y, 0);


        return compressionRatio;
    }

    void Movement()
    {
        rb.AddRelativeForce(0f, 0, m_zMove * speed * 100 * Time.deltaTime, ForceMode.Force);

        Debug.Log("forward/backward Speed: " + Vector3.Dot(transform.forward, rb.velocity));
        Debug.Log("right/left Speed: " + Vector3.Dot(transform.right, rb.velocity));
        float lateralRightSpeed = Vector3.Dot(transform.right, rb.velocity);
        float lateralForwardSpeed = Vector3.Dot(transform.forward, rb.velocity);
        rb.AddForce(((-lateralRightSpeed * gripForce) * transform.right), ForceMode.VelocityChange);
        rb.AddForce(((-lateralForwardSpeed * gripForce) * transform.forward), ForceMode.VelocityChange);
    }

    void Rotate()
    {

        float turn = m_rotate * torque * Time.deltaTime; // Determine the number of degrees to be turned based on the input, speed and time between frames.
        Quaternion turnRotation = Quaternion.Euler(0, turn, 0); // Make this into a rotation in the y axis.
        rb.MoveRotation(rb.rotation * turnRotation); // Apply this rotation to the rigidbody's rotation.
    }
}
