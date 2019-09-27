using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject scoreManager;

    //private List<Rigidbody> rbs;
    GameObject[] obstacles;

    // Start is called before the first frame update
    void Start()
    {
        /*GameObject[] objs = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach(GameObject obj in objs)
        {
            rbs.Add(obj.GetComponent<Rigidbody>());
        }*/
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject obj in obstacles)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            scoreManager.GetComponent<ScoreManager>().p1Score += (int)rb.velocity.magnitude;
            scoreManager.GetComponent<ScoreManager>().UpdateScore();
        }

        /*foreach(Rigidbody rb in rbs)
        {
            scoreManager.GetComponent<ScoreManager>().p1Score += (int)rb.velocity.magnitude;
            scoreManager.GetComponent<ScoreManager>().UpdateScore();
        }*/
    }
}
