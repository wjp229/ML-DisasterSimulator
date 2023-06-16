using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraUI : MonoBehaviour
{
 
    public SimulatorManager simulatorManager;
    public void ToggleCameraButton(bool param)
    {
        if (param)
        {
            simulatorManager.MainCam.depth = -3;
        }
        else
        {
            simulatorManager.MainCam.depth = -1;
        }
    }
}
