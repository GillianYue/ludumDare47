using UnityEngine;
using System.Collections;
//--------------------------------------------------------------------
//Attach to objects to be used by InSceneLevelSwitcher as starting points
//--------------------------------------------------------------------
[System.Serializable]
public class InSceneLevel : MonoBehaviour {

    public enum levelType { DRUM, PAD, BASS, GUITAR };
    public levelType type;

    public string puzzleSource, puzzleSpawnSource;
    private int[] puzzle, puzzleSpawnAssignment; //spawn assignment is data on what stuff should be spawned on start of the level
    public int[] currAssignment;
    public int dx, dxNoise, startDistance;

    public float stoneSpawnDeltaTime;

    public GeometrySpawner geometrySpawner;


    void Start()
    {
        puzzle = new int[puzzleSource.Length];
        puzzleSpawnAssignment = new int[puzzleSpawnSource.Length];

        char[] ca = puzzleSource.ToCharArray();
        for (int c = 0; c<puzzleSource.Length; c++)
        {
            int.TryParse(puzzleSource.Substring(c, 1), out puzzle[c]);
            int.TryParse(puzzleSpawnSource.Substring(c, 1), out puzzleSpawnAssignment[c]);
        }

    }

    public IEnumerator setupLevel(bool[] done)
    {
        bool[] inPlace = new bool[1];
        StartCoroutine(putPuzzlesInPlace(inPlace));

        yield return new WaitUntil(() => inPlace[0]);
        done[0] = true;
    }

    private IEnumerator putPuzzlesInPlace(bool[] inPlace)
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 startDelta = geometrySpawner.getStartOrigin() + new Vector3(startDistance, 0, 0); //we only need the x here

        GameObject cam = geometrySpawner.getCameraTransform().gameObject;
        StartCoroutine(waitThenMoveCam(cam, 0.8f, new Vector3(startDelta.x + 16 * dx, geometrySpawner.getCameraLookat().y, startDelta.z), 16 * stoneSpawnDeltaTime));

        for(int a=0; a<16; a++)
        {
            Vector3 currXDelta = new Vector3(startDelta.x + a * dx + Random.Range(0, dxNoise), 0, 0);
            geometrySpawner.spawnStone(a, type, currXDelta, type!= levelType.BASS? new Vector3(0, 0, Random.Range(-geometrySpawner.angleNoise, geometrySpawner.angleNoise)) : 
                Vector3.zero, puzzleSpawnAssignment[a]);
            yield return new WaitForSeconds(stoneSpawnDeltaTime);
        }
        currAssignment = puzzleSpawnAssignment;

        yield return new WaitForSeconds(1.5f);
        inPlace[0] = true;
    }

    private IEnumerator waitThenMoveCam(GameObject cam, float sec, Vector3 dest, float moveTime)
    {
        yield return new WaitForSeconds(sec);
        StartCoroutine(Global.moveToInSecs(cam, dest, moveTime, new bool[1]));
    }

    public void setStoneAssignment(int spot, int assignment, Transform stoneTransform)
    {
        currAssignment[spot] = assignment;
        checkForSingleStone(spot); //see if this assignment is "correct"
        //TODO reassign height based on assignment note

        Vector3 delta = new Vector3(0, geometrySpawner.notesOffsetY[assignment], 0);
        stoneTransform.position = stoneTransform.position + delta;
    }

    public bool checkForLevelPass()
    {
        for(int a = 0; a<16; a++)
        {
            if (currAssignment[a] != puzzle[a]) return false;
        }

        return true;
    }

    public bool checkForSingleStone(int index)
    {
        if (currAssignment[index] == puzzle[index])
        {
            //vfx TODO
            puzzleSpawnAssignment[index] = 0; //can no longer modify this stone
            return true;
        }
        else return false;
    }

    public bool canModifyStone(int index)
    {
        return (puzzleSpawnAssignment[index] == 0);
    }

}
