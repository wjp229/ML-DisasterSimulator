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

    public Direction NodeDirection
    {
        get { return nodeDirection; }
        set
        {
            nodeDirection = value;
            transform.rotation = Quaternion.Euler(new Vector3(0, (int)nodeDirection * 90f, 0));
        }
    }
}
