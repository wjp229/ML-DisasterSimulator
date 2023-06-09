using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public struct ResultInfo
{
    public int InitEscapeeCnt;
    
    public float InitEscapeTime;
    public float AvgEscapeTime;
    public float TotalEscapeTime;
    public float LastEscapeTime;

    public int DeathCnt;
    public int EscapedCnt;
    public float DeathRate;
}


public class SimulatorManager : MonoBehaviour
{
    public static SimulatorManager Instance;

    public GameObject goEscapee;

    public List<Escapee> escapees = new List<Escapee>();
    public List<RouteNode> escapeNodes = new List<RouteNode>();

    public List<ExitNode> exitNodes = new List<ExitNode>();

    public int escapeeNum = 0;

    public TMP_InputField escapeeText;

    public Camera MainCam;

    private ResultInfo _resultInfo;

    private bool _simulationStarted = false;
    public float SimulationTime = 0f;

    public ResultUI resultUI;

    public TextMeshProUGUI TimeText;

    public bool IsSimulatorActive = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (!IsSimulatorActive)
            return;
        
        if (_simulationStarted)
        {
            SimulationTime += Time.deltaTime;
            TimeText.text = "Cur Time : " + SimulationTime.ToString("F1");
        }
    }

    public void StartSimulation()
    {
        int randomVal = Random.Range(0, escapeNodes.Count);

        _simulationStarted = true;

        _resultInfo.InitEscapeeCnt = escapees.Count;
        _resultInfo.DeathCnt = 0;
        _resultInfo.AvgEscapeTime = 0;

        foreach (var node in escapeNodes)
        {
            node.gameObject.SetActive(true);
        }
        
        foreach (var node in exitNodes)
        {
            node.gameObject.SetActive(true);
        }
        
        foreach (var node in escapeNodes)
        {
            node.SetConnectedNode();
        }

        escapeNodes[randomVal].IsOnFire = true;
        foreach (var escapee in escapees)
        {
            escapee._state = EscapeeState.Following;
        }
    }

    public void AddEscapee(int addValue = 1)
    {
        if (addValue > 0)
        {
            for (int i = 0; i < addValue; i++)
            {
                float radius = 30f;

                NavMeshHit hit;
                Vector3 finalPosition = Vector3.zero;

                Vector3 randomPoint = Random.insideUnitSphere * radius;

                if (NavMesh.SamplePosition(randomPoint, out hit, 15.0f, NavMesh.AllAreas))
                {
                    finalPosition = hit.position;
                }
                
                GameObject newEscapee =
                    Instantiate(goEscapee, finalPosition, Quaternion.identity);

                escapees.Add(newEscapee.GetComponent<Escapee>());
                escapeeNum++;
            }
        }
        else if (addValue < 0)
        {
            if (escapeeNum <= 0) return;

            Escapee removeEscapee = escapees[escapees.Count - 1];
            escapees.Remove(removeEscapee);
            DestroyImmediate(removeEscapee.gameObject);

            escapeeNum--;
        }

        escapeeText.text = escapeeNum.ToString();
    }

    public void SetEscapee(string param)
    {
        int targetVal = Int32.Parse(param);
        if (targetVal > escapeeNum)
        {
            int finalVal = targetVal - escapeeNum;
            for (int i = 0; i < finalVal; i++)
            {
                AddEscapee();
            }
        }
        else if (targetVal < escapeeNum)
        {
            int finalVal = escapeeNum - targetVal;
            for (int i = 0; i < finalVal; i++)
            {
                AddEscapee(-1);
            }
        }
    }

    public void EscapeeEscaped(Escapee escapee)
    {
        escapees.Remove(escapee);

        _resultInfo.EscapedCnt += 1;
        if (_resultInfo.EscapedCnt == 1)
        {
            _resultInfo.InitEscapeTime = SimulationTime;
        }

        _resultInfo.TotalEscapeTime += SimulationTime;
        
        _resultInfo.LastEscapeTime = SimulationTime;
        _resultInfo.AvgEscapeTime = (_resultInfo.TotalEscapeTime / _resultInfo.EscapedCnt);
        // Info Shard

        
        CheckSimulatorOver();
    }

    public void SetNodeDirection(int inNodeNum, Direction inNodeDirection)
    {
        escapeNodes[inNodeNum].NodeDirection = inNodeDirection;
    }

    public Vector3[] GetEscaepeeInfo()
    {
        Vector3[] escapeeInfo = new Vector3[escapees.Count];

        for (int i = 0; i < escapees.Count; i++)
        {
            escapeeInfo[i] = escapees[i].transform.position;
        }

        return escapeeInfo;
    }

    public int[] GetNodeStateInfo()
    {
        int[] nodeInfo = new int[escapeNodes.Count];

        for (int i = 0; i < escapeNodes.Count; i++)
        {
            nodeInfo[i] = (int)(escapeNodes[i].NodeDirection);
        }
        
        return nodeInfo;
    }

    public void EscapeeDead(Escapee escapee)
    {
        escapees.Remove(escapee);

        _resultInfo.DeathCnt += 1;
        _resultInfo.DeathRate = ((float)_resultInfo.DeathCnt / (float)_resultInfo.InitEscapeeCnt);
        // Info Share
        
        CheckSimulatorOver();
    }

    public void CheckSimulatorOver()
    {
        if (!IsSimulatorActive)
            return;
        
        if (escapees.Count <= 0)
        {
            resultUI.gameObject.SetActive(true);
            resultUI.SetResult(_resultInfo);

            IsSimulatorActive = false;
        }
    }
}