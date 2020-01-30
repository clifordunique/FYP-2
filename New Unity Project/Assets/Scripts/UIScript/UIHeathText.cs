using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHeathText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = player.GetComponent<PlayerController>().health.ToString();
    }
}
