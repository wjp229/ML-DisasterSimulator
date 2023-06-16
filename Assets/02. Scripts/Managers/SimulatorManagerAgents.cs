using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class SimulatorManagerAgents : Agent
{
    
    //노드의 방향 변수 정의
    [SerializeField] private Direction nodeDirection;

    // Initial direction
    public Direction initDirection;
    
    public override void Initialize()
    {
        
        
        // 초기 방향 저장
        
    }
    
    public override void OnActionReceived(float[] vectorAction)
    {
        
        if (Mathf.FloorToInt(vectorAction[0]) == 1) //분기가 1밖에 없기 때문
            ChangeNodeDirection();
    }

    public override void OnEpisodeBegin()
    {
        //환경 상태 초기화
        
        // foreach (List<RouteNode> routeNode in SimulatorManager.Instance.escapeNodes)
        // {
        //     
        // }
            nodeDirection = initDirection;
        //
        SimulatorManager.Instance.StartSimulation();
    }

    // 노드 방향 전환 Method
    public Direction ChangeNodeDirection
    {
        get { return nodeDirection; }
        set
        {
            nodeDirection = value;
            transform.rotation = Quaternion.Euler(new Vector3(0, (int) nodeDirection * 90f, 0));
        }
    }
}
