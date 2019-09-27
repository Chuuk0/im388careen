using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jump : MonoBehaviour
{
    public Rigidbody rb;
    private bool JumpTimer = true;
    public float JumpUp = 4500;
    public float JumpForward = 3000;
    public float JumpWait = 3.2F;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && JumpTimer == true)
        {
            print("jump");
            JumpTimer = false;
            //rb.AddExplosionForce(800, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f), 10, 300, ForceMode.Force);
            rb.AddForce(JumpForward, JumpUp, 0, ForceMode.Impulse);
            StartCoroutine(Jumping());
        }

        if (Input.GetKey(KeyCode.R))
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
            Time.timeScale = 1;
        }
    }

    IEnumerator Jumping()
    {
        yield return new WaitForSeconds(JumpWait);
        JumpTimer = true;
    }
}
