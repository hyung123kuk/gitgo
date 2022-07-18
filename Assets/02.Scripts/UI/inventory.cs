using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class inventory : MonoBehaviourPun, IPointerClickHandler, IEndDragHandler
{
    int invencheck = 0; //인벤 처음 열릴때 소리안나게하기위함
    public static inventory inven;
    public static bool iDown; // 인벤토리가 열려있으면 true
    public GameObject Inven; // 인벤토리 창
    static public Slot[] slots;
    public Slot[] eqslots;
    [SerializeField]
    private GameObject SlotsParent;
    public Text Gold;
    private PlayerStat playerStat;
    [SerializeField]
    private AllUI allUI;
    [SerializeField]
    private itemStore itemStore;
    [SerializeField]
    private ToolTip toolTip;
    [SerializeField]
    private GameObject LoadingUI; //로딩창 인벤껐다킬때 가려주는용도 


    public Slot[] GetSlots() { return slots; }
    public Slot[] GetEqSlots() { return eqslots; }
    [SerializeField]
    Item[] items;

    public void LoadToInven(int _arrNum, string _itemName, int _itemCount)
    {
        for(int i = 0; i < items.Length; i++)
        { 
            if(items[i].itemName == _itemName)
            {
                slots[_arrNum].AddItem(items[i], _itemCount);
            }

            
        }

    }

    public void LoadToEq(int _arrNum, string _itemName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == _itemName)
            {
                eqslots[_arrNum].AddItem(items[i]);
 
            }

        }
    }

    private void Awake()
    {

        //LoadingUI = GameObject.Find("UIENd").transform.GetChild(6).gameObject;

       
        
        allUI = FindObjectOfType<AllUI>();
        itemStore = FindObjectOfType<itemStore>();
        toolTip = FindObjectOfType<ToolTip>();
        inven = this;
        iDown = false;
        StartCoroutine(invenSet());
        
        StartCoroutine(LoadingSet());
        Inven.SetActive(true);
        slots = SlotsParent.GetComponentsInChildren<Slot>();
        eqslots = GameObject.FindGameObjectWithTag("EqueSlot").GetComponentsInChildren<Slot>();
       

    }

    private void Start()
    {

        if (playerStat == null)
        {
            PlayerStat[] playerStats = GameObject.FindObjectsOfType<PlayerStat>();


            foreach (PlayerStat myplayerStat in playerStats)
            {
                if (myplayerStat.GetComponent<PhotonView>().IsMine)
                {
                    playerStat = myplayerStat;
                    break;
                }
            }
        }
        GoldUpdate();

       
        

    }
    IEnumerator invenSet() //playerstat을 슬롯이 받아오질못해 없으면 처음시작할때 인벤을 키지않고 아이템을 먹으면 널리퍼런스가뜸 그러나 인벤을 한번키면 안뜸 이유는 모르겠음.
    {
        invenOn();
        yield return new WaitForSeconds(0.1f);
        invenOff();
        invencheck = 1;

    }
    IEnumerator LoadingSet() 
    {
        yield return new WaitForSeconds(2f);
        //LoadingUI.SetActive(false);

    }



    void Update()
    {
        
    }

    public void invenOn()
    {
        if (invencheck == 1)
        {
            UiSound.uiSound.InventoryOpenSound();

            Inven.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            allUI.MouseCursor.transform_cursor.gameObject.SetActive(true);
            iDown = true;
            GoldUpdate();
        }
    }

    public void invenOff()
    {
        if (invencheck == 1)
            UiSound.uiSound.InventoryCloseSound();

        if(itemStore)
            itemStore.storeOff();
        Inven.SetActive(false);
        
        toolTip.ToolTipOff();
        iDown = false;

        if (NET_UIPlayer.TradeOn)
        {
            FindObjectOfType<NET_Trade>().FailTrade();
        }

        AllUI.allUI.CheckCursorLock();
        
       
    }

   
    public void GoldUpdate()
    {

        if (playerStat == null)
        {
            PlayerStat[] playerStats = GameObject.FindObjectsOfType<PlayerStat>();


            foreach (PlayerStat myplayerStat in playerStats)
            {
                if (myplayerStat.GetComponent<PhotonView>().IsMine)
                {
                    playerStat = myplayerStat;
                    break;
                }
            }
        }

        playerStat.MONEY = (int)playerStat.MONEY;
        Gold.text = "Gold : " + playerStat.MONEY.ToString();
    }

    

    public bool HasEmptySlot(int num=1)
    {
        int _num = 1;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                
                if (_num >= num)
                {
                    return true;
                }
                _num++;

            }
        }
        
        return false;
    }

    public bool HasSameSlot(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (slots[i].item.itemName == item.itemName)
                {
                    return true;
                }
            }
        }
       
        return false;
    }

    public void BuyItem(Item buyitem,int num=0)
    {
       
        if (buyitem.itemType == Item.ItemType.Used)
        {
            
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (buyitem.itemName == slots[i].item.itemName)
                    {

                        slots[i].SetSlotCount(num);
                        playerStat.MONEY -= buyitem._PRICE * num;
                        GoldUpdate();
                        return;
                    }
                }
            }
        }
       

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                if (buyitem.itemType == Item.ItemType.Used)
                {
                    slots[i].SetSlotCount(num);
                    playerStat.MONEY -= buyitem._PRICE*num;
                }
                else
                {
                    playerStat.MONEY -= buyitem._PRICE;
                }
                slots[i].item = buyitem;
                slots[i].itemImage.sprite = slots[i].item.itemImage;
                slots[i].SetColor(1);
                slots[i].ItemLimitColorRed();
                
                GoldUpdate();
                return;
            }

        }
    }
    public void SellItem(Slot sellitem, int num = 0)
    {
        if (sellitem.item.itemType == Item.ItemType.Used)
        {
            playerStat.MONEY += Mathf.Round(sellitem.item._PRICE * 0.66f) * num;
            sellitem.SetSlotCount(-num);
        }
        else
        {
            playerStat.MONEY += Mathf.Round(sellitem.item._PRICE * 0.66f);
            sellitem.item = null;
            sellitem.SetColor(0);
        }
       
        
        GoldUpdate();
       
    }

    public void addItem(Item item, int num = 0) //같은 아이템이 있거나 빈창이있을때 넣는다.
    {

        if (item.itemType == Item.ItemType.Used)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (item.name == slots[i].item.name)
                    {

                        slots[i].SetSlotCount(num);
                        return;
                    }
                }
            }
        }


        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                if (item.itemType == Item.ItemType.Used)
                {
                    slots[i].SetSlotCount(num);
                  
                }

                slots[i].item = item;
                slots[i].itemImage.sprite = slots[i].item.itemImage;
                slots[i].SetColor(1);
                slots[i].ItemLimitColorRed();
                return;
            }

        }
    }

    public void AllSlotItemLimitColor()
    {
        for (int i = 0; i < slots.Length; i++)
        { if (slots[i].item!=null)
                slots[i].ItemLimitColorRed(); }
    }

    public void ToolTIpOff()
    {
        toolTip.ToolTipOff();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        allUI.InvenTop();
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        allUI.InvenTop();
    }

}
