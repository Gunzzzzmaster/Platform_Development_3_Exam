using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour // Note: a slightly more fitting name would be TiltCamera, since the actual player rotation is handled in PlayerController
{
    // FIELDS
    public GameObject Character;
    public GameObject Ball;
    public float MaxAngle = 15.0f; // max angle of camera tilt, in degrees

    private Vector3 _initialAngles;
    private Vector3 _currentAngles;
    [SerializeField]
    private float _rotationSpeed = 90.0f; // horizontal tilt speed in degrees per second
    [SerializeField]
    private float _resetSpeed = 120.0f; // speed of camera returning back to its original rotation

    // METHODS
    void Start()
    {
        _initialAngles = transform.eulerAngles;
        _currentAngles = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (!Ball.GetComponent<PlayerController>().IsFlying() && !Ball.GetComponent<PlayerController>().IsPushing()) // disable camera rotation when flying or pushing a button
        {
            // rotate camera pivot
            float verticalMovementInput = Input.GetAxis("Vertical");
            float horizontalMovementInput = Input.GetAxis("Horizontal");

            _currentAngles.x -= _rotationSpeed * verticalMovementInput * Time.deltaTime; // Note: in the case of my game, camera rotation is mass independent
            _currentAngles.y = Character.transform.eulerAngles.y;
            _currentAngles.z += _rotationSpeed * horizontalMovementInput * Time.deltaTime;

            //if (!(Input.GetButton("Vertical")))
            if (Input.GetAxis("Vertical") == 0f)
                {
                if (Mathf.Abs(_currentAngles.x) < 2.0f)
                {
                    _currentAngles.x = 0.0f;
                }
                else
                {
                    int direction = 0;
                    if (_currentAngles.x > 0.0f)
                        direction = -1;
                    if (_currentAngles.x < 0.0f)
                        direction = 1;

                    _currentAngles.x += _resetSpeed * direction * Time.deltaTime;
                }
            }

            //if (!(Input.GetButton("Horizontal")))
            if (Input.GetAxis("Horizontal") == 0f)
            {
                if (Mathf.Abs(_currentAngles.z) < 2.0f)
                {
                    _currentAngles.z = 0.0f;
                }
                else
                {
                    int direction = 0;
                    if (_currentAngles.z > 0.0f)
                        direction = -1;
                    if (_currentAngles.z < 0.0f)
                        direction = 1;

                    _currentAngles.z += _resetSpeed * direction * Time.deltaTime;
                }
            }

            // limit camera rotation
            if (_currentAngles.x > MaxAngle)
            {
                _currentAngles.x = MaxAngle;
            }
            if (_currentAngles.x < -MaxAngle)
            {
                _currentAngles.x = -MaxAngle;
            }

            if (_currentAngles.z > MaxAngle)
            {
                _currentAngles.z = MaxAngle;
            }
            if (_currentAngles.z < -MaxAngle)
            {
                _currentAngles.z = -MaxAngle;
            }

            // apply rotation
            transform.eulerAngles = _initialAngles + _currentAngles;
        }
    }
}
