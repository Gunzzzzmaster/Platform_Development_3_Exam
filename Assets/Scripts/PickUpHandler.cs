using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHandler : MonoBehaviour {

    // FIELDS
    public GameObject GameManagerObject;
    public int Value = 40; // points you get for each pickup

    // METHODS
    void OnTriggerEnter(Collider col)
    {
        // "remove" (hide) pick-up upon contact with the player, play sound and add points
        if (col.gameObject.name == "Ball")
        {
            transform.GetComponent<AudioSource>().Play();
            transform.GetComponent<Collider>().enabled = false;
            transform.GetComponent<Renderer>().enabled = false;
            GameManagerObject.GetComponent<GameManager>().AddPoints(Value);
            GameManagerObject.GetComponent<GameManager>().AddPickup();
        }
    }

    public void EnablePickUp()
    {
        transform.GetComponent<Collider>().enabled = true;
        transform.GetComponent<Renderer>().enabled = true;
    }
}
