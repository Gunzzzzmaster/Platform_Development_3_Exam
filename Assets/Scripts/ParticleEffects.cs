using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffects : MonoBehaviour {

    // FIELDS
    public Font CustomFont;

    [SerializeField]
    private List<GameObject> _fadingTextObjectArray = null;

    // METHODS
    void Update()
    {
        // delete the particle effects when they disappear
        if (_fadingTextObjectArray.Count > 0)
        {
            for (int i = 0; i < _fadingTextObjectArray.Count; i++)
            {
                if (_fadingTextObjectArray[i] != null)
                {
                    if (_fadingTextObjectArray[i].GetComponent<FadingText>().IsTimeUp())
                    {
                        Destroy(_fadingTextObjectArray[i]);
                        _fadingTextObjectArray[i] = null;
                        // Test
                        //Debug.Log("particle destroyed");
                    }
                }
            }
        }
    }

    public void CreateFadingText(string plusMinus, int points)
    {
        // create new object and add script with string initialisation
        GameObject fadingTextObject = new GameObject();
        fadingTextObject.AddComponent<FadingText>();
        fadingTextObject.GetComponent<FadingText>().SetString(plusMinus + " " + points.ToString());

        // add to array
        if (_fadingTextObjectArray.Count > 0)
        {
            for (int i = 0; i < _fadingTextObjectArray.Count; i++)
            {
                if (_fadingTextObjectArray[i] == null)
                {
                    _fadingTextObjectArray[i] = fadingTextObject;
                    return;
                }
            }
        }
        _fadingTextObjectArray.Add(fadingTextObject);
    }

    public Font GetFont()
    {
        return CustomFont;
    }
}
