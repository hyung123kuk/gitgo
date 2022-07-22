using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

public class itemBuyQuestion : MonoBehaviourPun , IPointerClickHandler
{
    public static bool isBuyQuestion = false;
    public GameObject itemBuyWindow;
    public Text itemBuyText;
    public GameObject PortionBuyWindow;
    public InputField PortionNum;
    public Text PortionBuyText;

    public Item item;
    public inventory inven;
    [SerializeField]
    private AllUI allUI;
    [SerializeField]
    private PlayerStat playerStat;

    bool isPortionWindow = false;

    void Start()
    {
        inven = FindObjectOfType<inventory>();
        allUI = FindObjectOfType<AllUI>();
        PlayerStat[] playerStats = FindObjectsOfType<PlayerStat>();
        foreach (PlayerStat myPlayerstat in playerStats)
        {
            if (myPlayerstat.photonView.IsMine)
            {
                playerStat = myPlayerstat;
                break;
            }
        }
    }

    

    void Update()
    {

        if (isBuyQuestion&&Input.GetKeyDown(KeyCode.Return) ){
            BuyItem();
        }

        if (isPortionWindow)
        {
            if (PortionNum.text != "")
            {
                if (int.Parse(PortionNum.text) * (int)item._PRICE > (int)playerStat.MONEY)
                {

                    Debug.Log((int)playerStat.MONEY / (int)item._PRICE);
                    PortionNum.text = ((int)playerStat.MONEY / (int)item._PRICE).ToString();
                }
                else if (int.Parse(PortionNum.text) < 0)
                {
                    PortionNum.text = 0.ToString();
                }
            }
        }

    }
   
    public void BuyQuestionOn(Item _BuyItem)
    {
        if (_BuyItem.itemType == Item.ItemType.Used)
        {
            isPortionWindow = true;
            PortionBuyWindow.SetActive(true);
            PortionBuyText.text = "<color=#9ACD32>" + _BuyItem.itemName + "</color>" + " 구매 개수 : ";
        }
        else
        {
            itemBuyText.text = "<color=#9ACD32>" + _BuyItem.itemName + "</color>" + "을 구매 하시겠습니까?";
            itemBuyWindow.SetActive(true);
        }
        item = _BuyItem;
        isBuyQuestion = true;
        allUI.ItemBuyTop();
    }

    public void BuyQuestionOff()
    {
        PortionBuyWindow.SetActive(false);
        
        itemBuyWindow.SetActive(false);
        isBuyQuestion = false;
        isPortionWindow = false;
    }

    public void BuyItem()
    {
        if (isBuyQuestion == false)
            return;
        isBuyQuestion = false;
        if (item.itemType == Item.ItemType.Equipment)
        {
            if (!inven.HasEmptySlot())
            {
                UiSound.uiSound.BuyfailSound();
                Debug.Log("빈창이 없습니다.");
                LogManager.logManager.Log("빈창이 없습니다", true);
                BuyQuestionOff();
                return;
            }
        }
        else if(item.itemType == Item.ItemType.Used){
            if (!inven.HasEmptySlot()&& !inven.HasSameSlot(item))
            {
                UiSound.uiSound.BuyfailSound();
                Debug.Log("빈창이 없습니다.");
                LogManager.logManager.Log("빈창이 없습니다", true);
                BuyQuestionOff();
                return;
            }
        }
        if(item.itemType == Item.ItemType.Used && int.Parse(PortionNum.text)!=0)
            inven.BuyItem(item, int.Parse(PortionNum.text));
        if (item.itemType == Item.ItemType.Equipment)
            inven.BuyItem(item, 1);
        UiSound.uiSound.BuySound();
        BuyQuestionOff();

     }

    public void OnPointerClick(PointerEventData eventData)
    {
        allUI.ItemBuyTop();
    }
}
