using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed;

    public int maxHealth;
    public int health;

    //jump
    [SerializeField] float jumpForce;
    public bool grounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask layerGround;

    private Animator animator;
    private Rigidbody2D body2d;
    private BoxCollider2D box2d;
    private EdgeCollider2D edge2d;

    private float baseSpeed = 4.0f;
    private bool combatIdle = false;
    private bool isDead = false;

    //speed modifier for different speed when in different actions(attack,shooting etc.)
    public float speedMod = 1;

    //AttackTimer
    private float timeBtwAttack;
    private float meleeTimeBtwAttack;
    private float rangedTimeBtwAttack;

    //Levels
    public int speedLevel;
    public int rangedLevel;
    public int attackLevel;
    public int magicLevel;

    public bool attacking;
    public bool shooting;
    public GameObject arrowPrefab;
    public LayerMask layerEnemies;
    public int attackChain;

    public int damage;

    public float basicHit1Range;
    public float basicHit2Range;
    public float jumpAttackRange;
    public Transform basicHit1Pos;
    public Transform basicHit2Pos;
    public Transform jumpAttackPos;

    public Animator camAnim;

    private Vector2 lookdirection = new Vector2(1, 0);

    public string currentAnim;
    //int x = 0;

    //Testing
    public IUnityService UnityService;

    // Use this for initialization
    void Start()
    {

        attacking = false;
        shooting = false;

        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        box2d = GetComponent<BoxCollider2D>();
        edge2d = GetComponent<EdgeCollider2D>();
        health = maxHealth;
        attackChain = 1;

        speed = baseSpeed;
        speedLevel = 0;

        currentAnim = "No Animation";

        if (UnityService == null)
        {
            UnityService = new UnityService();
        }
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, layerGround);
        animator.SetBool("Grounded", grounded);
        Debug.Log(currentAnim);

        //checking if the player is in an animation(r.g. attacking)
        bool InAction = attacking || shooting;

        //Debug.Log(grounded);
        //Debug.Log(XSpeedCheck());
        lookdirection = new Vector2 (transform.localScale.x, 0);

        // -- Handle input and movement --

        float inputX = UnityService.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction

        if (inputX > 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        // Move

        //if (attacking)
        //{
        //    body2d.velocity = new Vector2(inputX * speed * 0.2f, body2d.velocity.y);
        //}
        //else if (shooting)
        //{
        //    body2d.velocity = new Vector2(0, body2d.velocity.y);
        //}
        //tbc else was originally used, causes probelm cause it makes the velocity if
        //else
        if (inputX != 0)
        {
            body2d.velocity = new Vector2(inputX * speed * speedMod, body2d.velocity.y);
        }


        // Old method, does not allow for more in depth settings for e.g. stopping movement while shooting arrow
        //if (timeBtwAttack <= 0)
        //{
        //    body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);
        //}
        //else
        //{
        //    //speed 
        //    body2d.velocity = new Vector2(inputX * 0.5f, body2d.velocity.y);
        //}




        //Attack
        //of (timeBtwAttack <= 0)
        if (!InAction)
        {
            //without unityServiceL Input.GetKeyDown(KeyCode.K)

            if ((UnityService.GetMouseButtonDown1() || Input.GetKeyDown(KeyCode.K)) && grounded) //can move to below?tbc
            {
                ShootArrow();
            }
            else if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J)))
            {
                Attack();
            }
            else if (Input.GetKeyDown("q"))
            {
                Punch();
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }

        
        //Jump
        if (Input.GetKeyDown("space") && grounded)
        {
            Jump();
        }


        //Run
        //tbc       

        


        else if ((Mathf.Abs(inputX) > Mathf.Epsilon) && grounded) { 
            animator.SetInteger("AnimState", 1);
            //x = 1;
        }
        //falling
        else if ((body2d.velocity.y < 0) && !grounded)
        {
            animator.SetInteger("AnimState", 2);
            //x = 2;
        }
        //falling

        //Idle
        else {
            animator.SetInteger("AnimState", 0);
            //x = 0;
        }
            
        //Debug.Log(x);

    }
    public void Punch()
    {
        animator.SetTrigger("FlyingPunch");
        //force tbc
        StartCoroutine(Punching());
        
    }
    public IEnumerator Punching()
    {
        yield return new WaitForSeconds(0.35f);

        //body2d.AddForce(lookdirection * 3000);
        body2d.AddForce(lookdirection*100);

        yield return null;
    }

    public IEnumerator MeleeAttack(float time, float range, Transform pos)
    {
        yield return new WaitForSeconds(time);

        //tbc
        //camAnim.SetTrigger("Shake");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(pos.position, range, layerEnemies);
        MeleeHit(enemiesToDamage);
    }

    public IEnumerator Launch()
    {
        //8th frame, each frame is 1/12 seconds
        yield return new WaitForSeconds(0.667f);
        // + Vector2.up * 0.1f
        Arrow arrow = Instantiate(arrowPrefab, body2d.position - Vector2.up * 0.1f, Quaternion.identity).GetComponent<Arrow>();
        //tbc?
        //ARROW2
        //Vector2 lookdirection = new Vector2(transform.localScale.x, 0);       

        //negative used because default is to left
        arrow.transform.localScale = new Vector3(-transform.localScale.x, 1.0f, 1.0f);

        //tbc, scale with ranged lvl?
        arrow.Launch(lookdirection, 1000);
    }



    //Easier testing
    //As Melee attacks should hit all enemies in the area and uses overlap functions, Collider2D[] is used for detecting the enemies 
    public void MeleeHit(Collider2D[] enemiesToDamage)
    {
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            //GolemController should change to enemy                            not sure if needed tbc
            if ((enemiesToDamage[i].GetComponent<Enemy>() != null) && (enemiesToDamage[i].GetComponent<Enemy>().health>0))
            {
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                //enemiesToDamage[i].GetComponent<Rigidbody2D>().AddForce(-transform.right * 20000);
                Debug.Log("hit!");
            }
        }
    }


    //taking damage
    public void TakingDamage(int damage)
    {
        ChangeHealth(-damage);
    }

    public void ChangeHealth(int amount)
    {
        health += amount;
        UIHealth.Instance.SetValue(health / (float)maxHealth);
    }

    //attack moves
    public void Attack()
    {
        if (grounded)
        {
            animator.SetTrigger("BasicHit" + attackChain.ToString());
            if (attackChain == 1)
            {
                StartCoroutine(MeleeAttack(0.21f,basicHit1Range,basicHit1Pos));
                attackChain = 2;
            }
            else if (attackChain == 2)
            {
                StartCoroutine(MeleeAttack(0.12f, basicHit2Range, basicHit2Pos));
                attackChain = 1;
            }
            //timeBtwAttack = meleeTimeBtwAttack;
        }
        else if (!grounded)
        {
            //***
            animator.SetTrigger("JumpAttack1");
            StartCoroutine(MeleeAttack(0.05f, jumpAttackRange, jumpAttackPos));
            //tbc chain
        }
    }

    public void ShootArrow()
    {
        animator.SetTrigger("Shoot");
        StartCoroutine(Launch());
        timeBtwAttack = rangedTimeBtwAttack;
    }

    public void Pickup(GameObject other)
    {
        //29 11
        if (other.CompareTag("Boots"))
        {
            speedLevel += 1;
            speed = baseSpeed + speedLevel;
            //other.gameObject.SetActive(false);
            Destroy(other);
        }
        if (other.CompareTag("Scroll"))
        {
            magicLevel += 1;
            Destroy(other);
        }
        if (other.CompareTag("Sword"))
        {
            attackLevel += 1;
            Destroy(other);
        }
        if (other.CompareTag("Bow"))
        {
            rangedLevel += 1;
            Destroy(other);
        }
    }
    
    public void Jump()
    {
        animator.SetTrigger("Jump");
        body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
    }
    public void ChangeSpeed(float speedChange)
    {

        speed += speedChange;
    }

    //item pickup
    void OnTriggerEnter2D(Collider2D other)
    {
        Pickup(other.gameObject);
    }

    //Added for testing
    public float XSpeedCheck()
    {
        return body2d.velocity.x;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(basicHit1Pos.position, basicHit1Range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(basicHit2Pos.position, basicHit2Range);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(jumpAttackPos.position, jumpAttackRange);

    }


    //prolly not needed
    public IEnumerator BasicHit1(float time)
    {
        //Time needed for the blade to hit in the animations. Calcuated by looking at the exact frame of the sword's swing divided by the sample rate(12)
        yield return new WaitForSeconds(time);

        //tbc
        //camAnim.SetTrigger("Shake");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(basicHit1Pos.position, basicHit1Range, layerEnemies);
        MeleeHit(enemiesToDamage);

        //movement disabled until attack animation is finished
    }


    public IEnumerator BasicHit2(float time)
    {

        yield return new WaitForSeconds(time);
        //tbc
        //camAnim.SetTrigger("Shake");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(basicHit2Pos.position, basicHit2Range, layerEnemies);
        MeleeHit(enemiesToDamage);

    }
}
