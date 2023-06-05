using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum Direction
{
    East = 0,
    South = 1,
    West = 2,
    North = 3,
    Burned = 4
}

public class RouteNode : MonoBehaviour
{
    private Direction nodeDirection;

    private bool isOnFire = false;
    public bool IsOnFire
    {
        get { return isOnFire; }
        set
        {
            isOnFire = value;
            if (isOnFire)
            {
                nodeDirection = Direction.Burned;
            }
            FireParticle.SetActive(value);
        }
    }

    public float TransitionTime = 12.0f;
    private float curTime = 0;

    public List<RouteNode> connectedNode = new List<RouteNode>();

    public RouteNode FacingNode;

    public GameObject FireParticle;

    public Direction NodeDirection
    {
        get { return nodeDirection; }
        set
        {
            nodeDirection = value;
            transform.rotation = Quaternion.Euler(new Vector3(0, (int)nodeDirection * 90f, 0));
        }
    }

    private void OnEnable()
    {
        connectedNode.Clear();
    }

    public void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.DrawRay(transform.position + Vector3.up*1.5f, GetForwardVector((Direction)i) * 2f, Color.red);
        }
        
        if (!IsOnFire) return;

        curTime += Time.deltaTime;

        if (curTime > TransitionTime)
        {
            TransitFire();
        }
        
    }

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

    public void StartSimulation()
    {
        
    }

    public void OnDirectionChange(Direction InNodeDirection)
    {
        NodeDirection = InNodeDirection;
    }

    public void TransitFire()
    {
        if (connectedNode.Count <= 0) return;
        
        foreach (var node in connectedNode)
        {
            node.IsOnFire = true;
        }
    }
}
