using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stoneBehavior : MonoBehaviour
{
    public InSceneLevel myLevel;
    public SpriteRenderer mySprite;
    private float originalOpacity;
    public int index = -1, note; //by default, NA

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
        if (intersectingSpace == null) return false;
        else transform.position = intersectingSpace.transform.position;
        index = intersectingSpace.index;
        note = (myLevel.defaultGenerateNote == -1) ? myLevel.pickRandomNoteOption(index) : myLevel.defaultGenerateNote;
        intersectingSpace.gameObject.SetActive(false);
        myLevel.setStoneAssignment(index, note, transform); //will also check for level pass
        return true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (myLevel)
        {
            if (tag.Equals("Space") && other.tag.Equals("Stone"))
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

    public void beatAnim()
    {
        if (tag.Equals("Stone"))
        {
            Global.rotateAnim(gameObject, this);
        }
        else if (tag.Equals("Space"))
        {
            StartCoroutine(Global.brieflyChangeColor(mySprite.color, new Color(250, 130, 0, mySprite.color.a), mySprite, 0.4f));
        }
    }

    void OnMouseEnter()
    {
        if (myLevel)
        {
            myLevel.geometrySpawner.currMouseOver = this;
        }
    }

    void OnMouseExit()
    {
        if (myLevel && myLevel.geometrySpawner.currMouseOver &&
            myLevel.geometrySpawner.currMouseOver.Equals(this)) myLevel.geometrySpawner.currMouseOver = null;
    }

    public void removeThisStone()
    {
        Vector3 startDelta = myLevel.geometrySpawner.getStartOrigin() + new Vector3(myLevel.startDistance, 0, 0); //we only need the x here

        myLevel.geometrySpawner.spawnHighlightAt(myLevel.stones, index, new Vector3(startDelta.x + index * myLevel.dx + Random.Range(0, myLevel.dxNoise), 0, 0));
        myLevel.setStoneAssignment(index, 0, null);
        Destroy(gameObject);
    }

}
