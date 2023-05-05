using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EscapeeState
{
    Active,
    Damaged,
    Dead
}

public class Escapee : MonoBehaviour
{
    private Direction progressDirection;

    private float _speed;
    private float _slowSpeed;
    public float maxSpeed = 15;
    public float minSpeed = 2;

    private EscapeeState _state = EscapeeState.Active;

    private bool _isColliding;

    public void Awake()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
        _slowSpeed = Random.Range(minSpeed, maxSpeed / 2);
    }

    private void Update()
    {
        if (_state == EscapeeState.Dead)
            return;
        
        float curSpeed;
        
        if (_isColliding)
        {
            curSpeed = _slowSpeed;
        }
        else
        {
            curSpeed = _speed;
        }

        if (_state == EscapeeState.Damaged)
            curSpeed /= 3f;
        
        transform.localPosition += transform.forward * curSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Node"))
        {
            if (other.GetComponent<EscapeNode>().IsOnFire)
            {
                EscapeeDead();
            }
            
            Debug.Log("Collide");
            transform.forward = other.transform.forward;
        }
    }

    void EscapeeDead()
    {
        _state = EscapeeState.Dead;
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.layer == LayerMask.NameToLayer("Escapee"))
        {
            _isColliding = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Escapee"))
        {
            _isColliding = false;
        }
    }
}
