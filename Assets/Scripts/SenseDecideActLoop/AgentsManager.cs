using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsManager : MonoBehaviour {

    [Header("References")]
    [SerializeField] public GameObject agentPrefab = null;

    [Header("Frequencies")]
    [Space(10)]
    [SerializeField] public float senseUpdateFrequency = 0.2f;
    [SerializeField] public float decideUpdateFrequency = 0.5f;
    [SerializeField] public float actUpdateFrequency = 0.2f;

    [Header("Cursors")]
    [Space(10)]
    [SerializeField] public uint senseCursorRate = 2;
    [SerializeField] public uint decideCursorRate = 3;
    [SerializeField] public uint actCursorRate = 5;

    [Header("Specifications")]
    [Space(10)]
    [SerializeField] public uint poolSize = 10;
    [SerializeField] public new string name = "Default";


    private List<Agent> agents;

    //Cursors
    private uint senseCursor = 0;
    private uint decideCursor = 0;
    private uint actCursor = 0;

    //Timers
    private float senseUpdateTimer = 0.0f;
    private float decideUpdateTimer = 0.0f;
    private float actUpdateTimer = 0.0f;


    void Start()
    {
        gameObject.name = "AgentsPool: " + name;
        CreatePool();
    }
    void Update()
    {
        UpdateTimers();
        UpdateSense();
        UpdateDecide();
        UpdateAct();
    }

    private void CreatePool()
    {
        if (!agentPrefab)
        {
            Debug.LogError("Agent prefab was not set in " + name);
            return;
        }

        agents = new List<Agent>((int)poolSize);
        for (uint index = 0; index < poolSize; index++)
        {
            GameObject obj = Object.Instantiate(agentPrefab);
            obj.transform.parent = transform;
            obj.transform.localScale = Vector3.one;
            Agent script = obj.GetComponent<Agent>();
            script.Init(this);
            agents.Add(script);
        }
    }
    private void UpdateTimers()
    {
        if (senseUpdateTimer > 0.0f)
        {
            senseUpdateTimer -= Time.deltaTime;
            if (senseUpdateTimer <= 0.0f)
                senseUpdateTimer = 0.0f;
        }
        if (decideUpdateTimer > 0.0f)
        {
            decideUpdateTimer -= Time.deltaTime;
            if (decideUpdateTimer <= 0.0f)
                decideUpdateTimer = 0.0f; ;
        }
        if (actUpdateTimer > 0.0f)
        {
            actUpdateTimer -= Time.deltaTime;
            if (actUpdateTimer <= 0.0f)
                actUpdateTimer = 0.0f;
        }
    }


    private void UpdateSense()
    {
        if (senseUpdateTimer > 0.0f)
            return;

        senseUpdateTimer = senseUpdateFrequency;

        int counter = 0;
        int poolSizeCounter = 0;

        while (counter != (int)senseCursorRate)
        {
            if (agents[(int)senseCursor].GetState()) {
                agents[(int)senseCursor].Sense();
                counter++;
            }

            senseCursor++;
            if (senseCursor == agents.Count)
                senseCursor = 0;

            poolSizeCounter++;
            if (poolSizeCounter >= agents.Count)
                break;
        }
    }
    private void UpdateDecide()
    {
        if (decideUpdateTimer > 0.0f)
            return;

        decideUpdateTimer = decideUpdateFrequency;

        int counter = 0;
        int poolSizeCounter = 0;

        while (counter != (int)decideCursorRate) {
            if (agents[(int)decideCursor].GetState()) {
                agents[(int)decideCursor].Decide();
                counter++;
            }

            decideCursor++;
            if (decideCursor == agents.Count)
                decideCursor = 0;

            poolSizeCounter++;
            if (poolSizeCounter >= agents.Count)
                break;
        }
    }
    private void UpdateAct()
    {
        if (actUpdateTimer > 0.0f)
            return;

        actUpdateTimer = actUpdateFrequency;

        int counter = 0;
        int poolSizeCounter = 0;

        while (counter != (int)actCursorRate)
        {
            if (agents[(int)actCursor].GetState()) {
                agents[(int)actCursor].Act();
                counter++;
            }

            actCursor++;
            if (actCursor == agents.Count)
                actCursor = 0;

            poolSizeCounter++;
            if (poolSizeCounter >= agents.Count)
                break;
        }
    }

    public Agent GetValidEnemy()
    {
        if (agents.Count <= 0.0f)
            return null;

        for (uint index = 0; index < agents.Count; index++)
        {
            if (!agents[(int)index].GetState())
                return agents[(int)index];
        }

        Debug.LogWarning(name + " manager was unable to return valid agent!");
        return null;
    }
}
