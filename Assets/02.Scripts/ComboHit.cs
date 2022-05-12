using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboHit : MonoBehaviour
{
    public Animator anim;
    public int noOfClicks = 0; //클릭수
    float lastClickdTime = 0; //마지막 클릭시간
    public float maxComboDelay; //콤보사이 시간

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }


    void Update()
    {
        if (inventory.iDown || NPC.isNPCRange)
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
            }
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        }
    }
    public void return1()
    {

        if (noOfClicks >= 2)
        {
            anim.SetBool("isAttack2", true);
           
            
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
            anim.SetBool("isAttack3", true);
            
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
}
