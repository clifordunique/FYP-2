using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : Enemy
{
    public int baseHealth;
    public int baseDamage;
    public float baseSpeed;
    public float baseRange;
    public float baseDropRate;

    public GameObject necromancerProjectilePrefab;

    public override void Attack()
    {
        animator.SetTrigger("Attack");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        health = baseHealth;
        speed = baseSpeed;
        damage = baseDamage;

        attacking = false;

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null && staggered != true && attacking != true)
        {

            
            if (health > 0)
            {
                LookAtPlayer();
                RangedBehaviour(baseRange, baseSpeed);
            }

            if (health == 0)
            {
                OnDeath();
            }
        }
    }
    //move to range to attack, move away from player
    protected void RangedBehaviour(float range, float speed)
    {
        float distance = Mathf.Abs(transform.position.x - playerTransform.position.x);

        bool playerGrounded = playerTransform.GetComponent<PlayerController>().grounded;

        if (distance > range && attacking != true && staggered != true)
        {
            MoveTowardsPlayer();
            animator.SetBool("Moving", true);
        }
        //add another for when player is not on ground(wont att)
        else if (distance >= range - 0.5f && distance <= range )
        {
            animator.SetBool("Moving", false);
            Attack();
        }
        else if (distance < range - 0.5f&& attacking != true)
        {
            MoveAwayFromPlayer();
            animator.SetBool("Moving", true);
        }else
        {
            animator.SetBool("Idle", true);
        }
    }

    public void FireProjectile()
    {
        NecromancerProjectile necromancerProjectile = Instantiate(necromancerProjectilePrefab, rigidbody2d.position - Vector2.up * 0.2f, Quaternion.identity).GetComponent<NecromancerProjectile>();
        necromancerProjectile.transform.localScale = new Vector3(transform.localScale.x, 1.0f, 1.0f);
        necromancerProjectile.Fired(new Vector2(transform.localScale.x, 0), 500);
        //throw new NotImplementedException();
    }
}
