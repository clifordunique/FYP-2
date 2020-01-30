using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Used for testing
public interface IUnityService
{
    float GetAxis(string axisName);
    bool GetMouseButtonDown1();
}

class UnityService : IUnityService
{
    public float GetAxis(string axisName)
    {
        return Input.GetAxis(axisName);
    }
    public bool GetMouseButtonDown1()
    {
        return Input.GetMouseButtonDown(1);
    }
}