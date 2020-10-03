using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class dialoguePlacer : MonoBehaviour
{
    // Start is called before the first frame update

    private char[] textToDisplay;
    private TMPro.TextMeshPro textDisplay;
    private int lengthOfText;
    

    public void Initalize(string text)
    {
        textToDisplay = text.ToCharArray();

        lengthOfText = textToDisplay.Length;
        StartCoroutine(DisplayText());
    }

    // Start is called before the first frame update
    void Start()
    {
        textDisplay = GetComponent<TMPro.TextMeshPro>();

    }



    private IEnumerator DisplayText()
    {
        for(int i = 0; i < lengthOfText; i++)
        {
            yield return new WaitForSeconds(.05f);
            textDisplay.text = textDisplay.text + textToDisplay[i];

        }
    }
    
}
