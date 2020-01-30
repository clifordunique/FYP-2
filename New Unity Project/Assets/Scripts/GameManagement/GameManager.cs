using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameObject currentGame;

    public int level;
    private LevelManager levelManager;
    public static GameObject current;

    void Awake()
    {
        if (instance == null)
        {
            level = 1;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else if (instance != null)
        {
            Destroy(gameObject);    //gameObject is a local variable of type GameObject which is inherited from Component.                                   //It allows one to access the instance of the GameObject to which this component is attached.
        }

        levelManager = GetComponent<LevelManager>();

        Debug.Log("Init1");
        InitGame();
    }

    void InitGame()
    {
        levelManager.SetUpLevel(level);
    }

}
