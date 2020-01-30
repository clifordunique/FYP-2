using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startPos;
    private Transform cam;
    public float parallaxEffect;
    // Start is called before the first frame update
    private void Awake()
    {
        cam = Camera.main.transform;
    }
    void Start()
    {
        
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float d = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startPos + d, transform.position.y, transform.position.z);
    }
}
