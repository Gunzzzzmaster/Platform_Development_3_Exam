using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour {

    // FIELDS
    public GameObject GameManagerObject;
    public int Penalty = 100;

    // METHODS
    void OnCollisionEnter(Collision col)
    {
        // reset ball position when out of bounds (update: done elsewhere now), play sound, lose points and lose 1 life
        if (col.gameObject.name == "Ball")
        {
            transform.GetComponent<AudioSource>().Play();
            GameManagerObject.GetComponent<GameManager>().LoseLife();
            GameManagerObject.GetComponent<GameManager>().LosePoints(Penalty);
        }

        // reset enemy ball position when out of bounds
        if (col.gameObject.name == "EnemyBall")
        {
            col.gameObject.GetComponent<EnemyBehaviour>().RespawnEnemy();
        }

        // "destroy" (hide) boxes when out of bounds
        if (col.gameObject.name == "Box")
        {
            col.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
