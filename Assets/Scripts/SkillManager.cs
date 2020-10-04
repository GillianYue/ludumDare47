using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public GameController gameControl;
    public int currSkillIndex;
    public GameObject[] SkillIcons;

    void Start()
    {
        for(int i = 0; i<SkillIcons.Length; i++)
        {
            SkillIcons[i].SetActive(i == currSkillIndex);
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

/*    public void nextSkill()
    {
        Debug.Log("in next: " + currSkillIndex);
        if (currSkillIndex == gameControl.SkillLv)
        { //maxed out
            updateSkillIcon(0);
        }
        else
        {
            updateSkillIcon(currSkillIndex + 1);
        }

    }

    public void previousSkill()
    {
        if(currSkillIndex == 0)
        {
            updateSkillIcon(gameControl.SkillLv);
        }
        else
        {
            updateSkillIcon(currSkillIndex-1);
        }
    }

    public void updateSkillIcon(int index)
    {
        if (gameControl.SkillLv < index) return;

        //TODO livelier transition
        SkillIcons[currSkillIndex].SetActive(false);
        SkillIcons[index].SetActive(true);
        currSkillIndex = index;
    }*/

    public void updateInstrumentIcon(int index)
    {
        SkillIcons[currSkillIndex].SetActive(false);
        SkillIcons[index].SetActive(true);
        currSkillIndex = index;
    }


}
