﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    Dictionary<string, string> npcLines = new Dictionary<string, string>()
    {
        {"move there ok", "I shall do as you command." },
        {"Put 1 there", "As you wish, my lord." },
        {"Put 2 there", "whatever you wish." },
        {"Put 3 there", "It shall be done." }
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
