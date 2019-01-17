using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

    // FIELDS
    public GameObject Character;
    public GameObject GameManager;
    public GameObject Portal;
    public float Speed = 60.0f; // accelleration (a) in m/s^2
    public float RotationVelocity = 4.0f; // camera rotation speed in m/s
    public float StumbleDuration = 0.6f; // how long the player's movement is interrupted (in seconds)
    public float MinimumStumbleForce = 50.0f; // force in Newton

    private Animator _animator;
    private float _yOffset = 1.074f;
    private float _zOffset = 0.055f;
    private float _animationMaxSpeed = 10.0f;
    private float _currentTime = 0.0f; // in seconds
    private bool _interactIsFly = false;
    private bool _isFlying = false;
    private bool _isStumbling = false;
    private bool _interactIsPush = false;
    private bool _startPushing = false;
    private bool _isPushing = false;
    // fields needed for LookAtTarget() script
    public GameObject TargetButton;
    private Vector3 _targetRotation;
    private Vector3 _lastKnownPosition = Vector3.zero;
    private Quaternion _lookAtRotation;
    private float _cinematicCameraRotationSpeed = 360.0f; // in degrees per second

    // METHODS
    void Start()
    {
        Vector3 position = transform.position;
        position.y -= _yOffset;
        position.z -= _zOffset;
        Character.transform.position = position;

        _animator = Character.transform.GetComponent<Animator>();
#if DEBUG
        Assert.IsNotNull(_animator, "DEPENDENCY ERROR: PlayerController needs an AnimatorComponent");
#endif
    }

    private void FixedUpdate()
    {
        // Handle time
        if (_currentTime >= 0.0f)
        {
            _currentTime -= Time.deltaTime;
        }
        if (_isStumbling &&_currentTime <= 0.0f)
        {
            _isStumbling = false;
        }

        Vector3 direction = Character.transform.forward;

        float verticalMovementInput = Input.GetAxis("Vertical");
        float horizontalMovementInput = Input.GetAxis("Horizontal");

        // Freeze input on pause
        if (GameManager.GetComponent<GameManager>().IsGamePaused())
        {
            verticalMovementInput = 0.0f;
            horizontalMovementInput = 0.0f;
        }

        if (!_isStumbling && !_isPushing) // only enable movement when player isn't stumbling or pushing a button
        {
            if (Input.GetButtonDown("Interact") && _interactIsFly && !_isFlying) // enable flying when button is pressed & allowed to fly
            {
                _isFlying = true;
                transform.GetComponent<Rigidbody>().useGravity = false;
            }

            if (!_interactIsFly) // disable flying when no longer in fly area
            {
                _isFlying = false;
                transform.GetComponent<Rigidbody>().useGravity = true;
            }

            // enable pushing button state when key is pressed & not flying (or stumbling)
            if (Input.GetButtonDown("Interact") && _interactIsPush && !_isFlying)
            {
                _startPushing = true;
            }

            // MOVEMENT of the ball (regular)
            if (!_isFlying)
            {
                float mass = transform.GetComponent<Rigidbody>().mass;
                transform.GetComponent<Rigidbody>().AddForce(direction * Speed * mass * verticalMovementInput); // F=m*a <=> F=mass*Speed
                // Debug.Log(Speed * mass * verticalMovementInput);
                // The highest possible force is 300N after testing (= max speed)
            }
            else // special flying movement
            {
                transform.GetComponent<Rigidbody>().AddForce(direction * Speed * verticalMovementInput);
                transform.GetComponent<Rigidbody>().AddForce(Character.transform.up * Speed * Input.GetAxis("Interact"));
            }
        }

        // Turn character left/right when not in cinematic view
        if (!_isPushing)
        {
            Vector3 rotation = new Vector3(0, horizontalMovementInput * RotationVelocity, 0);
            Character.transform.Rotate(rotation);
        }
        else // rotate camera to target
        {
            LookAtTarget(TargetButton);
        }

        // Set animator speed
        float speed = Mathf.Clamp01(transform.GetComponent<Rigidbody>().velocity.magnitude / _animationMaxSpeed);
        _animator.SetFloat("Speed", speed);
        _animator.SetBool("EnableFlyingAnimation", _isFlying);
        _animator.SetBool("EnableStumbleAnimation", _isStumbling);
        _animator.SetBool("EnablePushAnimation", _startPushing);

        if (_startPushing)
        {
            _isPushing = true;
            _startPushing = false;
            Portal.GetComponent<EndReached>().EnablePortal(!Portal.GetComponent<EndReached>().IsEnabled()); // button press turns end portal on/off
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Push_Button_Start") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Push_Button_Stop"))
        {
            _isPushing = true;
        }
        else
        {
            _isPushing = false;
        }

    }

    void Update()
    {
        // Keep character in sphere
        Vector3 position = transform.position;
        position.y -= _yOffset;
        position.z -= _zOffset;
        Character.transform.position = position;
    }

    void OnCollisionEnter(Collision col) // play a sound when the collision is big enough, with a volume relative to the magnitude
    {
        float v = col.relativeVelocity.magnitude;
        if (v > 10.0f)
        {
            Mathf.Clamp(v, 10.0f, 100.0f);
            transform.GetComponent<AudioSource>().volume = v / 100.0f;
            transform.GetComponent<AudioSource>().Play();
            //Debug.Log(col.relativeVelocity.magnitude);

            // In case of a big collision, make the player stumble
            if (v >= MinimumStumbleForce)
                MakePlayerStumble(StumbleDuration);
        }
    }

    public void EnableFlying(bool isEnabled)
    {
        _interactIsFly = isEnabled;
        GameManager.GetComponent<GameManager>().ShowFlyUI(isEnabled);
    }

    public bool InteractIsFly()
    {
        return _interactIsFly;
    }

    public bool IsFlying()
    {
        return _isFlying;
    }

    public void MakePlayerStumble() // default stumble function
    {
        _isStumbling = true;
        _currentTime = StumbleDuration;
    }

    public void MakePlayerStumble(float duration) // overloaded stumble function
    {
        _isStumbling = true;
        _currentTime = duration;
    }

    public float GetPlayerStumbleDuration()
    {
        return StumbleDuration;
    }

    public void EnablePushing(bool isEnabled)
    {
        _interactIsPush = isEnabled;
        GameManager.GetComponent<GameManager>().ShowButtonUI(isEnabled);
    }

    public bool IsPushing()
    {
        return _isPushing;
    }

    private void LookAtTarget(GameObject target) // make character look at the target, only changing the y-rotation
    {
        Vector3 targetPosition = target.transform.position;
        if (_lastKnownPosition != targetPosition)
        {
            _lastKnownPosition = targetPosition;
            _lookAtRotation = Quaternion.LookRotation(_lastKnownPosition - Character.transform.position);
            _lookAtRotation = Quaternion.LookRotation(_lastKnownPosition - Character.transform.position);
        }

        if (Character.transform.rotation != _lookAtRotation)
        {
            Quaternion newRotation = Quaternion.RotateTowards(Character.transform.rotation, _lookAtRotation, _cinematicCameraRotationSpeed * Time.deltaTime);
            // freeze the x and z rotations
            _targetRotation.y = newRotation.eulerAngles.y;

            Character.transform.eulerAngles = _targetRotation;
        }
    }
}
