using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class dialoguePlacer : MonoBehaviour
{
    // Start is called before the first frame update

    private char[] textToDisplay;
    private TMPro.TextMeshPro textDisplay;
    private int lengthOfText;
    private Text text;
    private float fadeInTime = .04f;

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
        text = GetComponent<Text>();
    }

    private IEnumerator FadeCharacter(char character)
    {
        Color originalColor = text.color;
        string grexText = "<color = clear>" + character + "</color>";
        string currentText = text.text;
        text.text = text.text + grexText;
        for (float t = 0.01f; t < fadeInTime; t += Time.deltaTime)
        {
            text.text = currentText;
            Color color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeInTime));
            grexText = "<color = "+ ColorUtility.ToHtmlStringRGBA(color) + ">" + character + "</color>";
            text.text = text.text + grexText;
            yield return null;
        }
    }

    private IEnumerator DisplayText()
    {
        for(int i = 0; i < lengthOfText; i++)
        {
            yield return new WaitForSeconds(.01f);
            StartCoroutine(FadeCharacter(textToDisplay[i]));
            

        }
    }
    
}
