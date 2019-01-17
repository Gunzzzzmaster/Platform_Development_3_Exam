using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour {

    // FIELDS
    public GameObject SliderObject;

    // METHODS
    void Start()
    {
        SliderObject.GetComponent<Slider>().value = AudioListener.volume;
    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // slider
    public void ChangeVolume()
    {
        AudioListener.volume = SliderObject.GetComponent<Slider>().value;
    }
}
