using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimulatorManager : MonoBehaviour
{
    public static SimulatorManager Instance;
    
    public GameObject goEscapee;

    public List<Escapee> escapees = new List<Escapee>();
    public List<EscapeNode> escapeNodes = new List<EscapeNode>();

    public int escapeeNum = 0;

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

        escapeNodes[randomVal].IsOnFire = true;
    }

    public void AddEscapee(int addValue = 1)
    {
        if (addValue > 0)
        {
            for (int i = 0; i < addValue; i++)
            {
                GameObject newEscapee = Instantiate(goEscapee);

                escapees.Add(newEscapee.GetComponent<Escapee>());
                escapeeNum++;
            }
        }
        else if(addValue < 0)
        {
            
        }
    }

    public void SetEscapee(string param)
    {
        int targetVal = Int32.Parse(param);
        if (targetVal > escapeeNum)
        {
            for (int i = 0; i < targetVal - escapeeNum; i++)
            {
                AddEscapee();
            }
        }
        else if (targetVal < escapeeNum)
        {
            for (int i = 0; i < escapeeNum - targetVal; i++)
            {
                
            }
        }
    }
}
