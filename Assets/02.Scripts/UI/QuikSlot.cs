using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuikSlot : MonoBehaviour
{

    
    [SerializeField]
    public Slot slot;
    [SerializeField]
    public SkillSlot skill;



    void Start()
    {
        
    }

    
    void Update()
    {



        if (slot.item!=null && Input.GetButtonDown(gameObject.tag))
        {
            Debug.Log(slot.item.itemName + " 을 사용했습니다.");
            slot.UseItem();
            slot.SetSlotCount(-1);
        }
    }
}
