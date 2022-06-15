using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ComboHit : MonoBehaviourPun
{
    public Animator anim;
    public int noOfClicks = 0; //Ŭ����
    float lastClickdTime = 0; //������ Ŭ���ð�
    public float maxComboDelay; //�޺����� �ð�
    Weapons weapons;

    private void Awake()
    {
        if (!photonView.IsMine)
            this.enabled = false;
    }

    void Start()
    {
       
        weapons = FindObjectOfType<Weapons>();
        anim = gameObject.GetComponent<Animator>();
    }


    void Update()
    {
      

        if (AllUI.isUI || NPC.isNPCRange)
            return;
        if (Time.time - lastClickdTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            lastClickdTime = Time.time;
            noOfClicks++;
            if (noOfClicks == 1)
            {
                anim.SetBool("isAttack", true);
                weapons.StartCoroutine("Swing");
                SoundManager.soundManager.WarriorAttackSound();
                SoundManager.soundManager.WarriorAttackVoice();
            }
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        }
    }
    public void return1()
    {

        if (noOfClicks >= 2)
        {
            anim.SetBool("isAttack2", true);
            weapons.StartCoroutine("Swing");
            SoundManager.soundManager.WarriorAttackSound();
            SoundManager.soundManager.WarriorAttackVoice();
        }
        else
        {
            anim.SetBool("isAttack", false);
            noOfClicks = 0;
        }
    }
    public void return2()
    {
        if (noOfClicks >= 3)
        {
            SoundManager.soundManager.WarriorAttackVoice2();
            anim.SetBool("isAttack3", true);
            Invoke("ThreeAttack", 0.2f);
        }
        else
        {
            anim.SetBool("isAttack2", false);
            noOfClicks = 0;
        }
    }
    public void return3()
    {
        anim.SetBool("isAttack3", false);
        anim.SetBool("isAttack2", false);
        anim.SetBool("isAttack", false);
        noOfClicks = 0;
    }
    void ThreeAttack()
    {
        SoundManager.soundManager.WarriorAttackSound();
        
        weapons.StartCoroutine("Swing");
    }
}
