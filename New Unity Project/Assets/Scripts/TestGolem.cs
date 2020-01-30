using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGolem : Enemy
{
    public int testGolemHealth = 5;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        health = testGolemHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
