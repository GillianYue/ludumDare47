using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    public static IEnumerator moveToInSecs(GameObject e, int x, int y, float sec, bool[] done)
    {
        Debug.Log("moving something");
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

    public static void rotateAnim(GameObject e, MonoBehaviour m) { m.StartCoroutine(rotateAnimIE(e));  }
    public static void horizShakeAnim(GameObject e, MonoBehaviour m) { m.StartCoroutine(horizShakeAnimIE(e));  }

    private static IEnumerator rotateAnimIE(GameObject e)
    {
        Transform t = e.transform;
        Vector3 origRotation = t.rotation.eulerAngles;


        for (int l = 0; l < 2; l++)
        {
            t.rotation = Quaternion.Euler(origRotation + new Vector3(0, 0, -5));
            yield return new WaitForSeconds(0.03f);
            t.rotation = Quaternion.Euler(origRotation);
        }
    }

    private static IEnumerator horizShakeAnimIE(GameObject e)
    {
        Transform t = e.transform;
        Vector3 origPosition = t.position;

        for (int l = 0; l < 2; l++)
        {
            t.position += new Vector3(20, 0, 0);
            yield return new WaitForSeconds(0.03f);
            t.position = origPosition;
        }
    }

    public static IEnumerator brieflyChangeColor(Color orig, Color newCol, SpriteRenderer s, float waitTime)
    {
        s.color = newCol;
        yield return new WaitForSeconds(waitTime);

        s.color = orig;
    }
}
