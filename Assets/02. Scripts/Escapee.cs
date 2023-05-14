using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EscapeeState
{
    Moving,
    Staying,
    Following,
    Dead
}

public class Escapee : MonoBehaviour
{
    private Direction progressDirection;

    private float _speed;
    private float _slowSpeed;
    public float maxSpeed = 15;
    public float minSpeed = 2;

    public EscapeeState _state = EscapeeState.Staying;

    private float stayingTime = 4f;
    public float minStayingTime = 1f;
    public float maxStayingTime = 6f;

    private bool _isColliding;

    private NavMeshAgent agent;

    public void Awake()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
        _slowSpeed = Random.Range(minSpeed, maxSpeed / 2);

        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        switch (_state)
        {
            case EscapeeState.Moving:
                Moving();
                break;
            case EscapeeState.Staying:
                Staying();
                break;
            case EscapeeState.Following:
                Following();
                break;
            case EscapeeState.Dead:
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Following()
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

    private float curStaytime = 0f;
    private Vector3 newDestination;
    private void Staying()
    {
        curStaytime += Time.deltaTime;
        if (curStaytime > stayingTime)
        {
            curStaytime = 0f;
            NavMeshPath path = new NavMeshPath();
            do
            {
                // Find Next Destination
                float newXValue = Random.Range(-20f, 20f);
                float newZValue = Random.Range(-30f, 30f);

                newDestination = new Vector3(newXValue, 0, newZValue);

            } while (!agent.CalculatePath(newDestination, path));

            _state = EscapeeState.Moving;
            agent.destination = newDestination;
        }

    }

    private float offset = 2f;
    private void Moving()
    {
        if (Vector3.Distance(newDestination, transform.position) < offset + transform.position.y)
        {
            _state = EscapeeState.Staying;
        }
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Node"))
        {
            if (other.GetComponent<EscapeNode>().IsOnFire)
            {
                EscapeeDead();
            }
            
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
