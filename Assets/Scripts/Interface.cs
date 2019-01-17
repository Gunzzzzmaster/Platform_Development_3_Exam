using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour {

    // FIELDS
    public GameObject GameManagerObject;
    public float TextMargin = 20.0f; // amount of whitespace from the edge of the screen, in pixels
    public Font CustomFont;
    public Texture HeartImage;

    private GameManager _gameManager;
    private int _fontSize = 26;
    private int _spacing = 40; // spacing between newlines
    private float _dangerTime = 20.0f; // seconds
    private int _bitmapWidth;
    private int _bitmapHeight;
    private float _scaling = 2.0f;

    // METHODS
    void Start()
    {
        _gameManager = GameManagerObject.GetComponent<GameManager>();
        _bitmapWidth = HeartImage.width;
        _bitmapHeight = HeartImage.height;
    }

    void OnGUI()
    {
        //if (!GameManagerObject.GetComponent<GameManager>().IsGamePaused())
        if (Time.timeScale != 0f)
        {
            // print UI
            GUIStyle style = new GUIStyle();
            style.font = CustomFont;
            style.fontSize = _fontSize;
            style.normal.textColor = Color.white;

            Rect textPosition = new Rect(TextMargin, TextMargin, Screen.width - TextMargin, Screen.height - TextMargin);
            float startYPos = textPosition.y;
            float startXPos = textPosition.x;
            string textToPrint;

            // top left
            textToPrint = "Score: " + _gameManager.GetScore().ToString();
            GUI.Label(textPosition, textToPrint, style);

            style.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 1); // light grey
            textPosition.y += _spacing;
            textToPrint = "Highscore: " + _gameManager.GetHighscore().ToString();
            GUI.Label(textPosition, textToPrint, style);


            style.normal.textColor = new Color(0.7f, 0.5f, 0, 1); // brown
            textPosition.y += _spacing;
            textToPrint = "Lives left: " + _gameManager.GetLives().ToString();
            GUI.Label(textPosition, textToPrint, style);

            // draw lives
            Rect textureFrame = new Rect();
            textureFrame.x = textPosition.x;
            textureFrame.y = textPosition.y + _spacing;
            textureFrame.width = _bitmapWidth * _scaling;
            textureFrame.height = _bitmapHeight * _scaling;
            for (int i = 0; i < _gameManager.GetLives(); i++)
            {
                GUI.DrawTexture(textureFrame, HeartImage, ScaleMode.ScaleToFit);
                textureFrame.x += _bitmapWidth + _spacing / 2.0f;
            }

            // top center

            style.normal.textColor = new Color(0.9f, 0.5f, 0, 1); // dark orange
            if (_gameManager.GetTime() < _dangerTime)
            {
                style.normal.textColor = Color.red; // turns red when almost out of time
            }
            textPosition.y = startYPos;
            textPosition.x = Screen.width / 2 - 90;
            textToPrint = "Time left: ";
            GUI.Label(textPosition, textToPrint, style);

            textPosition.y += _spacing;
            textPosition.x = Screen.width / 2 - 60;
            textToPrint = _gameManager.GetTime().ToString("000.##");
            GUI.Label(textPosition, textToPrint, style);

            // top right

            style.normal.textColor = new Color(1, 0.8f, 0, 1); // yellow

            textPosition.x = 0;
            textPosition.y = startYPos;
            style.alignment = TextAnchor.UpperRight;
            textToPrint = "Pickups: " + _gameManager.GetCurrentPickups().ToString("D2") + "/" + _gameManager.GetTotalPickups().ToString("D2");
            GUI.Label(textPosition, textToPrint, style);
            style.alignment = TextAnchor.UpperLeft;

            style.normal.textColor = Color.white;
            textPosition.x = startXPos;
            textPosition.y = Screen.height - 50;
            // display the speed, GetBallSpeed() is in km/h
            textToPrint = "Speed: " + _gameManager.GetBallSpeed() + " km/h";
            GUI.Label(textPosition, textToPrint, style);

            // middle of the screen

            if (_gameManager.ShowControls())
            {
                style.normal.textColor = new Color(0, 0.9f, 1, 1); // light blue
                style.alignment = TextAnchor.MiddleCenter;
                textPosition.x = 10.0f;
                textPosition.y = Screen.height / 3.0f;
                style.fontSize = _fontSize + 8;
                textToPrint = "Use the joysticks to move.";
                GUI.Label(textPosition, textToPrint, style);
                textPosition.y += 54;
                textToPrint = "Press the 'A' button to interact.";
                GUI.Label(textPosition, textToPrint, style);
                textPosition.y += 54;
                textToPrint = "Press Start to pause the game.";
                GUI.Label(textPosition, textToPrint, style);
            }

            // show fly UI
            else if (_gameManager.FlyUIEnabled())
            {
                style.normal.textColor = new Color(0, 0.9f, 1, 1); // light blue
                style.alignment = TextAnchor.MiddleCenter;
                textPosition.x = 10.0f;
                textPosition.y = Screen.height / 3.0f;
                style.fontSize = _fontSize + 8;
                textToPrint = "Press the 'A' button to start flying.";
                GUI.Label(textPosition, textToPrint, style);
                textPosition.y += 54;
                textToPrint = "Use the joysticks and 'A' and 'B' buttons to move.";
                GUI.Label(textPosition, textToPrint, style);
                textPosition.y += 54;
                textToPrint = "You can only fly in the fly area.";
                GUI.Label(textPosition, textToPrint, style);
            }

            // show press button UI
            else if (_gameManager.ButtonUIEnabled())
            {
                style.normal.textColor = new Color(0, 0.9f, 1, 1); // light blue
                style.alignment = TextAnchor.MiddleCenter;
                textPosition.x = 10.0f;
                textPosition.y = Screen.height / 3.0f;
                style.fontSize = _fontSize + 8;
                textPosition.y += 54;
                textToPrint = "Press the 'A' button to interact.";
                GUI.Label(textPosition, textToPrint, style);
            }
        }
    }
}
