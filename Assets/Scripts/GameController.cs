﻿using UnityEngine;
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
    public int currLevel, SkillLv; //instrumentLv indicates how many instruments have been unlocked
    [HideInInspector] public Vector3 playerStartPosOffset; //offset between player view (camera) spawn pos for each level and the floor spawn pos

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

/*    void OnGUI()
    {
        for (int i = 0; i < m_Levels.Length; i++)
        {
            int xIndex = (i) % (m_ButtonsPerRow);
            int yIndex = i / m_ButtonsPerRow;
            int xPos = Screen.width - m_ButtonsPerRow * m_ButtonSize + (xIndex) * m_ButtonSize;
            int yPos = yIndex * m_ButtonSize;

            int index = i;
            if (GUI.Button(new Rect(xPos, yPos, m_ButtonSize, m_ButtonSize), (index + 1).ToString()))
            {
                StartLevel(index);
            }
        }
    }*/

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(ascendFloor());
        }
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
}