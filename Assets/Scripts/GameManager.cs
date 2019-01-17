using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // FIELDS
    private List<GameObject> _pickUpArray = new List<GameObject>();

    public GameObject Ball;
    public GameObject Character;
    public GameObject ParticleEffects;
    public float ShowControlsTime = 12.0f; // seconds

    private float _initialTime = 300.0f; // seconds (= 5 min)
    private float _currentTime;
    private int _initialLives = 3;
    private int _lives;
    private static int _score = 0;
    private int _highscore = 0;
    private int _currentPickups = 0;
    private int _totalPickups;
    private bool _showControls = true;
    private bool _isGamePaused = false;
    private bool _isGameOver = false;
    private bool _isLevelComplete = false;
    private bool _hardPause = false;
    private bool _showButtonUI = false;
    private bool _showFlyUI = false;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Quaternion _startPlayerDirection;

    // METHODS
    void Start()
    {
        // save startpostition of the ball
        _startPosition = Ball.transform.position;
        _startRotation = Ball.transform.rotation;
        _startPlayerDirection = Character.transform.rotation;

        // initialize some datamembers
        _lives = _initialLives;
        _currentTime = _initialTime;
        _score = 0;

        foreach (GameObject pickup in GameObject.FindGameObjectsWithTag("pickup"))
        {
            _pickUpArray.Add(pickup);
        }
        _totalPickups = _pickUpArray.Count;
    }

    void Update()
    {
        HandleHardPause();
        if (_lives < 0)
        {
            //ResetLevel();
            _isGameOver = true;
        }
        HandleTime();

        if (_showControls & _initialTime - _currentTime >= ShowControlsTime)
        {
            _showControls = false;
        }

        HandlePause();
    }

    //public Vector3 GetStartPosition()
    //{
    //    return _startPosition;
    //}

    public int GetScore()
    {
        return _score;
    }

    public int GetHighscore()
    {
        return _highscore;
    }

    public int GetLives()
    {
        return _lives;
    }

    public int GetInitialLives()
    {
        return _initialLives;
    }

    public void LoseLife()
    {
        _lives -= 1;
        RespawnBall();
    }

    public void AddPoints(int points)
    {
        _score += points;
        ParticleEffects.GetComponent<ParticleEffects>().CreateFadingText("+", points);
    }

    public void LosePoints(int points)
    {
        _score -= points;
        if (_score < 0)
            _score = 0;
        ParticleEffects.GetComponent<ParticleEffects>().CreateFadingText("-", points);
    }

    public void ResetLevel() // Not used in the final build of the game
    {
        // reset game when out of lives and check for highscore
        if (_score > _highscore)
        {
            _highscore = _score;
        }
        _score = 0;
        _lives = _initialLives;

        // Enable Pick-ups
        EnableAllPickups();
        _currentPickups = 0;

        // Reset Ball
        RespawnBall();

        // Reset Timer
        _currentTime = _initialTime;
    }

    public void RespawnBall()
    {
        Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.transform.position = _startPosition;
        Ball.transform.rotation = _startRotation;
        Character.transform.rotation = _startPlayerDirection;
    }

    void HandleTime()
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime < 0.0f)
        {
            _currentTime = 0.0f;
            //ResetLevel();
            _isGameOver = true;
        }
    }

    public float GetTime()
    {
        return _currentTime;
    }

    public int GetTotalPickups()
    {
        return _totalPickups;
    }

    public int GetCurrentPickups()
    {
        return _currentPickups;
    }

    public void EnableAllPickups()
    {
        foreach (GameObject pickup in _pickUpArray)
        {
            pickup.GetComponent<PickUpHandler>().EnablePickUp();
        }
    }

    public bool AllPickupsTaken()
    {
        return (_currentPickups == _totalPickups);
    }

    public void AddPickup()
    {
        _currentPickups++;
    }

    public float GetBallSpeed() // in km/h
    {
        // return the speed, but convert from m/s to km/h first
        // 1 m/s = 3600 m/h = 3.6 km/h
        float v = Ball.transform.GetComponent<Rigidbody>().velocity.magnitude;
        return (int)(v * 3.6f);
    }

    public bool ShowControls()
    {
        return _showControls;
    }

    private void HandleHardPause()
    {
        if (_hardPause)
            Time.timeScale = 0f;
    }

    private void HandlePause()
    {
        if (!_hardPause)
        {
            // Pause/resume game when Menu-button is pressed.
            if (Input.GetButtonDown("Menu"))
            {
                _isGamePaused = !_isGamePaused;
            }

            if (_isGamePaused)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }
    }

    public bool IsGamePaused()
    {
        return _isGamePaused;
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    public void HardPauseGame(bool pause)
    {
        _hardPause = pause;
    }

    public void LevelComplete(bool complete)
    {
        _isLevelComplete = complete;

        // since reset level doesn't get executed anymore we will update the highscore here:
        if (complete && (_score > _highscore))
        {
            _highscore = _score;
        }
    }

    public bool IsLevelComplete()
    {
        return _isLevelComplete;
    }

    public void ShowButtonUI(bool isEnabled)
    {
        _showButtonUI = isEnabled;
    }

    public bool ButtonUIEnabled()
    {
        return _showButtonUI;
    }

    public void ShowFlyUI(bool isEnabled)
    {
        _showFlyUI = isEnabled;
    }

    public bool FlyUIEnabled()
    {
        return _showFlyUI;
    }
}
