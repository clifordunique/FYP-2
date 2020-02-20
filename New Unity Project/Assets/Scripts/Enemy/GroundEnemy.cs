using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Groundenemy should: not trace the player's y axis
//Groundenemy might: jump to players on platform
public abstract class GroundEnemy : Enemy
{

    public abstract void Attack();

    public virtual void RangedAttackGround()
    {

    }

    //Ground
    protected void GroundTraceAndAttackPlayer(float range, float speed, Vector3 playerPos)
    {
        if ((Mathf.Abs(transform.position.x - playerPos.x) > range) && attacking != true && staggered != true)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerPos.x, transform.position.y), speed * Time.deltaTime);
            animator.SetBool("Moving", true);
        }
        //add another for when player is not on ground(wont att)
        else if (Mathf.Abs(transform.position.x - playerPos.x) <= range)
        {
            animator.SetBool("Moving", false);
            Attack();
        }
    }
    protected IEnumerator Hit(float attackTime, float attackRange)
    {
        //~time needed for the attack animations.
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackTime);

        Collider2D playerToDamage = Physics2D.OverlapCircle(attackPos.position, attackRange, layerPlayer);
        DamagePlayer(playerToDamage);


    }
}
