using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxManager : MonoBehaviour
{
    public void Initalize(string text, bool god)
    {
        GetComponentInChildren<DialoguePlacer>().Initalize(text, god);
        Debug.Log(god);
        if(god == true)
        {
            transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "God";
            transform.GetChild(1).GetComponent<Image>().color = Color.red;
        }
        else
        {
            Debug.Log("what?");
            transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Spirit";
            transform.GetChild(1).GetComponent<Image>().color = Color.blue;
        }
    }
}
