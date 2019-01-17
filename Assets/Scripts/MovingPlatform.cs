using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    // FIELDS
    public GameObject PositionA;
    public GameObject PositionB;
    public float Speed = 10.0f; // in m/s
    public float WaitTime = 3.0f; // seconds
    public bool IsElevator = false;

    private GameObject _ball;
    private Vector3 _direction;
    private Transform _destination;
    private float _currentWaitTime = 0.0f;
    [SerializeField]
    private float _dragFactor = 0.274f; // manually picked to make sure the ball stays on the platform. This is mass-, friction- and gravity dependent
    [SerializeField]
    private bool _moveBall = false;

    // METHODS
    void Start()
    {
        SetDestination(PositionB.transform);
    }
    
    void FixedUpdate()
    {
        if (_currentWaitTime == 0)
        {
            // move platform when timer reaches 0
            transform.GetComponent<Rigidbody>().MovePosition(transform.position + _direction * Speed * Time.deltaTime);
            if (_moveBall)
            {
                // apply movement to the ball when platform moves and ball is on platform
                _ball.transform.GetComponent<Rigidbody>().MovePosition(_ball.transform.position + _direction * Speed * Time.deltaTime * _dragFactor);
            }
        }

        // when destination is reached
        if (Vector3.Distance(transform.position, _destination.position) < Speed * Time.deltaTime)
        {
            transform.position = _destination.position;
            SetDestination(_destination == PositionB.transform ? PositionA.transform : PositionB.transform); // set new destination
            StartWaitTime(WaitTime); // start timer
        }
        HandleTime();
    }

    private void SetDestination(Transform moveTo)
    {
        // sets a destination for our moving platform
        _destination = moveTo;
        _direction = (_destination.position - transform.position).normalized;
    }

    private void StartWaitTime(float time)
    {
        // starts timer
        _currentWaitTime = time;
    }

    private void HandleTime()
    {
        // timer
        if (_currentWaitTime > 0.0f)
            _currentWaitTime -= Time.deltaTime;

        if (_currentWaitTime < 0.0f)
            _currentWaitTime = 0.0f;
    }

    void OnCollisionEnter(Collision col)
    {
        // when the ball is on the platform:
        if (col.gameObject.name == "Ball" && !IsElevator)
        {
            _ball = col.gameObject;
            _moveBall = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        // when the ball is not on the platform anymore:
        if (col.gameObject.name == "Ball")
            _moveBall = false;
    }
}
