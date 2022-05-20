using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class inventory : MonoBehaviour ,IPointerClickHandler,IEndDragHandler
{
    public static inventory inven;
    public static bool iDown; // 인벤토리가 열려있으면 true
    public GameObject Inven; // 인벤토리 창
    static public Slot[] slots;
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

    private void Awake()
    {
        
        playerStat = FindObjectOfType<PlayerStat>();
        slots = SlotsParent.GetComponentsInChildren<Slot>();
        allUI = FindObjectOfType<AllUI>();
        itemStore = FindObjectOfType<itemStore>();
        toolTip = FindObjectOfType<ToolTip>();
        inven = this;
        iDown = false;
        

    }

    private void Start()
    {
        
        GoldUpdate();

        StartCoroutine(invenSet());
        

    }
    IEnumerator invenSet() //playerstat을 슬롯이 받아오질못해 없으면 처음시작할때 인벤을 키지않고 아이템을 먹으면 널리퍼런스가뜸 그러나 인벤을 한번키면 안뜸 이유는 모르겠음.
    {
        invenOn();
        yield return new WaitForSeconds(0.2f);
        invenOff();

    }


    void Update()
    {
        
    }

    public void invenOn()
    {
        Inven.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        allUI.MouseCursor.transform_cursor.gameObject.SetActive(true);
        iDown = true;
        GoldUpdate();
    }

    public void invenOff()
    {
        itemStore.storeOff();
        Inven.SetActive(false);
        allUI.MouseCursor.transform_cursor.gameObject.SetActive(false);
        allUI.MouseCursor.Init_Cursor();
        iDown = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

   
    public void GoldUpdate()
    {
        Gold.text = "Gold : " + playerStat.MONEY.ToString();
    }

    

    public bool HasEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return true;
            }
        }
        
        return false;
    }

    public bool HasSameSlot(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                return true;
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
                if(buyitem == slots[i].item)
                {
                    
                    slots[i].SetSlotCount(num);
                    playerStat.MONEY -= buyitem._PRICE*num;
                    GoldUpdate();
                    return;
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
                if (item == slots[i].item)
                {

                    slots[i].SetSlotCount(num);
                    return;
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
