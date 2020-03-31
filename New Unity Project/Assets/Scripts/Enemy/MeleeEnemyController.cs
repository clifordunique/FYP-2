using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : Enemy      //concrete*
{
    public int baseHealth;
    public int baseDamage;
    public float baseSpeed;

    public float baseAttackRange;
    public float baseRange;
    public Transform attackPos;

    //for others, would be initialized to (int)AttackStyle.Melee at start()

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
                MeleeBehaviour(baseRange, baseSpeed);
                LookAtPlayer();
            }

            if (health == 0)
            {
                OnDeath();
            }
        }
    }

    public override void Attack()
    {
        if (!attacking)
            animator.SetTrigger("Attack");
    }

    public void MeleeAttack()
    {
        Collider2D playerToDamage = CheckForPlayerInFront();
        if (playerToDamage != null)
        {
            DamagePlayer(playerToDamage);
        }
    }

    public Collider2D CheckForPlayerInFront()
    {
        return Physics2D.OverlapCircle(attackPos.position, baseAttackRange, layerPlayer);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, baseAttackRange);
    }
    //Ground
    void MeleeBehaviour(float range, float speed)
    {
        float distance = Mathf.Abs(transform.position.x - playerTransform.position.x);
        bool playerGrounded = playerTransform.GetComponent<PlayerController>().grounded;

        if (distance > range && attacking != true && staggered != true)
        {
            MoveTowardsPlayer();
            animator.SetBool("Moving", true);
        }
        //add another for when player is not on ground(wont att)
        else if (distance <= range && playerGrounded && attacking != true)
        {
            animator.SetBool("Moving", false);
            Attack();
        }
    }

}
