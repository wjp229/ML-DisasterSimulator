using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

// Node의 상태 정의
public enum Direction
{
    East = 0,
    South = 1,
    West = 2,
    North = 3,
    Burned = 4
}

public class RouteNode : Agent
{
    //노드의 방향 변수 정의
    [SerializeField] private Direction nodeDirection;

    public override void Initialize()
    {
        //
    }
    
    // public override void OnActionReceived(float[] vectorAction)
    // {
    //     if (Mathf.FloorToInt(vectorAction[0]) == 1) //분기가 1밖에 없기 때문
    //         ChangeNodeDirection();
    // }

    public override void OnEpisodeBegin()
    {
        //환경 상태 초기화
        
    }
    
    // 노드 방향 전환 Method
    public Direction ChangeNodeDirection
    {
        get { return nodeDirection; }
        set
        {
            nodeDirection = value;
            transform.rotation = Quaternion.Euler(new Vector3(0, (int)nodeDirection * 90f, 0));
        }
    }

    private void FixedUpdate()
    {
        //노드에 불이 나지 않았을 때만 Action 결정 요청
        if(!isOnFire)
            RequestDecision();
    }

    // 매 Frame 마다 화재 아닐 때는 사방에 Raycast 쏘기 및 화재일 때 쿨타임 차면 화재 전이
    public void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.DrawRay(transform.position + Vector3.up*1.5f, GetForwardVector((Direction)i) * 2f, Color.red);
        }
        
        if (!IsOnFire) return;

        curTime += Time.deltaTime;

        // 쿨타임 차면 화재 전이
        if (curTime > TransitionTime)
        {
            TransitFire();
        }
        
    }
    
    //주변 노드 정의하기 위한 변수 정의
    public List<RouteNode> connectedNode = new List<RouteNode>();
    //이 노드가 바라보고 있는 방향의 노드 변수
    public RouteNode FacingNode;
    
    //시뮬레이션 Start 시 주변 노드 초기화
    private void OnEnable()
    {
        connectedNode.Clear();
    }
    
    //Raycast 쏴서 주변 노드의 위치 값 가져오기
    public Vector3 GetTargetDestination()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up*1.5f, transform.forward);
        
        if (Physics.Raycast(transform.position + Vector3.up*1.5f, transform.forward, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Node"))
            {
                return hit.transform.position;
            }
        }

        return Vector3.zero;
    }
    
    //네 방향 주변 노드 추가
    public void SetConnectedNode()
    {
        for (int i = 0; i < 4; i++)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position + Vector3.up*1.5f, GetForwardVector((Direction)i));

            if (Physics.Raycast(transform.position + Vector3.up*1.5f, GetForwardVector((Direction)i), out hit))
            {
                if (hit.transform.GetComponent<RouteNode>())
                {
                    connectedNode.Add(hit.transform.GetComponent<RouteNode>());
                }
            }
        }
    }
    
    //바라보고 있는 방향 얻기
    Vector3 GetForwardVector(Direction directionValue)
    {
        switch (directionValue)
        {
            case Direction.East:
                return Vector3.forward;
            case Direction.South:
                return Vector3.right;
            case Direction.West:
                return Vector3.back;
            case Direction.North:
                return Vector3.left;
            default:
                throw new ArgumentOutOfRangeException(nameof(directionValue), directionValue, null);
        }
    }
    
    // public void StartSimulation()
    // {
    //     
    // }
    
    // // ??
    // public void OnDirectionChange(Direction InNodeDirection)
    // {
    //     NodeDirection = InNodeDirection;
    // }
    
    // 화재 파티클
    public GameObject FireParticle;
    //불 났는 지 여부 변수 정의
    private bool isOnFire = false;
    //화재 전이주기 정의
    public float TransitionTime = 12.0f;
    private float curTime = 0;
    
    // 불 났는지 여부에 따라 노드 상태 변경 및 파티클 활성화
    public bool IsOnFire
    {
        // Get
        get { return isOnFire; }
        
        // Set
        set
        {
            isOnFire = value;
            if (isOnFire) // isOnFire==True
            {
                // 노드의 상태를 Burned로 변경
                nodeDirection = Direction.Burned;
            }
            //화재 파티클 활성화
            FireParticle.SetActive(value);
        }
    }
    // 화재 전이
    public void TransitFire()
    {
        if (connectedNode.Count <= 0) return;
        
        foreach (var node in connectedNode)
        {
            node.IsOnFire = true;
        }
    }
}
