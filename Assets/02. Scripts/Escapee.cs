using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Escapee : MonoBehaviour
{
    private Direction progressDirection;

    private float _speed;
    private float _slowSpeed;
    public float maxSpeed = 15;
    public float minSpeed = 2;

    private bool _isColliding;

    public void Awake()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
        _slowSpeed = Random.Range(minSpeed, maxSpeed / 2);
    }

    private void Update()
    {
        float curSpeed;
        
        if (_isColliding)
        {
            curSpeed = _slowSpeed;
        }
        else
        {
            curSpeed = _speed;
        }
            
        
        transform.localPosition += transform.forward * curSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
    }
}
