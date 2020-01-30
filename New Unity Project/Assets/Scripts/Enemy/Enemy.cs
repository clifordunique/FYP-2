using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    [System.NonSerialized] public float speed;
    protected Rigidbody2D rigidbody2d;
    protected Animator animator;
    protected Transform playerLocation;
    public bool staggered;
    [System.NonSerialized] public int damage;

    void Start()
    {
        staggered = false;
    }
    

    //This is important as it allows AttackEnemy() to get health component from <Enemy> instead of checking for enemy type first
    public void TakeDamage(int damage)
    {
        //maybe can be used for different enemies? 
        animator.SetTrigger("Hurt");
        health -= damage;
        staggered = true;
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

        public void LookAtPlayer()
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





    //To be deleted
    public void TracePlayer(float range, float speed, Vector3 target, Vector3 enemy)
    {
        if (Vector2.Distance(enemy, target) > range)
        {
            animator.SetBool("Moving", true);
            transform.position = Vector2.MoveTowards(enemy, target, speed * Time.deltaTime);
            //Debug.Log("Moving");
        }
        else
        {
            animator.SetBool("Moving", false);
            //attackplayer
            Debug.Log("Not moving");
        }
    }
}
