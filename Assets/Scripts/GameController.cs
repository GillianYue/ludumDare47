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
    public Animator beatAnimator, endingA;
    private IEnumerator currentBeatCount;

    bool inTitleScreen = true;


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
    public InstrumentManager instrumentIcons;
    public GameObject titlePanel, leftMoveArrow, rightMoveArrow;

    //indices for instruments: 

    void Start()
    {
        playerStartPosOffset = m_Camera.transform.position - geometrySpawn.getStartOrigin(); //assumes camera on start is in its place

        StartCoroutine(initialize());
        endingA.gameObject.SetActive(false);
    }

    IEnumerator initialize()
    {

        yield return new WaitUntil(() => !inTitleScreen);
        titlePanel.SetActive(false);

        yield return StartCoroutine(m_Levels[currLevel].setupLevel());
        StartLevel(currLevel);
    }

    public void titleScreenStartGame()
    {
        inTitleScreen = false;
    }

    public void titleScreenExitGame()
    {
        Application.Quit();
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
        currLevel++; instrLv++;
        if (currLevel != 4)
        {
            instrumentIcons.updateInstrumentIcon(currLevel);
            yield return StartCoroutine(geometrySpawn.spawnNewFloor(currLevel));
        }
        else
        {
            StartCoroutine(ending());
        }
    }

    IEnumerator ending()
    {
        StopCoroutine(currentBeatCount);
        geometrySpawn.correctNumberText.text = "";
        beatText.gameObject.SetActive(false);
        instrumentIcons.disableAll();
        leftMoveArrow.SetActive(false);
        rightMoveArrow.SetActive(false);
        yield return StartCoroutine(CorrectCamera());
        yield return new WaitForSeconds(1);
        endingA.gameObject.SetActive(true);
    }

    public void setUpCurrLevel()
    {
        StartCoroutine(setupCurrLvIE());
    }

    private IEnumerator setupCurrLvIE()
    {
        yield return StartCoroutine(m_Levels[currLevel].setupLevel());
        StartLevel(currLevel);
    }

    public IEnumerator CorrectCamera()
    {
        
        Vector3 diff = geometrySpawn.getCameraLookat(currLevel, playerStartPosOffset) - m_Camera.transform.position;
        diff.z = 0;
        //Debug.Log("correct camera " + m_Camera.transform.position + diff);
        yield return StartCoroutine(Global.moveToInSecs(m_Camera.gameObject, m_Camera.transform.position + diff, 1));
    }
    void StartLevel(int a_Index)
    {
        if (a_Index >= m_Levels.Length)
        {
            return;
        }

        currLevel = a_Index;
        StartCoroutine(CorrectCamera());
        if (OnLevelStart != null)
        {
            OnLevelStart();
        }
    }

    public IEnumerator playCurrentLoop()
    {
        if (instrLv > 1)
        {
            yield return new WaitForSeconds(0.001f);
            StopCoroutine(currentBeatCount);
        }
            for (int i = 0; i <= instrLv; i++)
            {

                instruments[i].Play();
            }

            currentBeatCount = beatCounter();
            StartCoroutine(currentBeatCount);
    }

    public IEnumerator beatCounter()
    {
        currentBeat = 0; int count = 0; float startTime = Time.time;

        while (true) {

            currentBeat += 1; count++;
            beatAnimator.SetTrigger("a");
            if (count > 16) m_Levels[currLevel].currentBeatStoneAnim(currentBeat-1);
            yield return new WaitUntil(() => (Time.time >= startTime + count * 0.4f));


            if (currentBeat == 16) currentBeat = 0;
            
                }
    }
}