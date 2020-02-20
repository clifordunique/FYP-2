using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockEnemyController : GroundEnemy
{
    public int baseMockHealth;
    public int baseMockDamage;
    public float baseMockSpeed;

    public float mockAttackRange;
    public float mockAttackTime;
    public float mockRange;



    // Start is called before the first frame update
    void Awake()
    {
        health = baseMockHealth;
        speed = baseMockSpeed;
        damage = baseMockDamage;

        attacking = false;

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerLocation != null && staggered != true)
        {
            LookAtPlayer();

            if (health > 0)
            {
                GroundTraceAndAttackPlayer(mockRange, baseMockSpeed, playerLocation.position);
            }           
        }

        if (health == 0)
        {
            OnDeath();
        }

        if (staggered == true)
        {
            StopAllCoroutines();
        }
    }
    public override void Attack()
    {
        if (attacking != true)
        StartCoroutine(Hit(mockAttackTime, mockAttackRange));
    }
    void OnDrawGizmosSelected()
    {   
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, 1.0f);
    }


}
