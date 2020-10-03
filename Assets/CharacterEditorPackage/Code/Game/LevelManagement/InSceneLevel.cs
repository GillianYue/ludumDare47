using UnityEngine;
using System.Collections;
//--------------------------------------------------------------------
//Attach to objects to be used by InSceneLevelSwitcher as starting points
//--------------------------------------------------------------------
[System.Serializable]
public class InSceneLevel : MonoBehaviour {

    public enum levelType { DRUM, PAD, BASS };
    public levelType type;

    public string puzzleSource;
    private int[] puzzle;
    public int dx, dxNoise, startDistance;

    public GeometrySpawner geometrySpawner;


    void Start()
    {
        puzzle = new int[puzzleSource.Length];

        char[] ca = puzzleSource.ToCharArray();
        for (int c = 0; c<puzzleSource.Length; c++)
        {
            int.TryParse(puzzleSource.Substring(c, 1), out puzzle[c]);
        }

    }

    public void setupLevel()
    {
        StartCoroutine(putPuzzlesInPlace());
    }

    private IEnumerator putPuzzlesInPlace()
    {
        Vector3 start = geometrySpawner.getStartOrigin() + new Vector3(startDistance, 0, 0);

        for(int a=0; a<16; a++)
        {
            Vector3 currX = new Vector3(start.x + a * dx + Random.Range(0, dxNoise), start.y, start.z);
            geometrySpawner.spawnStone(type, currX, new Vector3(0, 0, Random.Range(-20, 20)), puzzle[a]);
        }


        yield return new WaitForSeconds(0.1f);

    }



}
