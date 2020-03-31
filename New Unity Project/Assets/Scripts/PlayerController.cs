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

    //boxcollider seems to cuase the player to get stucked
    private EdgeCollider2D edge2d;

    private float baseSpeed = 4.0f;
    private bool combatIdle = false;
    private bool isDead = false;

    //speed modifier for different speed when in different actions(attack,shooting etc.)
    public float speedMod;

    public readonly float meleeSpeedModValue = 0.5f;
    public readonly float rangedSpeedModValue = 0.1f;
    public readonly float castSpeedModeValue = 0;
    public readonly float defaultSpeedModValue = 1;

    public float fallMultiplier = 2.5f;       //makes falling more natural
    public readonly float speedPerLevel = 0.5f;

    //Levels
    public int speedLevel;
    public int rangedLevel;
    public int attackLevel;
    public int magicLevel;

    //state(anim)
    public bool attacking;
    public bool shooting;
    public bool staggered;
    public bool inAction;

    public GameObject arrowPrefab;
    public LayerMask layerEnemies;
    public int attackMove;

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
        staggered = false;

        speedMod = 1;

        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        box2d = GetComponent<BoxCollider2D>();
        edge2d = GetComponent<EdgeCollider2D>();
        health = maxHealth;
        attackMove = 1;

        speed = baseSpeed;
        speedLevel = 0;

        //currentAnim = "No Animation";

        if (UnityService == null)
        {
            UnityService = new UnityService();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetPosition().x);
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, layerGround);
        animator.SetBool("Grounded", grounded);
        speed = baseSpeed + speedLevel * speedPerLevel;
        //Debug.Log(currentAnim);

        //checking if the player is in an animation(r.g. attacking)
        inAction = attacking || shooting;

        //Debug.Log(grounded);
        //Debug.Log(XSpeedCheck());
        lookdirection = new Vector2 (transform.localScale.x, 0);

        // -- Handle input and movement --

        float inputX = UnityService.GetAxis("Horizontal");
        //float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        
            ChangeLookDirection(inputX);

        if (!staggered)
        {
            Move(inputX);
        }
        //body2d.velocity = new Vector2(inputX * speed * speedMod, body2d.velocity.y);

        if (body2d.velocity.y < 0)
        {
            body2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        //Attack
        if (!inAction)
        {
            //without unityServiceL Input.GetKeyDown(KeyCode.K)

            if (UnityService.GetMouseButtonDown1() || Input.GetKeyDown(KeyCode.K)) //can move to below?tbc
            {
                ShootArrow();
            }
            else if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J)))
            {
                StartAttack();
            }
            else if (Input.GetKeyDown("q"))
            {
                Punch();
            }
        }

        
        //Jump
        if (Input.GetKeyDown("space") && grounded)
        {
            Jump();
        }


        //Run
        //tbc       

        

        //state for loop animations.
        else if ((Mathf.Abs(inputX) > Mathf.Epsilon) && grounded) { 
            animator.SetInteger("AnimState", 1);
            //x = 1;
        }
        //falling
        else if ((body2d.velocity.y <= 0) && !grounded)
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



    public void Launch()
    {
        //8th frame, each frame is 1/12 seconds
        // + Vector2.up * 0.1f
        Arrow arrow = Instantiate(arrowPrefab, body2d.position - Vector2.up * 0.1f, Quaternion.identity).GetComponent<Arrow>();
        //tbc?
        //ARROW2
        //Vector2 lookdirection = new Vector2(transform.localScale.x, 0);       

        //negative used because default is to left
        arrow.transform.localScale = new Vector3(transform.localScale.x, 1.0f, 1.0f);

        //tbc, scale with ranged lvl?
        var fireDirection = lookdirection + new Vector2(0, 0.05f);
        arrow.Fired(fireDirection, 1000);
    }





    //taking damage
    public void TakingDamage(int damage)
    {
        ChangeHealth(-damage);
        if (health > 0)
        {
            animator.SetTrigger("Hurt");
        }
    }

    public void ChangeHealth(int amount)
    {
        health += amount;
        if(UIHealth.Instance!=null)
        UIHealth.Instance.SetValue(health / (float)maxHealth);
    }
    
    //attack moves
    public void StartAttack()
    {
        if (grounded)
        {
            animator.SetTrigger("BasicHit" + attackMove.ToString());
            if (attackMove == 1)
            {
                attackMove = 2;
            }
            else if (attackMove == 2)
            {
                attackMove = 1;
            }
            //timeBtwAttack = meleeTimeBtwAttack;
        }
        else if (!grounded)
        {
            //***
            animator.SetTrigger("JumpAttack1");
            //StartCoroutine(MeleeAttack(jumpAttackRange, jumpAttackPos));
            //tbc chain
        }
    }

    public void ShootArrow()
    {
        if (grounded)
        animator.SetTrigger("Shoot");
        //timeBtwAttack = rangedTimeBtwAttack;
    }

    public void Pickup(GameObject item)
    {
        if (item.CompareTag("Boots"))
        {
            speedLevel += 1;           
            Destroy(item);
        }
        if (item.CompareTag("Scroll"))
        {
            magicLevel += 1;
            Destroy(item);
        }
        if (item.CompareTag("Sword"))
        {
            attackLevel += 1;
            Destroy(item);
        }
        if (item.CompareTag("Bow"))
        {
            rangedLevel += 1;
            Destroy(item);
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
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float XSpeedCheck()
    {
        return Mathf.Abs(body2d.velocity.x);
    }
    public float GetVelocityX()
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


    public void BasicHit1()
    {

        //camAnim.SetTrigger("Shake");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(basicHit1Pos.position, basicHit1Range, layerEnemies);
        MeleeHit(enemiesToDamage);

        //movement disabled until attack animation is finished
    }


    public void BasicHit2()
    {

        //tbc
        //camAnim.SetTrigger("Shake");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(basicHit2Pos.position, basicHit2Range, layerEnemies);
        MeleeHit(enemiesToDamage);
    }

    
    //As Melee attacks should hit all enemies in the area and uses overlap functions, Collider2D[] is used for detecting the enemies 
    public void MeleeHit(Collider2D[] enemiesToDamage)
    {
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            //GolemController should change to enemy                            not sure if needed tbc
            if ((enemiesToDamage[i].GetComponent<Enemy>() != null) && (enemiesToDamage[i].GetComponent<Enemy>().health > 0))
            {
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                //enemiesToDamage[i].GetComponent<Rigidbody2D>().AddForce(-transform.right * 20000);
                Debug.Log("hit!");
            }
        }
    }
    public bool IsCurrentAnimaion(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
    public void MeleeAttack(float range, Transform pos)
    {
        //tbc
        //camAnim.SetTrigger("Shake");
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(pos.position, range, layerEnemies);
        MeleeHit(enemiesHit);
    }

    public void ChangeLookDirection(float inputX)
    {
        if (!(inAction || staggered))
        {
            if (inputX > 0)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            else if (inputX < 0)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
        }
    }
    public void Move(float inputX)
    {
            body2d.velocity = new Vector2(inputX * speed * speedMod, body2d.velocity.y);
    }
    //for testing
    public float GetLookDirection()
    {
        return transform.localScale.x;
    }
}
