using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableButton : MonoBehaviour
{
    // METHODS
    void OnTriggerEnter(Collider col)
    {
        // Enable pushing the button with the interact key when the player is in front of the button
        if (col.gameObject.name == "Ball")
        {
            col.gameObject.GetComponent<PlayerController>().EnablePushing(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        // Disable pushing when out of range
        if (col.gameObject.name == "Ball")
        {
            col.gameObject.GetComponent<PlayerController>().EnablePushing(false);
        }
    }
}
