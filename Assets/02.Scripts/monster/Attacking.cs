using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public static Attacking attacking;
    public bool isAttacking; //공격한 직후. 공격딜레이
    void Start()
    {
        attacking = this;
    }


    void Update()
    {
        
    }
}
