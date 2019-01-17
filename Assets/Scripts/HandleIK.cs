using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HandleIK : MonoBehaviour
{
    // FIELDS
    public GameObject Ball;
    public GameObject TargetButton;

    private Animator _animator;
    // the following values are determined by closely analizing the animation clip
    private float _ikWeight = 0.0f;
    private float _ikMaxWeight = 0.3f;
    private float _weightStartTime = 0.40f; // times are in seconds
    private float _weightEndTime = 2.00f;
    private float _currentTime = 0.0f;
    private float _pushDuration = 4.33f;
    private float _weightIncrement = 0.02f;
    private bool _resetTime = true;

    // METHODS
    void Start()
    {
        _animator = transform.GetComponent<Animator>();
    }

    void OnAnimatorIK() // calculating the Inverse Kinematics
    {
        // enable IK when the character is pushing a target button: set the position and rotation directly to the goal. 
        if (Ball.GetComponent<PlayerController>().IsPushing())
        {
            // Handle time
            if (_resetTime) // initialize timer
            {
                _resetTime = false;
                _currentTime = _pushDuration;
            }
            // weight should be 0 at the start of the push animation
            // the arms are stretched out after 0.35 seconds have passed
            // the arms relax after 3.40 seconds
            if (_currentTime > 0.0f)
            {
                _currentTime -= Time.deltaTime;
                if (_currentTime < _pushDuration - _weightStartTime && _currentTime > _pushDuration - _weightEndTime)
                {
                    // gradually increase weight
                    _ikWeight = Mathf.Clamp(_ikWeight, _ikWeight + _weightIncrement, _ikMaxWeight);
                }
                else
                {
                    // gradually decrease weight
                    _ikWeight = Mathf.Clamp(_ikWeight, 0.0f, _ikWeight - _weightIncrement);
                }
            }

            // Make sure a target has been assigned
            if (TargetButton != null)
            {
                // Set the look target position
                _animator.SetLookAtWeight(_ikWeight);
                _animator.SetLookAtPosition(TargetButton.transform.position);

                // Set the right hand target position and rotation
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _ikWeight);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _ikWeight);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, TargetButton.transform.position);
                _animator.SetIKRotation(AvatarIKGoal.RightHand, TargetButton.transform.rotation);

                // Set the left hand target position and rotation
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _ikWeight);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _ikWeight);
                _animator.SetIKPosition(AvatarIKGoal.LeftHand, TargetButton.transform.position);
                _animator.SetIKRotation(AvatarIKGoal.LeftHand, TargetButton.transform.rotation);
            }
            else //if the IK is not active, set the position and rotation of the hands and head back to the original position
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                _animator.SetLookAtWeight(0);
            }
        }
        else // prepare for the next push timer as soon as this one ends
        {
            _resetTime = true;
            _ikWeight = 0.0f;
        }
    }
}
