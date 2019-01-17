using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLevelComplete : MonoBehaviour {

    // FIELDS
    public GameObject GameManagerObject;

    // METHODS
    void Start()
    {
        transform.GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {
        if (GameManagerObject.GetComponent<GameManager>().IsLevelComplete()) // render screen when level complete
        {
            string score = GameManagerObject.GetComponent<GameManager>().GetScore().ToString();
            string highscore = GameManagerObject.GetComponent<GameManager>().GetHighscore().ToString();
            string scoreInformation = "score: " + score + "\nhighscore: " + highscore;
            
            GameObject scoreText = GameObject.Find("ScoreText");
            scoreText.GetComponent<Text>().text = scoreInformation; // update score display

            GameManagerObject.GetComponent<GameManager>().HardPauseGame(true);
            transform.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            transform.GetComponent<Canvas>().enabled = false;
        }
    }
}
