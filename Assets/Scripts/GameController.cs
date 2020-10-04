using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    //Level start event (for other scripts to use when the level is changed)
    public delegate void OnLevelStartEvent();
    public static event OnLevelStartEvent OnLevelStart;
    //[SerializeField] CharacterControllerBase m_Character = null;
    [SerializeField] InSceneLevel[] m_Levels = null;
    [SerializeField] int m_ButtonSize = 0;
    [SerializeField] int m_ButtonsPerRow = 0;

    static GameController g_gameController;
    public int currentBeat; public Text beatText;
    public static GameController Get()
    {
        if (g_gameController == null)
        {
            g_gameController = FindObjectOfType<GameController>();
            if (g_gameController == null)
            {
                return null;
            }
        }
        return g_gameController;
    }

    [SerializeField] public Transform m_Camera = null;
    public GeometrySpawner geometrySpawn;
    public int currLevel, instrLv; //instrumentLv indicates how many instruments have been unlocked
    [HideInInspector] public Vector3 playerStartPosOffset; //offset between player view (camera) spawn pos for each level and the floor spawn pos
    public AudioSource[] instruments;

    //indices for instruments: 

    void Start()
    {
        playerStartPosOffset = m_Camera.transform.position - geometrySpawn.getStartOrigin(); //assumes camera on start is in its place

        StartCoroutine(initialize());
    }

    IEnumerator initialize()
    {
        bool[] done = new bool[1];
        StartCoroutine(m_Levels[currLevel].setupLevel(done));
        yield return new WaitUntil(() => done[0]);
        StartLevel(currLevel);
    }

    public InSceneLevel getCurrLevel()
    {
        return m_Levels[currLevel];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(ascendFloor());
        }

        beatText.text = currentBeat.ToString();
    }

    public void SetIndex(int a_Index)
    {
        currLevel = a_Index;
    }

    public void Respawn()
    {
        StartLevel(currLevel);

    }

    public IEnumerator ascendFloor()
    {
        currLevel++;
        geometrySpawn.spawnNewFloor(currLevel);
        bool[] done = new bool[1];
        StartCoroutine(m_Levels[currLevel].setupLevel(done));
        yield return new WaitUntil(() => done[0]);
        StartLevel(currLevel);
    }

    public void CorrectCamera()
    {
        Vector3 diff = geometrySpawn.getCameraLookat(currLevel, playerStartPosOffset) - m_Camera.transform.position;
        diff.z = 0;
        StartCoroutine(Global.moveToInSecs(m_Camera.gameObject, m_Camera.transform.position + diff, 1, new bool[1]));
    }
    void StartLevel(int a_Index)
    {
        if (a_Index >= m_Levels.Length)
        {
            return;
        }

        currLevel = a_Index;
        CorrectCamera();
        if (OnLevelStart != null)
        {
            OnLevelStart();
        }
    }

    public void playCurrentLoop()
    {
        for (int i = 0; i<=instrLv; i++)
        {
            
            instruments[i].Play();
        }

        StartCoroutine(beatCounter());
    }

    public IEnumerator startBeatCounterAfterFirstLoop()
    {
        yield return new WaitForSeconds(6.4f);
        StartCoroutine(beatCounter());
    }

    public IEnumerator beatCounter()
    {
        currentBeat = 0; int count = 0;

        while (true) {

            currentBeat += 1;
            if (count > 16) m_Levels[currLevel].currentBeatStoneAnim(currentBeat-1);
            yield return new WaitForSeconds(0.4f);


            if (currentBeat == 16) currentBeat = 0;
            count ++;
                }
    }
}