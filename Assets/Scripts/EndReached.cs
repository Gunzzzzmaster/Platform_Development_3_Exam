using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndReached : MonoBehaviour {

    // FIELDS
    public GameObject GameManagerObject;
    public int BonusPointsPerLife = 60;
    public int BonusPointsPerTenSeconds = 20;
    public int NoDeathsBonus = 120;
    public int AllPickupsBonus = 200;

    private GameManager _gameManager;
    [SerializeField]
    private bool _isEnabled = false;

    // METHODS
    void Start()
    {
        _gameManager = GameManagerObject.GetComponent<GameManager>();
        EnablePortal(_isEnabled);
    }

    void OnCollisionEnter(Collision col)
    {
        // telling gamemanager when end is reached
        if (col.gameObject.name == "Ball")
        {
            _gameManager.AddPoints(DetermineExtraPoints());
            //col.rigidbody.velocity = Vector3.zero;
            //col.transform.position = _gameManager.GetStartPosition();
            transform.GetComponent<AudioSource>().Play();
            //_gameManager.ResetLevel();
            _gameManager.LevelComplete(true);
        }
    }

    int DetermineExtraPoints() // add bonus points
    {
        int bonusPoints = BonusPointsPerLife * _gameManager.GetLives();
        if (_gameManager.GetLives() == _gameManager.GetInitialLives())
        {
            bonusPoints += NoDeathsBonus;
        }

        bonusPoints += (int)(_gameManager.GetTime() / 10) * BonusPointsPerTenSeconds;

        int pickupBonus = _gameManager.AllPickupsTaken() ? AllPickupsBonus : 0;
        bonusPoints += pickupBonus;

        return bonusPoints;
    }

    public void EnablePortal(bool isEnabled)
    {
        //Debug.Log("switch portal");
        _isEnabled = isEnabled;
        transform.GetComponent<MeshRenderer>().enabled = isEnabled;
        transform.GetComponent<MeshCollider>().enabled = isEnabled;
    }

    public bool IsEnabled()
    {
        return _isEnabled;
    }
}
