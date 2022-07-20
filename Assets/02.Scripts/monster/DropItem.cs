using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
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
  
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PhotonView>().IsMine && item.itemType==Item.ItemType.Equipment)
        {
            RangeText.enabled = true;
            RangeText.text = "ZŰ�� ������ �������� �ݽ��ϴ�.";
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (!inventory.inven.HasEmptySlot())
                {
                    Debug.Log("������ â�� �����ϴ�.");
                    LogManager.logManager.Log("��â�� �����ϴ�", true);
                    return;
                }
                else
                {

                    RangeText.enabled = false;
                    inventory.inven.addItem(item);
                   
                    photonView.RPC("DestroyEquip",RpcTarget.All);
                    
                    
                }


            }
        }
    }

    [PunRPC]
    public void DestroyEquip()
    {
        Destroy(gameObject);
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

            RangeText.enabled = false;
        }
    }

}
