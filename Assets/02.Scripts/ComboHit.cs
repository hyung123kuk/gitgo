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
    public ParticleSystem Attack1;
    public ParticleSystem Attack2;
    public ParticleSystem Attack3;
    public ParticleSystem Attack4;

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
                Invoke("Attack1Eff", 0.2f);
                weapons.StartCoroutine("Swing");
                SoundManager.soundManager.WarriorAttackSound();
                SoundManager.soundManager.WarriorAttackVoice();
            }
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 4);
        }
    }
    void Attack1Eff()
    {
        photonView.RPC("Attack1EffPlay", RpcTarget.All);
    }
    void Attack2Eff()
    {
        photonView.RPC("Attack2EffPlay", RpcTarget.All);
    }
    void Attack3Eff()
    {
        photonView.RPC("Attack3EffPlay", RpcTarget.All);
    }
    void Attack4Eff()
    {
        photonView.RPC("Attack4EffPlay", RpcTarget.All);
    }
    [PunRPC]
    void Attack1EffPlay()
    {
        Attack1.Play();
    }
    [PunRPC]
    void Attack2EffPlay()
    {
        Attack2.Play();
    }
    [PunRPC]
    void Attack3EffPlay()
    {
        Attack3.Play();
    }
    [PunRPC]
    void Attack4EffPlay()
    {
        Attack4.Play();
    }
    public void return1()
    {

        if (noOfClicks >= 2)
        {
            anim.SetBool("isAttack2", true);
            Invoke("Attack2Eff", 0.2f);
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
            Invoke("Attack3Eff", 0.2f);
            Invoke("ThreeAttack", 0.2f);
        }
        else
        {
            anim.SetBool("isAttack2", false);
            anim.SetBool("isAttack", false);
            noOfClicks = 0;
        }
    }
    public void return3()
    {
        if (noOfClicks >= 4)
        {
            SoundManager.soundManager.WarriorAttackVoice2();
            anim.SetBool("isAttack4", true);
            Invoke("Attack4Eff", 0.3f);
            Invoke("ThreeAttack", 0.2f);
        }
        else
        {
            anim.SetBool("isAttack3", false);
            anim.SetBool("isAttack2", false);
            anim.SetBool("isAttack", false);
            noOfClicks = 0;
        }
    }

    public void return4()
    {
        anim.SetBool("isAttack4", false);
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
