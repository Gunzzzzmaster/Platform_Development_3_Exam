using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    // FIELDS
    public GameObject Target;
    public GameObject LocalBulletSpawn;
    public GameObject Bullet;

    public float CannonRange = 40.0f; // in meters
    public float RotateSpeed = 100.0f; // in degrees per second
    public float ShotPower = 80.0f; // force in Newton
    public float ReloadTime = 1.5f; // makes cannon fire every x seconds

    private float _currentTime = 0.0f;
    private bool _readyToFire = true;

    private Vector3 _targetRotation;
    private Vector3 _lastKnownPosition = Vector3.zero;
    private Quaternion _lookAtRotation;

    // METHODS
    void Start()
    {
        _targetRotation = transform.eulerAngles;
    }

    void FixedUpdate ()
    {
        if (_currentTime > 0.0f)
        {
            _currentTime -= Time.deltaTime; // handle timer
        }
        else if (!_readyToFire)
        {
            _readyToFire = true;
        }

        if (TargetInRange(Target))
        {
            LookAtTarget(Target);
            if (_readyToFire)
            {
                FireCannon(Bullet.GetComponent<Rigidbody>(), LocalBulletSpawn.transform, ShotPower);
                transform.GetComponent<AudioSource>().Play(); // Play fire sound
                _readyToFire = false; // disable ability to fire
                _currentTime = ReloadTime; // start countdown
            }
        }
	}

    private bool TargetInRange(GameObject target) // check if target is in range
    {
        return (Vector3.Distance(target.transform.position, transform.position) <= CannonRange);
    }

    private void LookAtTarget(GameObject target) // look at the target, only changing the y-rotation
    {
        Vector3 targetPosition = target.transform.position;
        if (_lastKnownPosition != targetPosition)
        {
            _lastKnownPosition = targetPosition;
            _lookAtRotation = Quaternion.LookRotation(_lastKnownPosition - transform.position);
            _lookAtRotation = Quaternion.LookRotation(_lastKnownPosition - transform.position);
        }

        if (transform.rotation != _lookAtRotation)
        {
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, _lookAtRotation, RotateSpeed * Time.deltaTime);
            // freeze the x and z rotations
            _targetRotation.y = newRotation.eulerAngles.y;

            transform.eulerAngles = _targetRotation;
        }
    }
    private void FireCannon(Rigidbody bullet, Transform spawnLocation, float shotPower)
    {
        // create new bullet object at target location
        Rigidbody bulletClone = Instantiate(bullet, spawnLocation.position, spawnLocation.rotation);
        // fire the newly created bullet
        bulletClone.AddForce(transform.forward * shotPower * bulletClone.mass, ForceMode.Impulse); // F=m*a
    }
}
