using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject [] floor;
    GameObject tile;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, floor.Length);
        
        //https://forum.unity.com/threads/instantiating-prefab-as-child-of-existing-gameobject-c.83598/
        tile = Instantiate(floor[rand], transform.position, Quaternion.identity);
        tile.transform.SetParent(transform);
    }
}
