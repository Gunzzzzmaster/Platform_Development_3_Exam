using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingText : MonoBehaviour {

    // FIELDS
    public int FontSize = 42;
    private Rect _textPosition;
    private string _textToPrint;
    private int _offset = 40; // how much whitespace is needed from the edges of the screen, value in pixels
    private float _displayTime = 1.4f; // seconds
    private float _currentTime;
    private float _alpha = 1.0f; // text transparency
    private bool _timeUp = false;

    // METHODS
    // Use this for initialization
    void Start()
    {
        _textPosition = new Rect(Screen.width / 2.0f + _offset * 2.0f, Screen.height / 2.0f + _offset, Screen.width, Screen.height);
        _currentTime = _displayTime;
    }

    void OnGUI()
    {
        if (_currentTime >= 0.0f)
        {
            // print string on screen for player feedback
            GUIStyle customStyle = new GUIStyle();
            customStyle.font = FindObjectOfType<ParticleEffects>().GetFont();
            customStyle.fontSize = FontSize;
            customStyle.normal.textColor = new Color(255, 255, 255, _alpha); // white

            GUI.Label(_textPosition, _textToPrint, customStyle);

            // have text move up and fade out over time
            _currentTime -= Time.deltaTime;
            _textPosition.y--;
            _alpha -= 1.0f / (_displayTime * 60.0f);
        }
        else
        {
            _timeUp = true;
        }
    }

    public void SetString(string text)
    {
        _textToPrint = text;
    }

    public bool IsTimeUp()
    {
        return _timeUp;
    }
}
