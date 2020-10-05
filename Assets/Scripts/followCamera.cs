using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public GameObject player;
    private float helpCameraFollowFaster = 2;
    public Vector3 delta;

    void FixedUpdate()
    {
        this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, player.transform.position.x + delta.x, Time.deltaTime * helpCameraFollowFaster),
                                               Mathf.Lerp(this.transform.position.y, player.transform.position.y + delta.y, Time.deltaTime * helpCameraFollowFaster), 0);
    }
}
