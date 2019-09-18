using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    public float power = 10f;
    public float radius = 5.0f;
    public float upforce = 1.0f;
    private MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(0f, 0, speed * 100 * Time.deltaTime, ForceMode.Impulse);
        GetComponent<BoxCollider>().enabled = false;
        Invoke("Enable", 0.05f);
    }

    private void Enable()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        rb.velocity = new Vector3(0, 0, 0);
        Debug.Log("boom boom");
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach (Collider hit in colliders) //for each collider hit with the explosions collider, run this
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upforce, ForceMode.Impulse);
            }
        }
        Destroy(gameObject, 1);
    }

}
