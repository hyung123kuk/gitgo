using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : DropCoin
{

    [SerializeField]
    private Text RangeText;

    public float RotateSpeed = 2f;

    void Start()
    {
        SetDropITem();
        RangeText = GameObject.FindGameObjectWithTag("RangeText").GetComponent<Text>();
    }



    public void FixedUpdate()
    {
        tr.Rotate(Vector3.up * RotateSpeed, Space.World);
    }

    private void OnTriggerStay(Collider other)
    {
  
        if (other.gameObject.tag == "Player" && item.itemType==Item.ItemType.Equipment)
        {
            RangeText.enabled = true;
            RangeText.text = "ZŰ�� ������ �������� �ݽ��ϴ�.";
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (!inventory.inven.HasEmptySlot())
                {
                    Debug.Log("������ â�� �����ϴ�.");
                    return;
                }
                else
                {
                  
                    RangeText.enabled = false;
                    inventory.inven.addItem(item);
                    Destroy(gameObject);
                }


            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

            RangeText.enabled = false;
        }
    }

}
