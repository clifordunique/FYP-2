using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsOfSpeed : ChestItems
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().ChangeSpeed(2.0f);
            gameObject.SetActive(false);
        }

    }

}
