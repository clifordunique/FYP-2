using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject[] rooms;

    private int direction;

    public float minX;
    public float maxX;
    private bool stopGeneration;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;

    public float moveAmount;
    public int room;

    public LayerMask whatIsRoom;
    public Enemy[] enemies;

    private GameObject current;

    public void SetUpLevel(int level)
    {
        //level needs to be reduced by 1 as the list starts from 0
        //Instantiate(rooms[level-1], transform.position, Quaternion.identity);
        //spawn enemy?
        //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
