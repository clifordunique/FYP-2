using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerProjectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //So it doesnt slow the game down when it is still flying outside the map
        if (transform.position.magnitude > 100.0f)
        {
            Debug.Log("Projectile destroyed out of scene");
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.TakingDamage(1);
        }
        else if (enemy != null)
        {
            //wont hit the enemy himself
            return;
        }
        Destroy(gameObject);
        Debug.Log("collision: " + other.gameObject);
    }

    public void Fired(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
}
