using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class itemSellQuestion : MonoBehaviour ,IPointerClickHandler
{
    public static bool isSellQuestion = false;
    public GameObject itemSellWindow;
    public Text itemSellText;
    public GameObject PortionSellWindow;
    public InputField PortionNum;
    public Text PortionSellText;

    public GameObject itemSellScope;
   
    public inventory inven;
    public Slot SellSlot;
    [SerializeField]
    private AllUI allUI;
    

    bool isPortionWindow=false;

    void Start()
    {
        inven = FindObjectOfType<inventory>();
        allUI = FindObjectOfType<AllUI>();
       
    }

   
    void Update()
    {
        if (isSellQuestion && Input.GetKeyDown(KeyCode.Return))
        {
            SellItem();
        }
        if(isPortionWindow)
        {
            if (PortionNum.text != "")
            {
                
                if (int.Parse(PortionNum.text) > SellSlot.itemCount)
                {
                    PortionNum.text = SellSlot.itemCount.ToString();
                }
                else if (int.Parse(PortionNum.text) < 0)
                {
                    PortionNum.text = 0.ToString();
                }
            }
        }
    }

    public void SellQuestionOn(Slot _SellSlot)
    {
        allUI.ItemSellTop();
        isSellQuestion = true;
        if (_SellSlot.item.itemType == Item.ItemType.Used)
        {
            PortionSellWindow.SetActive(true);
            if (_SellSlot.itemCount > 99)
            {
                PortionNum.text = 99.ToString();
            }
            else
            {
                PortionNum.text = _SellSlot.itemCount.ToString();
            }
            isPortionWindow = true;
            PortionSellText.text = "<color=#9ACD32>" + _SellSlot.item.itemName + "</color>" + " 판매 개수 : ";
            
        }
        else {
            itemSellText.text = "<color=#9ACD32>"+_SellSlot.item.itemName + "</color>" + "을 판매 하시겠습니까?";
            itemSellWindow.SetActive(true);
        }
        
        SellSlot = _SellSlot;
    }

    public void SellQuestionOff()
    {
        if (PortionSellWindow)
        {
            PortionSellWindow.SetActive(false);
            isPortionWindow = false;
        }
        if(itemSellWindow)
        {
            itemSellWindow.SetActive(false);
          }
        
              
    }

    public void SellItem()
    {
        if (SellSlot.item.itemType == Item.ItemType.Used)
        {
            
            inven.SellItem(SellSlot, int.Parse(PortionNum.text));
        }
        else
        {            
            inven.SellItem(SellSlot);
        }
        isSellQuestion = false;
        itemSellScope.SetActive(false);
        SellQuestionOff();
        SellSlot.tooltip.toolTip.gameObject.SetActive(false);
        SellSlot = null;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        allUI.ItemSellTop();
    }
}
