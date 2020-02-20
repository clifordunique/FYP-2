using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemController : GroundEnemy
{

    public int GolemHealth;
    public float GolemSpeed;
    public float GolemRange;
    public float GolemAttackRange;
    public int GolemDamage;

    //attack
    private float timeBtwAttack;
    private float golemTimeBtwAttack;



    // Start is called before the first frame update
    void Awake()
    {
        health = GolemHealth;
        speed = GolemSpeed;
        damage = GolemDamage;

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timeBtwAttack = 0;
        golemTimeBtwAttack = 1.95f;
        

    }


    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
        //Debug.Log(staggered);
        //health > 0 so it will not trace when it is dead
        GroundTraceAndAttackPlayer(GolemRange, GolemSpeed, playerLocation.position);


        if (staggered == true)
        {
            //Used to stop the damage from going through even though the animation is cut off by the hurt animation
            StopAllCoroutines();
            Debug.Log("coroutine STOPPED");
        }

        //Death tbc to enemy instead maybe?
        if (health == 0)
        {
            OnDeath();
        }
    }

    override public void Attack()
    {
        if(!attacking)
        StartCoroutine(Hit(golemTimeBtwAttack, GolemRange));

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, GolemAttackRange);
    }

}
//IEnumerator GolemHit()
//{
//    //~10/12 second calculated from the frame of the golem swinging his fists
//    yield return new WaitForSeconds(1.3f);

//    Collider2D playerToDamage = Physics2D.OverlapCircle(attackPos.position, GolemAttackRange, layerPlayer);
//    if (playerToDamage != null)
//    {
//        DamagePlayer(playerToDamage);
//    }

//}