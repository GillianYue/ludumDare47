using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometrySpawner : MonoBehaviour
{
    public GameObject startFloor, secondFloor, currFloor;
    public GameObject floorPrefab;
    public Material[] floorMaterials;
    private Vector3 startOrigin; //floors will be spawned based on this position + dy * levelNum
    [HideInInspector] public float dy; //height of each floor; increment difference in height
    public int[] floorType; //stores the material index of this floor

    public GameObject[] padCrystals, bassStones, drumCubes;
    public int[] crystalOffsetY, stoneOffsetY, cubeOffsetY; //int[7]s that indicate y offset for different notes - 0 for do, etc. 
    public GameController gameController;

    public GameObject geometryHolder;

    void Start()
    {
        startOrigin = startFloor.transform.position;
        dy = Mathf.Abs(secondFloor.transform.position.y - startFloor.transform.position.y);
        Debug.Log("dy is " + dy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 getStartOrigin()
    {
        return startOrigin;
    }

    public Vector3 getCameraLookat(int level, Vector3 camOffset)
    {
        return new Vector3(startOrigin.x + camOffset.x, startOrigin.y + level * dy + camOffset.y, 0);
    }

    public void spawnNewFloor(int floorIndex)
    {
        if (floorType[floorIndex] == -1) return;

        Material floorMat = floorMaterials[floorType[floorIndex]];
        GameObject f = Instantiate(floorPrefab, geometryHolder.transform);
        f.transform.GetChild(0).GetComponent<MeshRenderer>().material = floorMat;

        f.transform.position = startOrigin + new Vector3(0, floorIndex * dy, 0);
        currFloor = f;
    }

    public void spawnStone(InSceneLevel.levelType type, Vector3 deltaPosition, Vector3 rotation, int note)
    {
        StartCoroutine(spawnStoneCoroutine(type, deltaPosition, rotation, note));
    }


        private IEnumerator spawnStoneCoroutine(InSceneLevel.levelType type, Vector3 deltaPosition, Vector3 rotation, int note)
    {
        Vector3 floorOrigin = startOrigin + new Vector3(0, gameController.currLevel * dy, 0), //starting point
            spawnPos = floorOrigin + deltaPosition, deltaUp = new Vector3(0, 50, 0);
        GameObject go = null;

        switch (type)
        {
            case InSceneLevel.levelType.DRUM:
                spawnPos += new Vector3(0, cubeOffsetY[note], 0);
                go = Instantiate(drumCubes[Random.Range(0, drumCubes.Length)], spawnPos + deltaUp, Quaternion.Euler(rotation));
                break;
            case InSceneLevel.levelType.PAD:
                spawnPos += new Vector3(0, crystalOffsetY[note], 0);
                go = Instantiate(padCrystals[Random.Range(0, padCrystals.Length)], spawnPos + deltaUp, Quaternion.Euler(rotation));
                
                break;
            case InSceneLevel.levelType.BASS:
                spawnPos += new Vector3(0, stoneOffsetY[note], 0);
                go = Instantiate(bassStones[Random.Range(0, bassStones.Length)], spawnPos + deltaUp, Quaternion.Euler(rotation));
                break;
        }

        go.transform.parent = currFloor.transform;

        bool[] don = new bool[1];
        StartCoroutine(Global.moveToInSecs(go, spawnPos, 2, don));
        yield return new WaitUntil(() => don[0]);
    }


}
