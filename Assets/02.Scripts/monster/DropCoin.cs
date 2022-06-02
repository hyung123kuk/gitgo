using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCoin : MonoBehaviour
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
        playerStat = FindObjectOfType<PlayerStat>();
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
       
        if (other.gameObject.tag == "Player" && item.itemType == Item.ItemType.Used)
        {           
                if (!inventory.inven.HasEmptySlot() && !inventory.inven.HasSameSlot(item))
                {
                    
                    Debug.Log("아이템 창이 없습니다.");
                LogManager.logManager.Log("빈창이 없습니다", true);
                return;
                }
                else
                {
                    inventory.inven.addItem(item,1);
                    Destroy(gameObject);
                }           
        }
        if(other.gameObject.tag == "Player"&& item.itemType == Item.ItemType.ETC)
        {            
            playerStat.AddGold(Coin);
            SoundManager.soundManager.GetCoinSound();

            Destroy(gameObject);
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
                Destroy(gameObject, 10f);
            }
        }
    }


}
