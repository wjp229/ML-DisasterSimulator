using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class SimulatorAgent : Agent
{
    public bool IsLearningMode;
    public SimulatorManager simulatorManager;

    public override void Initialize()
    {
    }

    public override void OnEpisodeBegin()
    {      
        if (IsLearningMode)
        {
            simulatorManager.StartSimulation(true);
        }
        
    }

    // 관측 정보 전달
    public override void CollectObservations(VectorSensor sensor)
    {
        for (int i = 0; i < 10; i++)
        {
            if (simulatorManager.escapees[i] == null)
            {
                sensor.AddObservation(-1);
                sensor.AddObservation(-1);
                
                continue;
            }
            
            sensor.AddObservation(simulatorManager.escapees[i].transform.localPosition.x);
            sensor.AddObservation(simulatorManager.escapees[i].transform.localPosition.z);
        }
        
        int NodeCnt = simulatorManager.escapeNodes.Count;
        for (int i = 0; i < 60; i++)
        {
            sensor.AddObservation(simulatorManager.escapeNodes[i].IsOnFire);
        }

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int NodeNum = actions.DiscreteActions[0];
        int NodeDirection = actions.DiscreteActions[1];

        if ((RNDirection)NodeDirection == RNDirection.Burned)
            return;

        simulatorManager.SetNodeDirection(NodeNum, (RNDirection)NodeDirection);
    }

    void SetResetParameters()
    {
    }
}