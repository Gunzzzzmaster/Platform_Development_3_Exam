using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLifetime : MonoBehaviour {

    // FIELDS
    public float AirTime = 5.0f; // how many seconds each bullet stays in the scene before getting destroyed

    // METHODS
    void Awake()
    {
        Destroy(gameObject, AirTime);
    }
}
