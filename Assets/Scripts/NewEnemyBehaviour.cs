using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
public class NewEnemyBehaviour : MonoBehaviour
{
    // FIELDS
    public GameObject Character;
    public GameObject GameManager;
    public GameObject Target;
    public List<Transform> WaypointArray;
    public float RegularSpeed = 20.0f; // accelleration (a) in m/s^2
    public float ChargeSpeed = 80.0f; // accelleration (a) in m/s^2
    public float RotationVelocity = 360.0f; // character rotation speed in degrees per second
    public float AttackRange = 45.0f; // range in meters from which the enemy will charge at the player
    public Vector2 WaitTimeRange = new Vector2(1.0f, 6.0f); // minimum to maximum idle time in seconds

    public float PushBackForce = 600.0f; // force in Newton
    private float _radius = 4.0f;

    private Animator _animator;
    private float _yOffset = 1.35f;
    private float _animationMaxSpeed = 10.0f;

    private NavMeshAgent _agent;
    private Vector3 _targetRotation;
    private Vector3 _lastKnownPosition = Vector3.zero;
    private Quaternion _lookAtRotation;
    private float _currentTime = 0.0f;
    private float _waypointCorrection = 4.0f; // radius of the waypoints from where it will count as destination reached
    private bool _isMovingToWaypoint = false;
    private bool _waypointReached = false;
    //private bool _startBoxing = false;
    private bool _isBoxing = false;
    private float _boxingTime = 4.0f;
    private Vector3 _tempWaypoint;
    [SerializeField]
    private ActionState _state = ActionState.Waiting;
    private Vector3 _spawnPosition;
    private Quaternion _spawnRotation;

    // METHODS
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _spawnPosition = transform.position;
        _spawnRotation = transform.rotation;
        Vector3 position = transform.position;
        position.y -= _yOffset;
        Character.transform.position = position;

        _animator = Character.transform.GetComponent<Animator>();
#if DEBUG
        Assert.IsNotNull(_animator, "DEPENDENCY ERROR: NewEnemyBehaviour needs an AnimatorComponent");
#endif
    }

    private void FixedUpdate()
    {
        // Handle time
        if (_currentTime >= 0.0f)
        {
            _currentTime -= Time.deltaTime;
        }

        DetermineBehaviour(Target, WaitTimeRange);

        // these 2 fields are changed depending on different behaviour states
        Vector3 moveToPosition = transform.position; // initialize target position at 0,0,0
        float moveSpeed = RegularSpeed; // initialize default speed

        if (_state == ActionState.Waiting) // Waiting State
        {
            // do nothing
            moveSpeed = 0.0f;
        }
        else if (_state == ActionState.Attacking) // Attack State
        {
            moveToPosition = Target.transform.position;
            moveSpeed = ChargeSpeed;
        }
        else if (_state == ActionState.Moving)
        {
            if (!_isMovingToWaypoint) // set random new waypoint if enemy isn't moving to one yet
            {
                int r = Random.Range(0, WaypointArray.Count - 1);
                _tempWaypoint = WaypointArray[r].position;
                _isMovingToWaypoint = true;
            }
            moveToPosition = _tempWaypoint;
            moveSpeed = RegularSpeed;

            if (Vector3.Distance(_tempWaypoint, transform.position) <= _waypointCorrection) // check to see if the waypoint has been reached
            {
                _waypointReached = true;
                moveSpeed = 0.0f;
            }
        }

        // EXECUTE MOVEMENT
        // MOVEMENT of the enemy
        _agent.speed = moveSpeed * 2f / transform.GetComponent<Rigidbody>().mass;
        _agent.destination = moveToPosition;
        //Debug.Log(_agent.speed);

        // Turn character left/right
        if (_state != ActionState.Waiting)
            LookAtTarget(Character, moveToPosition);

        // Set animator speed
        float speed = Mathf.Clamp01(_agent.speed / _animationMaxSpeed);
        _animator.SetFloat("Speed", speed);
        _animator.SetBool("EnableBoxingAnimation", _isBoxing);
    }
    void Update()
    {
        // Keep character in sphere
        Vector3 position = transform.position;
        position.y -= _yOffset;
        Character.transform.position = position;
    }

    void OnCollisionEnter(Collision col)
    {
        // Apply some extra force to the player upon contact
        if (col.gameObject.name == "Ball")
        {
            Vector3 center = transform.position;
            center.y = col.transform.position.y;
            col.rigidbody.AddExplosionForce(PushBackForce * col.rigidbody.mass, center, _radius); // a=F/m, and we want the same amount of knockback even if the mass of the player changes, so we multiply by m
        }
    }

    private void DetermineBehaviour(GameObject target, Vector2 waitTimeRange) // checks and sets the correct behaviour state
    {
        // the enemy will always prioritize attacking when the player is in range (unless he's boxing)
        if (Vector3.Distance(target.transform.position, transform.position) <= AttackRange)
        {
            // if player was moving, interrupt that move, if it was waiting, reset wait timer
            _isBoxing = false;
            _isMovingToWaypoint = false;
            _waypointReached = false;
            _currentTime = 0.0f;

            _state = ActionState.Attacking;
            return;
        }
        else if (_state == ActionState.Attacking || _waypointReached) // if the enemy was attacking but the player went out of range, OR if the player was moving and the waypoint has been reached, start waiting
        {
            _waypointReached = false;
            _isMovingToWaypoint = false;

            _state = ActionState.Waiting;
            _currentTime = Random.Range(waitTimeRange.x, waitTimeRange.y);
            if (_currentTime >= _boxingTime) // makes the enemy do something when the wait time is long enough (air boxing in this case)
            {
                _isBoxing = true;
            }
            return;
        }
        else if (_state == ActionState.Waiting && _currentTime <= 0.0f) // if the enemy was waiting and the timer ran out, start moving
        {
            _isBoxing = false;
            _state = ActionState.Moving;
            return;
        }
    }

    private void LookAtTarget(GameObject rotateCharacter, Vector3 targetPosition) // look at the target, only changing the y-rotation
    {
        Vector3 targetPos = targetPosition;
        if (_lastKnownPosition != targetPos)
        {
            _lastKnownPosition = targetPos;
            _lookAtRotation = Quaternion.LookRotation(_lastKnownPosition - rotateCharacter.transform.position);
            _lookAtRotation = Quaternion.LookRotation(_lastKnownPosition - rotateCharacter.transform.position);
        }

        if (transform.rotation != _lookAtRotation)
        {
            Quaternion newRotation = Quaternion.RotateTowards(rotateCharacter.transform.rotation, _lookAtRotation, RotationVelocity * Time.deltaTime);
            // freeze the x and z rotations
            _targetRotation.y = newRotation.eulerAngles.y;

            rotateCharacter.transform.eulerAngles = _targetRotation;
        }
    }

    public void RespawnEnemy()
    {
        transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = _spawnPosition;
        transform.rotation = _spawnRotation;
        _state = ActionState.Waiting;
        _currentTime = Random.Range(WaitTimeRange.x, WaitTimeRange.y);
        _isMovingToWaypoint = false;
        _waypointReached = false;
        //Debug.Log("Enemy respawned");
    }
}
