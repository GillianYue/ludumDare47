﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//--------------------------------------------------------------------
//Attach to objects to be used by InSceneLevelSwitcher as starting points
//--------------------------------------------------------------------
[System.Serializable]
public class InSceneLevel : MonoBehaviour {

    public enum levelType { DRUM, PAD, BASS, GUITAR };
    public levelType type;

    public string puzzleSource, puzzleSpawnSource;
    private int[] puzzle, puzzleSpawnAssignment; //spawn assignment is data on what stuff should be spawned on start of the level
    public int[] currAssignment;
    public int dx, dxNoise, startDistance;

    public float stoneSpawnDeltaTime;

    public GeometrySpawner geometrySpawner;
    public stoneBehavior[] stones;
    public int defaultGenerateNote;

    public bool useNotes;
    private int currentCorrectCount;

    void Start()
    {
        puzzle = new int[puzzleSource.Length];
        puzzleSpawnAssignment = new int[puzzleSpawnSource.Length];

        char[] ca = puzzleSource.ToCharArray();
        for (int c = 0; c<puzzleSource.Length; c++)
        {
            int.TryParse(puzzleSource.Substring(c, 1), out puzzle[c]);
            int.TryParse(puzzleSpawnSource.Substring(c, 1), out puzzleSpawnAssignment[c]);
        }

        stones = new stoneBehavior[16];

    }

    public IEnumerator setupLevel()
    {
        yield return StartCoroutine(putPuzzlesInPlace());
        checkForLevelPass(false);
    }

    private IEnumerator putPuzzlesInPlace()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(geometrySpawner.gameController.playCurrentLoop());

        Vector3 startDelta = geometrySpawner.getStartOrigin() + new Vector3(startDistance, 0, 0); //we only need the x here

        GameObject cam = geometrySpawner.getCameraTransform().gameObject;
        StartCoroutine(waitThenMoveCam(cam, 0.6f, new Vector3(startDelta.x + 16 * dx, geometrySpawner.getCameraLookat().y, startDelta.z), 16 * stoneSpawnDeltaTime));

        for(int a=0; a<16; a++)
        {
            Vector3 currXDelta = new Vector3(startDelta.x + a * dx + Random.Range(0, dxNoise), 0, 0);
            geometrySpawner.spawnStone(stones, a, type, currXDelta, type!= levelType.BASS? new Vector3(0, 0, Random.Range(-geometrySpawner.angleNoise, geometrySpawner.angleNoise)) : 
                Vector3.zero, puzzleSpawnAssignment[a]);
            yield return new WaitForSeconds(stoneSpawnDeltaTime);
        }
        currAssignment = puzzleSpawnAssignment;

        yield return new WaitForSeconds(1f);
    }

    public int pickRandomNoteOption(int index)
    {
        int puzzleNote = puzzle[index];
        int op1 = puzzleNote + 3, op2 = puzzleNote + 5;
        if (op1 > 7) op1 -= 7; if (op2 > 7) op2 -= 7;

        switch(Random.Range(0, 2))
        {
            case 0: return op2;
            case 1: return op1;
        }

        return op1;
    }

    private IEnumerator waitThenMoveCam(GameObject cam, float sec, Vector3 dest, float moveTime)
    {
        yield return new WaitForSeconds(sec);
        StartCoroutine(Global.moveToInSecs(cam, dest, moveTime));
    }

    public void setStoneAssignment(int spot, int assignment, Transform stoneTransform)
    {
        currAssignment[spot] = assignment;
        checkForSingleStone(spot); //see if this assignment is "correct"

        if (stoneTransform != null)
        {
            Vector3 delta = new Vector3(0, geometrySpawner.notesOffsetY[assignment], 0);
            stoneTransform.position = stoneTransform.position + delta;
            stones[spot] = stoneTransform.GetComponent<stoneBehavior>();
        }
    }

    public void checkForLevelPass(bool effect)
    {
        int previous = currentCorrectCount;

        int correctCount = 0;
        for(int a = 0; a<16; a++)
        {
            if (currAssignment[a] == puzzle[a]) correctCount++;
            //else Debug.Log("mismatch at " + a + ": curr " + currAssignment[1] + " and puzzle " + puzzle[a]);
        }

        if (correctCount > previous && effect) {
            StartCoroutine(delayEffect());
        }
        geometrySpawner.correctNumberText.text = correctCount.ToString() + "/16";
        if (correctCount == 16)
        {
            for(int s = 0; s<16; s++)
            {
                if(currAssignment[s] != 0)
                {
                    Global.horizShakeAnim(stones[s].gameObject, this);
                }
            }
            geometrySpawner.levelCompleteSFX.Play();
            StartCoroutine(geometrySpawner.gameController.ascendFloor());
            gameObject.SetActive(false);
        }

        currentCorrectCount = correctCount;
    }

    IEnumerator delayEffect()
    {
        yield return new WaitForSeconds(0.6f);
        geometrySpawner.correctTextAnim.SetTrigger("a");
        geometrySpawner.singleMatchCorrectSFX.Play();
    }

    public bool checkForSingleStone(int index)
    {
        checkForLevelPass(true);
        return (currAssignment[index] == puzzle[index]);
    }

    public bool canModifyStone(int index)
    {
        return (puzzleSpawnAssignment[index] == 0);
    }

    public void currentBeatStoneAnim(int index)
    {
     if(stones[index])   stones[index].beatAnim();
    }

    public void toggleNote(int index, Transform stone)
    {
        if (transform.tag.Equals("Space") || !useNotes) return;

        int originalNote = currAssignment[index];
        int puzzleNote = puzzle[index];

        int op1 = puzzleNote + 3, op2 = puzzleNote + 5;
        if (op1 > 7) op1 -= 7; if (op2 > 7) op2 -= 7;

        currAssignment[index] = (originalNote == op1) ? (op2) : ((originalNote == op2)? puzzleNote: op1);

        if(currAssignment[index] != puzzleNote) geometrySpawner.hammerSFX.Play();

        Vector3 stoneY = geometrySpawner.getFloorOrigin();
        stoneY += new Vector3(0, geometrySpawner.notesOffsetY[currAssignment[index]], 0);
        float currX = stone.transform.position.x;
        stone.transform.position = new Vector3(currX, stoneY.y, 0);

        checkForSingleStone(index);
    }
}
