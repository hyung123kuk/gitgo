using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRotate : MonoBehaviourPun
{

    Camera _camera;
    CapsuleCollider _controller;
    
    PlayerST playerST;
    Weapons weapons;
    public float smoothness = 10f;
    private void Awake()
    {
        if (!photonView.IsMine)
            this.enabled = false;

    }

    void Start()
    {
       
        _camera = Camera.main;
        _controller = this.GetComponent<CapsuleCollider>();
        playerST = GetComponent<PlayerST>();
        weapons = FindObjectOfType<Weapons>();
    }


    void Update()
    {

    }

    void LateUpdate()  //플레이어가 카메라를 바라봄
    {
        if (!photonView.IsMine)
            return;

        if (weapons.isMeteo || AllUI.isUI || playerST.isDie)
            return;

        if (!playerST.HorseMode)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }

    }
}
