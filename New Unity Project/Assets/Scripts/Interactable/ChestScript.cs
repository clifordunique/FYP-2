using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public Sprite openSprite, closedSprite;
    public GameObject[] chestItems;
    //temp
    public GameObject boots;
    //for testing
    public bool isOpen;

    private BoxCollider2D boxCollider2D;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isOpen = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isOpen == false)
        {
            WhenOpened();
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
    public void WhenOpened()
    {
       
        isOpen = true;
        spriteRenderer.sprite = openSprite;
        Invoke("Destroy", 2.0f);
        //for (int i = 0; i < chestitems.length; i++)
        //{

        //}
        GameObject chestItem = Instantiate(chestItems[Random.Range(0,chestItems.Length)], transform.position + Vector3.up * 0.2f, Quaternion.identity);
        //GameObject bootss = Instantiate(Resources.Load<GameObject>("Prefabs/Items/BootsOfSpeed"), transform.position + Vector3.up * 0.0f, Quaternion.identity);

        chestItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-300, 300), Random.Range(100,400)));

    }

}