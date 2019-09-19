using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem ps2;
    private GameObject childPsObj;

    // Start is called before the first frame update
    void Start()
    {
        childPsObj = GameObject.Find("ps1");
        ps = GetComponent<ParticleSystem>();
        ps2 = childPsObj.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ps != null && ps2 != null)
        {
            if (ps.isPlaying && ps2.isPlaying)
            {
                Destroy(gameObject, 5);
            }
            else
            {
                Destroy(gameObject, 5);
            }
        }

    }
}
