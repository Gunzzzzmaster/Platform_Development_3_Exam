using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArms : MonoBehaviour {

    // FIELDS
    public float RotationSpeed = 1.2f; // in m/s

    // METHODS
    void FixedUpdate()
    {
        // apply a seemingly constant rotational force to the arms (ignoring mass)
        transform.GetComponent<Rigidbody>().maxAngularVelocity = RotationSpeed;
        transform.GetComponent<Rigidbody>().AddTorque(transform.up * RotationSpeed, ForceMode.VelocityChange);
    }
}
