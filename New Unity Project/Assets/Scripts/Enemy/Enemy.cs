using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody2D rigidbody2d;
    protected Animator animator;
    protected Transform playerLocation;

    public int damage;
    public float speed;
    public int health;
    public bool canBeStaggered;

    //for attack
    public LayerMask layerPlayer;
    public Transform attackPos;             //for melee attacks

    public bool attacking;
    public bool staggered;

    void Start()
    {
        attacking = false;
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        staggered = false;
        Debug.Log("HELLO" + playerLocation);
    }

    //public abstract void Attack();
    //public abstract void TracePlayer();
    
    //This is important as it allows AttackEnemy() to get health component from <Enemy> instead of checking for enemy type first
    public virtual void TakeDamage(int damage)
    {
        //maybe can be used for different enemies? 
        if (canBeStaggered)
        {
            animator.SetTrigger("Hurt");
        }
        
        health -= damage;
    }

    public void DamagePlayer(Collider2D player)
    {
        Debug.Log(staggered);
        if (player.GetComponent<PlayerController>() != null && staggered != true)
        {
            player.GetComponent<PlayerController>().TakingDamage(damage);
        }
        else if (staggered == true && player.GetComponent<PlayerController>() != null)
        {
            Debug.Log("cannot attack because staggered");
        }
    }

        protected void LookAtPlayer()
        {
        if (transform.position.x > playerLocation.position.x)
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
        if (Mathf.Abs(transform.position.x - playerLocation.position.x) > range)
        {

        }
    }



    protected void Destroy()
    {
        Destroy(gameObject);
    }

    protected void OnDeath() {
        animator.SetTrigger("Death");

        //so it does get hit more
        Destroy(GetComponent<BoxCollider2D>());

        //so it does not fall under the map as boxcollider2d is now disabled
        Destroy(GetComponent<Rigidbody2D>());

        //so it does not trigger it over and over again
        health = -1;
        Invoke("Destroy", 5.0f);
    }






    

}
