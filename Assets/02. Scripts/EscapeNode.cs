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
        if (Input.GetKeyDown(KeyCode.A))
        {
            IsOnFire = true;
        }
    }
}
