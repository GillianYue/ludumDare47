using UnityEngine;
using System.Collections;
//--------------------------------------------------------------------
//Attach to objects to be used by InSceneLevelSwitcher as starting points
//--------------------------------------------------------------------
[System.Serializable]
public class InSceneLevel : MonoBehaviour {

    public Transform m_StartPoint;
    public enum levelType { DRUM, PAD, BASS };
    public levelType type;

    public int[] puzzle;
    public int dx;

    public void setupLevel()
    {

    }

    private IEnumerator putPuzzlesInPlace()
    {
        switch (type)
        {
            case levelType.DRUM:
                break;
            case levelType.PAD:
                break;
            case levelType.BASS:
                break;
        }


        yield return new WaitForSeconds(0.1f);

    }






    void OnDrawGizmos()
    {
        if (m_StartPoint != null)
        {
            Gizmos.color = Color.Lerp(Color.red, Color.white, 0.7f);
            Gizmos.DrawWireSphere(m_StartPoint.position, 0.5f);
        }
    }
}
