using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
    public float Speed;

    public PlayerMovement(float speed)
    {
        Speed = speed;
    }
    public float Cal(float x)
    {        
        return x * Speed;
    }
    public Vector3 SetLookDirection(float x)
    {
        if (x >= 0)
        {
            return new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            return new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }
}
