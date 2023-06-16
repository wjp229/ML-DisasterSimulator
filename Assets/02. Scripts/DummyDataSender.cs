using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyDataSender : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            int ranVal = Random.Range(0, SimulatorManager.Instance.escapeNodes.Count);
            
            SimulatorManager.Instance.escapeNodes[ranVal].NodeDirection = RNDirection.South;
        }
    }
}
