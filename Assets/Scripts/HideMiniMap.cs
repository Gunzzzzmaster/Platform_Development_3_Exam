using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMiniMap : MonoBehaviour {

    void Update()
    {
        // a quick way to disable the minimap when the game is paused
        transform.GetComponent<Camera>().enabled = (Time.timeScale != 0f);
    }
}
