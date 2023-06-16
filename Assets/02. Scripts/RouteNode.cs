using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;

public enum RNDirection
{
    East = 0,
    South = 1,
    West = 2,
    North = 3,
    Burned = 4
}

public class RouteNode : MonoBehaviour
{
    private RNDirection nodeDirection;

    private bool isOnFire = false;
    public bool IsOnFire
    {
        get { return isOnFire; }
        set
        {
            isOnFire = value;
            if (isOnFire)
            {
                nodeDirection = RNDirection.Burned;
            }
            FireParticle.SetActive(value);
            if(value) FireParticle.GetComponent<ParticleSystem>().Play();
        }
    }

    public float TransitionTime = 12.0f;
    public float curTime = 0;

    public List<RouteNode> connectedNode = new List<RouteNode>();

    public RouteNode FacingNode;

    public GameObject FireParticle;

    public RNDirection NodeDirection
    {
        get { return nodeDirection; }
        set
        {
            nodeDirection = value;
            transform.rotation = Quaternion.Euler(new Vector3(0, (int)nodeDirection * 90f, 0));
        }
    }

    public void ResetNodeState()
    {
        IsOnFire = false;
        nodeDirection = RNDirection.East;
        FireParticle.SetActive(false);
        curTime = 0;
    }

    private void OnEnable()
    {
        connectedNode.Clear();
    }

    public void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.DrawRay(transform.position + Vector3.up*1.5f, GetForwardVector((RNDirection)i) * 2f, Color.red);
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
            Ray ray = new Ray(transform.position + Vector3.up*1.5f, GetForwardVector((RNDirection)i));

            if (Physics.Raycast(transform.position + Vector3.up*1.5f, GetForwardVector((RNDirection)i), out hit))
            {
                if (hit.transform.GetComponent<RouteNode>())
                {
                    connectedNode.Add(hit.transform.GetComponent<RouteNode>());
                }
            }
        }
    }

    Vector3 GetForwardVector(RNDirection directionValue)
    {
        switch (directionValue)
        {
            case RNDirection.East:
                return Vector3.forward;
            case RNDirection.South:
                return Vector3.right;
            case RNDirection.West:
                return Vector3.back;
            case RNDirection.North:
                return Vector3.left;
            default:
                throw new ArgumentOutOfRangeException(nameof(directionValue), directionValue, null);
        }
    }
    
    public void OnDirectionChange(RNDirection InNodeDirection)
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
