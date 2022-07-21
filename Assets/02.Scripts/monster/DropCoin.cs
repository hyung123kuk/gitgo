using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DropCoin : MonoBehaviourPun
{
    [SerializeField]
    public Transform tr;

    [SerializeField]
    public Item item;
    [SerializeField]
    private PlayerStat playerStat;
    [SerializeField]
    public float Coin;
    [SerializeField]
    public CapsuleCollider col;
    [SerializeField]
    private Rigidbody rbody;

    private void Start()
    {

        col = GetComponent<CapsuleCollider>();
        rbody = GetComponent<Rigidbody>();
        

    }
   


    public void SetCoin(float _coin)
    {
        Coin = (int)_coin*Random.Range(0.8f, 1.2f);
        
    }

    public void SetDropITem()
    {
        tr = GetComponent<Transform>();
     
    }

    private void OnTriggerStay(Collider other)
    {
        

        if (other.gameObject.tag == "Player" && item.itemName == "����")
        {

            other.GetComponent<PlayerStat>().AddGold(Coin);
            UiSound.uiSound.GetCoinSound();
            DestroyItem();
        }
        else if (other.gameObject.tag == "Player" && item.itemType == Item.ItemType.Used)
        {           
                if (!inventory.inven.HasEmptySlot() && !inventory.inven.HasSameSlot(item))
                {
                    
                    Debug.Log("������ â�� �����ϴ�.");
                LogManager.logManager.Log("��â�� �����ϴ�", true);
                return;
                }
                else
                {
                    inventory.inven.addItem(item,1);
                LogManager.logManager.Log(item.itemName + "ȹ��");
                     DestroyItem();
            }           
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (item.itemType == Item.ItemType.Used || item.itemType == Item.ItemType.ETC)
        {
            if (other.tag == "ground")
            {
                col.enabled = false;
                rbody.isKinematic = true;
                StartCoroutine(DestroyTIme());
            }
        }
    }

    IEnumerator DestroyTIme()
    {
        yield return new WaitForSeconds(10.0f);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    public void DestroyItem(bool local = true) //ó�������� true�� �ٸ����� false �γѰ���
    {
        if (local)
        {
            photonView.RPC("DestroyItem", RpcTarget.Others, false);
        }
        Destroy(gameObject);
    }


}
