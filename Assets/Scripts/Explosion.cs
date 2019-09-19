using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float power = 10f;
    public float radius = 5.0f;
    public float upforce = 1.0f;
    public bool explode = false;
    public float fuseTime = 5f;
    public GameObject particle;
    private bool deployParticles = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (explode == true)
        {
            Invoke("Detonate", fuseTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            explode = true;
        }
    }

    void Detonate()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach (Collider hit in colliders) //for each collider hit with the explosions collider, run this
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upforce, ForceMode.Impulse);
            }
        }
        if(deployParticles == true)
        {
            deployParticles = false;
            Instantiate(particle, transform.position, transform.rotation = new Quaternion(0,0,0,0));
        }
        Destroy(gameObject, 1);
    }

}
