using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SimulatorManager : MonoBehaviour
{
    public static SimulatorManager Instance;

    public GameObject goEscapee;

    public List<Escapee> escapees = new List<Escapee>();
    public List<EscapeNode> escapeNodes = new List<EscapeNode>();

    public int escapeeNum = 0;

    public TMP_InputField escapeeText;

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

    public void StartSimulation()
    {
        int randomVal = Random.Range(0, escapeNodes.Count);

        foreach (var node in escapeNodes)
        {
            node.gameObject.SetActive(true);
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
                
                if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
                {
                    Debug.Log("H" + hit.position);
                    finalPosition = hit.position;
                }
                
                Debug.Log(finalPosition);

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
}