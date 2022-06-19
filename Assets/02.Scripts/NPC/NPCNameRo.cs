using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNameRo : MonoBehaviour
{
    public GameObject player;
   
    private void Start()
    {
        player = FindObjectOfType<Camera>().gameObject;
    }
    // Update is called once per frame
    void Update()
    {

        if (player != null)
        {
            gameObject.transform.LookAt(player.transform);
            gameObject.transform.Rotate(Vector3.up * 180);
        }
    }
}
