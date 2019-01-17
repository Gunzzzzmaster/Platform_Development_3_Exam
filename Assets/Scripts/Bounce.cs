using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour {

    // FIELDS
    public float BouncyForce = 2500f; // force in Newton
    public float Radius = 4f;
    public float StumbleDuration = 0.6f; // how long the player's movement is interrupted (in seconds)

    // METHODS
    void OnCollisionEnter(Collision col)
    {
        // Apply force to the ball upon contact and play sound
        if (col.gameObject.name == "Ball")
        {
            Vector3 center = transform.position;
            center.y = col.transform.position.y;
            col.rigidbody.AddExplosionForce(BouncyForce * col.rigidbody.mass, center, Radius); // a=F/m, and we want the same amount of knockback even if the mass of the player changes, so we multiply by m
            transform.GetComponent<AudioSource>().Play();
            // Make player stumble
            col.gameObject.GetComponent<PlayerController>().MakePlayerStumble(StumbleDuration);
        }

        // The enemies are also a viable target
        if (col.gameObject.name == "EnemyBall")
        {
            Vector3 center = transform.position;
            center.y = col.transform.position.y;
            col.rigidbody.AddExplosionForce(BouncyForce * col.rigidbody.mass, center, Radius); // a=F/m, and we want the same amount of knockback even if the mass of the player changes, so we multiply by m
            transform.GetComponent<AudioSource>().Play();
        }
    }
}
