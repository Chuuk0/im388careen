using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int p1Score;
    //public int p2Score;

    public Text scoreText1;
    //public Text scoreText2;
    

    // Start is called before the first frame update
    void Start()
    {
        p1Score = 0;
        //p2Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore()
    {
        scoreText1.text = "Player 1: " + p1Score;
        //scoreText2.text = "Player 2: " + p2Score;
    }

}
