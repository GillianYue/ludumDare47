using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometrySpawner : MonoBehaviour
{
    public GameObject startFloor, secondFloor;
    public GameObject floorPrefab;
    public Material[] floorMaterials;
    private Vector3 startOrigin; //floors will be spawned based on this position + dy * levelNum
    [HideInInspector] public float dy; //height of each floor; increment difference in height
    public int[] floorType; //stores the material index of this floor

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
    }


}
