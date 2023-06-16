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
    Dead,
    Escaped
}

public class Escapee : MonoBehaviour
{
    private RNDirection progressDirection;

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

    private GameObject CameraObj;

    bool initFollowing = true;

    private Vector3 CurrentDestination;

    public void Awake()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
        _slowSpeed = _speed / 2f;

        agent = GetComponent<NavMeshAgent>();
        CameraObj = GetComponentInChildren<Camera>().gameObject;
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
            case EscapeeState.Escaped:
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Following()
    {
        if (initFollowing)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, 1 << LayerMask.NameToLayer("Node"));
            Collider nearestCollider = null;
            
            float minSqrDistance = Mathf.Infinity;
            for (int i = 0; i < colliders.Length; i++)
            {
                float sqrDistanceToCenter = (transform.position - colliders[i].transform.position).sqrMagnitude;
                if (sqrDistanceToCenter < minSqrDistance)
                {
                    minSqrDistance = sqrDistanceToCenter;
                    nearestCollider = colliders[i];
                }
            }

            CurrentDestination = nearestCollider.transform.position;
            
            initFollowing = false;
        }
        
        agent.destination = CurrentDestination;
    }

    void MoveToNearNode()
    {
        
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
                float newXValue = Random.Range(-45f, 45f);
                float newZValue = Random.Range(-45f, 70f);

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
            if (other.gameObject.GetComponent<ExitNode>())
            {
                Escaped();
                return;
            }
            
            if (other.GetComponent<RouteNode>().IsOnFire)
            {
                EscapeeDead();
                return;
            }

            StartCoroutine(TransferDirection(other , 0f));
        }
    }

    private IEnumerator TransferDirection(Collider other, float delayTime)
    {
        yield return null;
        
        CurrentDestination = other.GetComponent<RouteNode>().GetTargetDestination();
    
        //transform.forward = other.transform.forward;
    }

    void EscapeeDead()
    {
        _state = EscapeeState.Dead;
        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
        foreach (var mr in mrs)
        {
            mr.material.color = Color.red;
        }
        SimulatorManager.Instance.EscapeeDead(this);
        
        Destroy(CameraObj);
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

    private void Escaped()
    {
        _state = EscapeeState.Escaped;
        SimulatorManager.Instance.EscapeeEscaped(this);
        
        Destroy(CameraObj);

        Destroy(this);
    }
}
