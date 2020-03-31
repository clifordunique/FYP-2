using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody2D rigidbody2d;
    protected Animator animator;
    protected Transform playerTransform;

    public GameObject Chest;

    public int damage;
    public float speed;
    public int health;
    public float dropRate;
    public bool canBeStaggered;

    

    //for attack
    public LayerMask layerPlayer;
    

    public bool attacking;
    public bool staggered;


    //public abstract void Attack();
    //public abstract void TracePlayer();
    
    //This is important as it allows AttackEnemy() to get health component from <Enemy> instead of checking for enemy type first
    public virtual void TakeDamage(int damage)
    {
        if (canBeStaggered == true)
        {
            animator.SetTrigger("Hurt");
        }       
        health -= damage;
    }

    public void DamagePlayer(Collider2D player)
    {
        if (player.GetComponent<PlayerController>() != null)
        {
            player.GetComponent<PlayerController>().TakingDamage(damage);
        }
    }

        protected void LookAtPlayer()
        {
        if (transform.position.x > playerTransform.position.x)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
    //Grounded enemies which move towards the player til the enemy is at a certain range(determined by the enemy's attack range)
    //https://answers.unity.com/questions/1084574/vector3movetowards-on-y-axis-only-c.html
    public void MoveTowardsPlayersGrounded(float range)
    {
        if (Mathf.Abs(transform.position.x - playerTransform.position.x) > range)
        {

        }
    }
    //enemy should move slower when running away from the player so the player can catch up easier
    public readonly float backwardsSpeedMod = 0.6f;

    public abstract void Attack();

    protected void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTransform.position.x, transform.position.y), speed * Time.deltaTime);
    }
    protected void MoveAwayFromPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTransform.position.x, transform.position.y), -backwardsSpeedMod * speed * Time.deltaTime);
    }


    protected void Destroy()
    {
        Destroy(gameObject);
    }

    public void OnDeath() {

        animator.SetTrigger("Death");
        Drop();

        //so it does get hit more
        Destroy(GetComponent<BoxCollider2D>());

        //so it does not fall under the map as boxcollider2d is now disabled
        Destroy(GetComponent<Rigidbody2D>());

        //so it does not trigger it over and over again
        health = -1;

        Invoke("Destroy", 5.0f);
    }

    //used in tests
    public float GetLookDirection()
    {
        return transform.localScale.x;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Drop()
    {
        float randomValue = UnityEngine.Random.value;
        if (randomValue <= dropRate)
        {
            Instantiate(Chest, rigidbody2d.position - Vector2.up * 1f, Quaternion.identity);
        }
    }
}
