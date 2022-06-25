using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NET_Trade : MonoBehaviourPun
{
    [SerializeField]
    GameObject NetTrade_Design;
    [SerializeField]
    AllUI allUI;
    [SerializeField]
    NET_UIPlayer net_UIPlayer;
    [SerializeField]
    NET_TradeRecieve tradeRecieve;

    [SerializeField]
    public Slot[] Net_My_Slot;
    [SerializeField]
    public NET_Trade_itemSlot[] Net_Your_Slot;
    [SerializeField]
    public Item[] items;



    [SerializeField]
    public Image My_button;
    [SerializeField]
    public Image My_TradeItemsWindow;
    [SerializeField]
    public Image Your_button;
    [SerializeField]
    public Image Your_TradeItemsWindow;
    


    [SerializeField]
    public Sprite Image_Complete;
    [SerializeField]
    public Sprite Image_NonComplete;
    [SerializeField]
    public Color Color_Compete = new Color(1f, 0.4f, 0f, 1f);


    public static bool isNET_Trade_Window;

    private void Start()
    {
        tradeRecieve = FindObjectOfType<NET_TradeRecieve>();
        allUI = FindObjectOfType<AllUI>();
        NET_UIPlayer[] net_uiplayes = GameObject.FindObjectsOfType<NET_UIPlayer>();


        foreach (NET_UIPlayer myUIPlaye in net_uiplayes)
        {
            if (myUIPlaye.GetComponent<PhotonView>().IsMine)
            {
                net_UIPlayer = myUIPlaye;
                break;
            }
        }

    }

    public void Net_TradeOn()
    {
        isNET_Trade_Window = true;
        NetTrade_Design.SetActive(true);
        tradeRecieve.Cancle();
        
        
        allUI.CheckCursorLock();
    }

    public void Cancle()
    {
        NetTrade_Design.SetActive(false);
        isNET_Trade_Window = false;
        allUI.CheckCursorLock();
    }

    public void RaiseIteam()
    {
        for (int i = 0; i < Net_My_Slot.Length; i++)
        {
            

            if (Net_My_Slot[i].item == null)
            {
                
                net_UIPlayer.TradeItemConnect(-1,i,0);
                continue;
            }

         

            for (int j = 0; j < items.Length; j++)
            {


                if (items[j].name == Net_My_Slot[i].item.name)
                {
                   
                    net_UIPlayer.TradeItemConnect(j, i, Net_My_Slot[i].itemCount); //아이템 넘버와 슬롯 넘버를 보낸다.
                    break;
                }

            }
            
        }
    }

   


    public void RecieveItem(int itemNUum , int slotNum, int itemCount)
    {
        if (itemNUum == -1)
        {
            Net_Your_Slot[slotNum].SetItem(null, itemCount);
        }
        else
        {
            Debug.Log("1");
            Net_Your_Slot[slotNum].SetItem(items[itemNUum], itemCount);
        }
    }

    public void MyCompleteButton()
    {

        if (net_UIPlayer.TradeComplete == false)  // 확인버튼 누름
        {
            net_UIPlayer.TradeComplete = true;
            My_button.sprite = Image_Complete;
            My_TradeItemsWindow.color = Color_Compete;
            if(net_UIPlayer.YourTradeComlete == true)
            {
                SlotNumCheck();
            }

        }
        else  //확인버튼 취소
        {
            net_UIPlayer.TradeComplete = false;
            My_button.sprite = Image_NonComplete;
            My_TradeItemsWindow.color = Color.white;

        }

        net_UIPlayer.MyCompleteButton(); // 컴플리트 상태 전달
    }

    public void YourCompleteButton(bool YourComplete) //상대 컴플리트 상태 받기
    {
        
        if (YourComplete == true) // 확인버튼 누름
        {
            Your_button.sprite = Image_Complete;
            Your_TradeItemsWindow.color = Color_Compete;

            if (net_UIPlayer.TradeComplete == true)
            {
                SlotNumCheck();
            }
        }
        else //확인버튼 취소
        {
            Your_button.sprite = Image_NonComplete;
            Your_TradeItemsWindow.color = Color.white;

        }
    }

    public bool SlotCheck;
    public void SlotNumCheck() // 슬롯 개수 체크해 성공여부를 결정한다.
    {
        
        SlotCheck = SlotCheckFucn(); //슬롯 갯수 체크
        Debug.Log(SlotCheck);
        net_UIPlayer.SlotNumCheck(SlotCheck); //상대에게 내 슬롯 체크 여부 보냄.
    }



    public bool SlotCheckFucn() //슬롯 개수 체크 하는 함수
    {
        int allitemcount = 0; //상대 인벤토리의 총 아이템 갯수 세기

        
        int usedItemCount = 0; // 인벤토리 중복아이템 있으면 갯수 빼기
        for (int i = 0; i < Net_Your_Slot.Length; i++) 
        {
            if (Net_Your_Slot[i].item != null) //받는 총 아이템 갯수
            {
                allitemcount++;
            }

            if ( Net_Your_Slot[i].item != null && Net_Your_Slot[i].item.itemType == Item.ItemType.Used && inventory.inven.HasSameSlot(Net_Your_Slot[i].item)) //동일한 물약개수
            {
                usedItemCount++;
            }

        }


        if (!inventory.inven.HasEmptySlot(allitemcount - usedItemCount)) //인벤토리 창이 부족한지확인하는 부분
        {
            LogManager.logManager.Log("빈창이 없습니다.", true); 
            return false; //인벤토리 창이 부족하면 false 반환
        }
        return true;
    }




    public void SuccessTrade() // 상대방의 아이템은 내 인벤토리에 들어오게한다.
    {
        Debug.Log("Success");
        for (int i = 0; i < Net_Your_Slot.Length; i++) {
            if (Net_Your_Slot[i].item != null)
            {
                inventory.inven.addItem(Net_Your_Slot[i].item, Net_Your_Slot[i].itemCount);
            }
        }

        TradeReset();
    }

    public void FailTrade() //내 아이템이 내 인벤토리에 들어오게만 하면된다.
    {
        for (int i = 0; i < Net_My_Slot.Length; i++)
        {
            if (Net_My_Slot[i].item != null)
            {
                inventory.inven.addItem(Net_My_Slot[i].item, Net_My_Slot[i].itemCount);
            }
        }


        TradeReset();
    }

    public void TradeReset()
    {
        for (int i = 0; i < Net_My_Slot.Length; i++) {
            Net_My_Slot[i].ClearSlot();
        }
        for(int i = 0; i < Net_Your_Slot.Length; i++)
        {
            Net_Your_Slot[i].ClearSlot();
        }

        isNET_Trade_Window = false;
        NET_UIPlayer.TradeOn = false;
        net_UIPlayer.photonView.RPC("TradeOffSet", RpcTarget.Others);
        net_UIPlayer.TradeComplete = false;
        net_UIPlayer.YourTradeComlete = false;
        Your_button.sprite = Image_NonComplete;
        Your_TradeItemsWindow.color = Color.white;
        net_UIPlayer.TradeComplete = false;
        My_button.sprite = Image_NonComplete;
        My_TradeItemsWindow.color = Color.white;
        allUI.CheckCursorLock();
       
    }

}
