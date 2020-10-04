using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTestBehavior : MonoBehaviour
{

    public int speedUpRate;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown("space")) //space for speeding time up
            {
                Time.timeScale = 1.0f * speedUpRate;
            }

            if (Input.GetKeyUp("space")) //release to go back to normal time
            {
                Time.timeScale = 1.0f;
            }



    }

    public static void pauseGame()
    {
        Time.timeScale = 0;
    }

    public static void resumeGame()
    {
        Time.timeScale = 1;
    }
}
