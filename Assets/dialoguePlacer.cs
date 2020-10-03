using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialoguePlacer : MonoBehaviour
{
    // Start is called before the first frame update

    private char[] textToDisplay;
    [SerializeField] TMPro.TextMeshProUGUI textDisplay;
    private int lengthOfText;
    private float fadeInTime = .1f;
    private float fadeOutTime = 1f;

    public void Initalize(string text)
    {

        textToDisplay = text.ToCharArray();

        lengthOfText = textToDisplay.Length;
        StartCoroutine(DisplayText());

    }


    private void FadeCharacter(char character)
    {
        Color originalColor = textDisplay.color;
        string grexText;
        string currentText = textDisplay.text;
        for (float t = 0.01f; t < fadeInTime; t += Time.deltaTime)
        {
            textDisplay.text = currentText;
            Color color = Color.Lerp(Color.clear, originalColor, Mathf.Min(1, t / fadeInTime));
            grexText = "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + character + "</color>";
            textDisplay.text = textDisplay.text + grexText;
        }
        textDisplay.text = currentText;
        textDisplay.text = textDisplay.text + character;
    }

    private IEnumerator DisplayText()
    {
        for (int i = 0; i < lengthOfText; i++)
        {
            yield return new WaitForSeconds(.1f);
            FadeCharacter(textToDisplay[i]);
        }
        yield return new WaitForSeconds(5f);
        StartCoroutine(RemoveText());

    }
    private void DestroyText()
    {
        textDisplay.color = Color.clear;
        Destroy(this.transform.parent.gameObject);

    }
    private IEnumerator RemoveText()
    {
        Color originalColor = textDisplay.color;
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            
            Color color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            textDisplay.color = color;
            yield return null;


        }
        DestroyText();
    }
}
