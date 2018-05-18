using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBehaivor : MonoBehaviour {

    public int score;
    public Text scoreText;
    public GameObject levelComplete;

    public GameController gameController;
    public int levelCompleteScore = 10;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        scoreText.text = "Points [" + score + "]";

        if(score>=levelCompleteScore){
            levelComplete.SetActive(true);
            gameController.StopSpawn();
        } 
    }

}
