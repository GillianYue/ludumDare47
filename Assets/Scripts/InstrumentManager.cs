using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentManager : MonoBehaviour
{
    public GameController gameControl;
    public int currInstrIndex;
    public GameObject[] InstrIcons, spawnIcons;

    void Start()
    {
        for(int i = 0; i< InstrIcons.Length; i++)
        {
            InstrIcons[i].SetActive(i == currInstrIndex);
            spawnIcons[i].SetActive(i == currInstrIndex);
        }
    }

    void Update()
    {
/*        if (Input.GetKeyDown(KeyCode.D))
        {
            nextSkill();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            previousSkill();
        }*/
    }


    public void updateInstrumentIcon(int index)
    {
        InstrIcons[currInstrIndex].SetActive(false);
        InstrIcons[index].SetActive(true);

        spawnIcons[currInstrIndex].SetActive(false);
        spawnIcons[index].SetActive(true);

        currInstrIndex = index;
    }

    public void disableAll()
    {
        for (int i = 0; i < InstrIcons.Length; i++)
        {
            InstrIcons[i].SetActive(false);
            spawnIcons[i].SetActive(false);
        }
    }

}
