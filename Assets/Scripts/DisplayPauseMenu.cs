using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPauseMenu : MonoBehaviour {

    // FIELDS
    public GameObject GameManagerObject;

    // METHODS
    void Start()
    {
        transform.GetComponent<Canvas>().enabled = false;
    }

    void Update() // render screen when game is paused
    {
        if (GameManagerObject.GetComponent<GameManager>().IsGamePaused())
        {
            transform.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            transform.GetComponent<Canvas>().enabled = false;
        }
    }
}
