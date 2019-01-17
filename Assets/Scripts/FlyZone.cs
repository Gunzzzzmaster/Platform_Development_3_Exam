using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyZone : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        // if the player enters the flying area, enable flying
        if (col.gameObject.name == "Ball")
        {
            col.gameObject.GetComponent<PlayerController>().EnableFlying(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        // if the player exits the flying area, disable flying
        if (col.gameObject.name == "Ball")
        {
            col.gameObject.GetComponent<PlayerController>().EnableFlying(false);
        }
    }
}
