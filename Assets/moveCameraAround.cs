using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCameraAround : MonoBehaviour
{
    public GameObject cam;
    public float xMin, xMax;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveCamera(int amount)
    {

        Vector3 dest = cam.transform.position + new Vector3(amount, 0, 0);
        if (dest.x <= xMin) dest = new Vector3(xMin, dest.y, dest.z);
        else if (dest.x >= xMax) dest = new Vector3(xMax, dest.y, dest.z);

        StartCoroutine(Global.moveToInSecs(cam, dest, 0.5f, new bool[1]));
    }
}
