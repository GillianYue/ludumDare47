using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stoneBehavior : MonoBehaviour
{
    public InSceneLevel myLevel;
    public SpriteRenderer mySprite;
    private float originalOpacity;
    public int index = -1; //by default, NA

    public stoneBehavior intersectingSpace;

    void Start()
    {
        mySprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        originalOpacity = mySprite.color.a;
    }


    void Update()
    {
        
    }

    public bool checkAssign()
    {
        Debug.Log("checking assign");
        if (intersectingSpace == null) return false;
        else transform.position = intersectingSpace.transform.position;
        index = intersectingSpace.index;
        intersectingSpace.gameObject.SetActive(false);
        myLevel.setStoneAssignment(index, 1, transform); 
        return true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (myLevel)
        {
            if(tag.Equals("Space") && other.tag.Equals("Stone"))
            {
            }
            if (tag.Equals("Space") && other.tag.Equals("Stone") && myLevel.canModifyStone(index))
            {
                mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, originalOpacity + 0.4f);
            }

            if (other.tag.Equals("Space") && tag.Equals("Stone"))
            {
                intersectingSpace = other.GetComponent<stoneBehavior>();
            }

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (myLevel)
        {

            if (tag.Equals("Space") && other.tag.Equals("Stone"))
            {
                mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, originalOpacity);
            }


            if (other.tag.Equals("Space") && tag.Equals("Stone"))
            {
                intersectingSpace = null;
            }
        }
    }
}
