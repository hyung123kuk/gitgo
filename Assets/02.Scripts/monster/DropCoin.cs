using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCoin : MonoBehaviour
{
    [SerializeField]
    public Transform tr;
    [SerializeField]
    SphereCollider col;
    [SerializeField]
    public Item item;
    [SerializeField]
    private PlayerStat playerStat;
    [SerializeField]
    public float Coin;


    private void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
       
    }

    public void SetCoin(float _coin)
    {
        Coin = (int)_coin*Random.Range(0.8f, 1.2f);
        
    }

    public void SetDropITem()
    {
        tr = GetComponent<Transform>();
        col = GetComponent<SphereCollider>();       
    }

    private void OnTriggerStay(Collider other)
    {
       
        if (other.gameObject.tag == "Player" && item.itemType == Item.ItemType.Used)
        {           
                if (!inventory.inven.HasEmptySlot() && !inventory.inven.HasSameSlot(item))
                {
                    
                    Debug.Log("아이템 창이 없습니다.");
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


}
