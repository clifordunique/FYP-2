using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestUtils
{
    public static GameObject[] FindListOf(string x)
    {
        return GameObject.FindGameObjectsWithTag(x);
    }
}

