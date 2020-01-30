using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rigidbody2d; 
 
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        //So it doesnt slow the game down when it is still flying outside the map
        if (transform.position.magnitude> 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Enemy e = other.gameObject.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(1);
        }
        Destroy(gameObject);
        Debug.Log("collision: " + other.gameObject);
    }
}
