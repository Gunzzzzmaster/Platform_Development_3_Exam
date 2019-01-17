using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGameOver : MonoBehaviour {

    // FIELDS
    public GameObject GameManagerObject;

    // METHODS
    void Start()
    {
        transform.GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {
        if (GameManagerObject.GetComponent<GameManager>().IsGameOver()) // render screen when game over
        {
            GameManagerObject.GetComponent<GameManager>().HardPauseGame(true);
            transform.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            transform.GetComponent<Canvas>().enabled = false;
        }
    }
}
