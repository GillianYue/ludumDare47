﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    public static IEnumerator moveToInSecs(GameObject e, int x, int y, float sec, bool[] done)
    {
        float xDist = x - e.transform.position.x;
        float yDist = y - e.transform.position.y;
        float dx = xDist / sec;
        float dy = yDist / sec;

        e.GetComponent<Rigidbody2D>().velocity = new Vector2(dx, dy);

        yield return new WaitForSeconds(sec);

        e.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0); //stops the GO at dest
        done[0] = true;
    }

    public static IEnumerator moveToInSecs(GameObject e, Vector2 dest, float sec, bool[] done)
    {
        return moveToInSecs(e, (int)dest.x, (int)dest.y, sec, done);
    }
}