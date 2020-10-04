using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    Dictionary<string, string> npcLines = new Dictionary<string, string>()
    {
        
        {"I command you to go here!", "I shall do as you command." },
        {"I command you to put 1 here!", "As you wish, my lord." },
        {"I command you to put 2 here!", "whatever you wish." },
        {"I command you to put 3 here!", "It shall be done." }
    };

    Dictionary<string, string> godLines = new Dictionary<string, string>()
    {
        {"move there", "I command you to go here!" },
        {"Put 1 there", "I command you to put 1 here!" },
        {"Put 2 there", "I command you to put 2 here!" },
        {"Put 3 there", "I command you to put 3 here!" } 
    };

    [SerializeField] GameObject _DisplayPrefab;
    // Start is called before the first frame update
    private void CreateNewMessage(string text, bool god)
    {
        GameObject display =  Instantiate(_DisplayPrefab, transform.position, Quaternion.identity, transform);
        display.GetComponent<DialogueBoxManager>().Initalize(text, god);
    }


    void Start()
    {
        StartNewDialogue("move there", true);
    }

    public void StartNewDialogue(string key, bool god)
    {
        if(god == true)
        {
            if(godLines.ContainsKey(key))
            {
                CreateNewMessage(godLines[key], god);
            }
        }

        else
        {
            if (npcLines.ContainsKey(key) == true)
            {
                CreateNewMessage(npcLines[key], god);
            }
        }
    }
    
    // Update is called once per frame

}
