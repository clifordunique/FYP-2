using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItems : MonoBehaviour
{
    protected void Awake()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(OnSpawn());
       
    }

    //so the item will not be picked up instantly after spawning
    IEnumerator OnSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
