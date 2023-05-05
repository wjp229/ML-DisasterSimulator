using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Forward = 0,
    Right = 1,
    Back = 2,
    Left = 3
}

public class EscapeNode : MonoBehaviour
{
    private Direction nodeDirection;

    private bool isOnFire = false;
    public bool IsOnFire
    {
        get { return isOnFire; }
        set
        {
            isOnFire = value;
            FireParticle.SetActive(value);
        }
    }

    public float TransitionTime = 12.0f;
    private float curTime = 0;

    public List<EscapeNode> connectedNode = new List<EscapeNode>();

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

    public void Update()
    {
        if (!IsOnFire) return;

        curTime += Time.deltaTime;

        if (curTime > TransitionTime)
        {
            TransitFire();
        }
    }

    public void TransitFire()
    {
        foreach (var node in connectedNode)
        {
            node.IsOnFire = true;
        }
    }
}
