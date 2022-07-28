using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNameRo : MonoBehaviour
{
    public GameObject player;
    public Camera ca;

    private void Start()
    {
        PlayerST[] playerSts = FindObjectsOfType<PlayerST>();
        foreach (PlayerST myPlayerst in playerSts)
        {
            if (myPlayerst.photonView.IsMine)
            {
                ca = myPlayerst.GetComponentInChildren<Camera>();
                break;
            }
        }

    
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
