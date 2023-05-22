using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUI : MonoBehaviour
{
    public void ToggleCameraButton(bool param)
    {
        if (param)
        {
            SimulatorManager.Instance.MainCam.depth = -3;
        }
        else
        {
            SimulatorManager.Instance.MainCam.depth = -1;
        }
    }
}
