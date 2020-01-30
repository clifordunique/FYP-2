using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Application.LoadLevel(Application.loadedLevel);
            //SceneManager.LoadScene("Stage1",LoadSceneMode.Single);
            GameManager.instance.level += 1;
        }
    }
}
