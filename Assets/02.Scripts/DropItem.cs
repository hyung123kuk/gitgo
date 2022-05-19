using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    private Transform tr;
    [SerializeField]
    SphereCollider col;
    [SerializeField]
    private Text RangeText;
    [SerializeField]
    private Item item;
    

    public float RotateSpeed=2f;

    void Start()
    {
        tr=GetComponent<Transform>();
        col = GetComponent<SphereCollider>();
        RangeText = GameObject.FindGameObjectWithTag("RangeText").GetComponent<Text>();
    }


    private void FixedUpdate()
    {
        tr.Rotate(Vector3.up * RotateSpeed,Space.World);
    }

   

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("hi");
        if (other.gameObject.tag == "Player")
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
