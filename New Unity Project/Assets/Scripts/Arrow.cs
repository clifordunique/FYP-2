using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rigidbody2d; 
 
    //Start() does not work as the rigidbody2d is used the same frame it is created (when it is shot out by the player)
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        //So it doesnt slow the game down when it is still flying outside the map
        if (transform.position.magnitude> 100.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Fired(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (enemy != null)
        {
            enemy.TakeDamage(1);
        }else if (player != null){
            //wont hit the player himself
            return;
        }
        Destroy(gameObject);
        Debug.Log("collision: " + other.gameObject);
    }
}
