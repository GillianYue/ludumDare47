using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeometrySpawner : MonoBehaviour
{
    public GameObject startFloor, secondFloor, currFloor;
    public GameObject floorPrefab, highlightPrefab;
    public Material[] floorMaterials;
    private Vector3 startOrigin; //floors will be spawned based on this position + dy * levelNum
    public float dy, angleNoise; //height of each floor; increment difference in height
    public int[] floorType; //stores the material index of this floor

    public GameObject[] padCrystals, bassStones, drumCubes, branchRocks;
    public int[] notesOffsetY; //int[7]s that indicate y offset for different notes - 0 for do, etc. 
    public GameController gameController;
    public Camera cam;

    public GameObject geometryHolder, currSpawnedStone;
    public stoneBehavior currMouseOver;
    public Text correctNumberText;

    public AudioSource placeSFX, cancelSFX, hammerSFX, levelCompleteSFX, singleMatchCorrectSFX;
    public Animator correctTextAnim;


    void Start()
    {
        startOrigin = startFloor.transform.position;
/*        dy = Mathf.Abs(secondFloor.transform.position.y - startFloor.transform.position.y);
        Debug.Log("dy is " + dy);*/
        currFloor = startFloor;
        cam = gameController.m_Camera.GetComponent<Camera>();
    }

    void Update()
    {
        if (currSpawnedStone != null)
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            currSpawnedStone.transform.position = new Vector3(pos.x, pos.y, 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseClick();
        }

        if (Input.GetMouseButtonDown(1))
        {
            mouseRightClick();
        }
    }

    public Transform getCameraTransform()
    {
        return gameController.m_Camera;
    }

    public Vector3 getStartOrigin()
    {
        return startOrigin;
    }

    public Vector3 getCameraLookat(int level, Vector3 camOffset)
    {
        return new Vector3(startOrigin.x + camOffset.x, startOrigin.y + level * dy + camOffset.y, 0);
    }

    public Vector3 getCameraLookat()
    {
        return new Vector3(startOrigin.x + gameController.playerStartPosOffset.x, startOrigin.y + gameController.currLevel * dy + gameController.playerStartPosOffset.y, 0);
    }

    public IEnumerator spawnNewFloor(int floorIndex)
    {
        if (floorType[floorIndex] == -1) yield return null;

        Material floorMat = floorMaterials[floorType[floorIndex]];
        GameObject f = Instantiate(floorPrefab, geometryHolder.transform);
        //f.transform.GetChild(0).GetComponent<MeshRenderer>().material = floorMat; TODO
        correctNumberText.text = "";

        f.transform.position = startOrigin + new Vector3(0, floorIndex * dy, 0);
        currFloor = f;

        yield return StartCoroutine(gameController.CorrectCamera());
        gameController.setUpCurrLevel();
    }

    public void spawnStone(stoneBehavior[] stones, int index, InSceneLevel.levelType type, Vector3 deltaPosition, Vector3 rotation, int note)
    {
        StartCoroutine(spawnStoneCoroutine(stones, index, type, deltaPosition, rotation, note, false));
    }

    public void spawnStoneMouse()
    {
        if (currSpawnedStone != null) Destroy(currSpawnedStone);
        InSceneLevel.levelType t = gameController.getCurrLevel().type;
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        GameObject go = null;

        switch (t)
        {
            case InSceneLevel.levelType.DRUM:
                go = Instantiate(drumCubes[Random.Range(0, drumCubes.Length)], pos, 
                    Quaternion.Euler(t != InSceneLevel.levelType.BASS ? new Vector3(0, 0, Random.Range(-angleNoise, angleNoise)) : Vector3.zero));
                break;
            case InSceneLevel.levelType.PAD:
                go = Instantiate(padCrystals[Random.Range(0, padCrystals.Length)], pos, 
                    Quaternion.Euler(t != InSceneLevel.levelType.BASS ? new Vector3(0, 0, Random.Range(-angleNoise, angleNoise)) : Vector3.zero));

                break;
            case InSceneLevel.levelType.BASS:
                go = Instantiate(bassStones[Random.Range(0, bassStones.Length)], pos, 
                    Quaternion.Euler(t != InSceneLevel.levelType.BASS ? new Vector3(0, 0, Random.Range(-angleNoise, angleNoise)) : Vector3.zero));
                break;
            case InSceneLevel.levelType.GUITAR:
                go = Instantiate(branchRocks[Random.Range(0, branchRocks.Length)], pos,
                Quaternion.Euler(t != InSceneLevel.levelType.BASS ? new Vector3(0, 0, Random.Range(-angleNoise, angleNoise)) : Vector3.zero));

                break;
        }

        go.transform.parent = currFloor.transform;

        stoneBehavior s = go.GetComponent<stoneBehavior>();
        s.myLevel = gameController.getCurrLevel();
        currSpawnedStone = go;
    }

    public Vector3 getFloorOrigin()
    {
        Vector3 floorOrigin = startOrigin + new Vector3(0, gameController.currLevel * dy, 0);
        return floorOrigin;
    }


    private IEnumerator spawnStoneCoroutine(stoneBehavior[] stones, int index, InSceneLevel.levelType type, Vector3 deltaPosition, Vector3 rotation, int note, bool mouseDrag)
    {
        Vector3 spawnPos = getFloorOrigin() + deltaPosition, deltaUp = new Vector3(0, 40, 0);
        GameObject go = null;

        if (note != 0)
        {
            switch (type)
            {
                case InSceneLevel.levelType.DRUM:
                    if(note != -1) spawnPos += new Vector3(0, notesOffsetY[note], 0);
                    go = Instantiate(drumCubes[Random.Range(0, drumCubes.Length)], spawnPos + deltaUp, Quaternion.Euler(rotation));
                    break;
                case InSceneLevel.levelType.PAD:
                    if (note != -1) spawnPos += new Vector3(0, notesOffsetY[note], 0);
                    go = Instantiate(padCrystals[Random.Range(0, padCrystals.Length)], spawnPos + deltaUp, Quaternion.Euler(rotation));

                    break;
                case InSceneLevel.levelType.BASS:
                    if (note != -1) spawnPos += new Vector3(0, notesOffsetY[note], 0);
                    go = Instantiate(bassStones[Random.Range(0, bassStones.Length)], spawnPos + deltaUp, Quaternion.Euler(rotation));
                    break;
                case InSceneLevel.levelType.GUITAR:
                    if (note != -1) spawnPos += new Vector3(0, notesOffsetY[note], 0);
                    go = Instantiate(branchRocks[Random.Range(0, branchRocks.Length)], spawnPos + deltaUp, Quaternion.Euler(rotation));

                    break;
            }

            go.transform.parent = currFloor.transform;

            if (mouseDrag) currSpawnedStone = go;

            yield return StartCoroutine(Global.moveToInSecs(go, spawnPos, 0.5f));
        }
        else
        { //space, generate hint highlight prefab
            go = Instantiate(highlightPrefab, spawnPos, Quaternion.Euler(0,0,0));
            go.transform.parent = currFloor.transform;
            SpriteRenderer spr = go.transform.GetChild(0).GetComponent<SpriteRenderer>();

            StartCoroutine(Global.brieflyChangeColor(spr.color, new Color(250, 130, 0, spr.color.a), spr, 0.4f));
        }

        stoneBehavior s = go.GetComponent<stoneBehavior>();
        stones[index] = s;
        s.index = index; s.myLevel = gameController.getCurrLevel(); s.note = note;
    }

    public GameObject spawnHighlightAt(stoneBehavior[] stones, int index, Vector3 deltaPosition)
    {
        Vector3 spawnPos = getFloorOrigin() + deltaPosition;

        GameObject go;
        go = Instantiate(highlightPrefab, spawnPos, Quaternion.Euler(0, 0, 0));
        go.transform.parent = currFloor.transform;

        stoneBehavior s = go.GetComponent<stoneBehavior>();
        if(stones[index] && s) stones[index] = s;
        s.index = index; s.myLevel = gameController.getCurrLevel(); s.note = 0;

        return go;
    }

    void mouseClick()
    {
        if (currSpawnedStone != null)
        {
            if (currSpawnedStone.GetComponent<stoneBehavior>().checkAssign())
            {
                currSpawnedStone = null;
                placeSFX.Play();
            }
            else
            {
                Destroy(currSpawnedStone);
                currSpawnedStone = null;
            }
        }else if (currMouseOver != null)
        {
            gameController.getCurrLevel().toggleNote(currMouseOver.index, currMouseOver.transform);
        }
    }


    void mouseRightClick()
    {
        if (currSpawnedStone != null)
        {
            cancelSFX.Play();
            Destroy(currSpawnedStone);
            currSpawnedStone = null;
        }else 

        if(currMouseOver != null)
        {
            currMouseOver.removeThisStone();
            cancelSFX.Play();
        }
    }
}
