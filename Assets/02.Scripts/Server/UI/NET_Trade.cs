using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NET_Trade : MonoBehaviourPun
{
    [SerializeField]
    public GameObject NetTrade_Design;
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


    [SerializeField]
    public GameObject Gold_Window;
    [SerializeField]
    public InputField GoldInput;
    [SerializeField]
    public Text My_goldText;
    [SerializeField]
    public Text Your_goldText;


    [SerializeField]
    GameObject BlockBox; //Ȯ���� ���� ���¿����� �������� �Ҵ�.



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
        if (FindObjectOfType<inventory>().Inven.activeSelf == false)
        {
            FindObjectOfType<inventory>().invenOn();
        }
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
                   
                    net_UIPlayer.TradeItemConnect(j, i, Net_My_Slot[i].itemCount); //������ �ѹ��� ���� �ѹ��� ������.
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

        if (net_UIPlayer.TradeComplete == false)  // Ȯ�ι�ư ����
        {
            net_UIPlayer.TradeComplete = true;
            My_button.sprite = Image_Complete;
            My_TradeItemsWindow.color = Color_Compete;
            BlockBox.SetActive(true);
            if(net_UIPlayer.YourTradeComlete == true)
            {
                SlotNumCheck();
            }

        }
        else  //Ȯ�ι�ư ���
        {
            net_UIPlayer.TradeComplete = false;
            My_button.sprite = Image_NonComplete;
            My_TradeItemsWindow.color = Color.white;
            BlockBox.SetActive(false);

        }

        net_UIPlayer.MyCompleteButton(); // ���ø�Ʈ ���� ����
    }

    public void YourCompleteButton(bool YourComplete) //��� ���ø�Ʈ ���� �ޱ�
    {
        
        if (YourComplete == true) // Ȯ�ι�ư ����
        {
            Your_button.sprite = Image_Complete;
            Your_TradeItemsWindow.color = Color_Compete;

            if (net_UIPlayer.TradeComplete == true)
            {
                SlotNumCheck();
            }
        }
        else //Ȯ�ι�ư ���
        {
            Your_button.sprite = Image_NonComplete;
            Your_TradeItemsWindow.color = Color.white;

        }
    }

    public bool SlotCheck;
    public void SlotNumCheck() // ���� ���� üũ�� �������θ� �����Ѵ�.
    {
        
        SlotCheck = SlotCheckFucn(); //���� ���� üũ
        Debug.Log(SlotCheck);
        net_UIPlayer.SlotNumCheck(SlotCheck); //��뿡�� �� ���� üũ ���� ����.
    }



    public bool SlotCheckFucn() //���� ���� üũ �ϴ� �Լ�
    {
        int allitemcount = 0; //��� �κ��丮�� �� ������ ���� ����

        
        int usedItemCount = 0; // �κ��丮 �ߺ������� ������ ���� ����
        for (int i = 0; i < Net_Your_Slot.Length; i++) 
        {
            if (Net_Your_Slot[i].item != null) //�޴� �� ������ ����
            {
                allitemcount++;
            }

            if ( Net_Your_Slot[i].item != null && Net_Your_Slot[i].item.itemType == Item.ItemType.Used && inventory.inven.HasSameSlot(Net_Your_Slot[i].item)) //������ ���ళ��
            {
                usedItemCount++;
            }

        }


        if (!inventory.inven.HasEmptySlot(allitemcount - usedItemCount)) //�κ��丮 â�� ��������Ȯ���ϴ� �κ�
        {
            LogManager.logManager.Log("��â�� �����ϴ�.", true); 
            return false; //�κ��丮 â�� �����ϸ� false ��ȯ
        }
        return true;
    }




    public void SuccessTrade() // ������ �������� �� �κ��丮�� �������Ѵ�.
    {
        
        for (int i = 0; i < Net_Your_Slot.Length; i++) {
            if (Net_Your_Slot[i].item != null)
            {
                inventory.inven.addItem(Net_Your_Slot[i].item, Net_Your_Slot[i].itemCount);
            }
        }
        net_UIPlayer.GetComponent<PlayerStat>().AddGold(int.Parse(Your_goldText.text));
        TradeReset();
    }

    public void FailTrade() //�� �������� �� �κ��丮�� �����Ը� �ϸ�ȴ�.
    {
        
        for (int i = 0; i < Net_My_Slot.Length; i++)
        {
            if (Net_My_Slot[i].item != null)
            {
                inventory.inven.addItem(Net_My_Slot[i].item, Net_My_Slot[i].itemCount);
            }
        }

        net_UIPlayer.GetComponent<PlayerStat>().AddGold(int.Parse(My_goldText.text));
        TradeReset();
    }

    public void TradeReset()
    {
        Debug.Log(1);
        for (int i = 0; i < Net_My_Slot.Length; i++) {
            Net_My_Slot[i].ClearSlot();
        }
        for(int i = 0; i < Net_Your_Slot.Length; i++)
        {
            Net_Your_Slot[i].ClearSlot();
        }


        NET_UIPlayer.TradeOn = false;
        net_UIPlayer.photonView.RPC("TradeOffSet", RpcTarget.Others);
        net_UIPlayer.TradeComplete = false;
        net_UIPlayer.YourTradeComlete = false;
        BlockBox.SetActive(false);
        net_UIPlayer.TradeResetCheck();
        Your_button.sprite = Image_NonComplete;
        Your_TradeItemsWindow.color = Color.white;
        net_UIPlayer.TradeComplete = false;
        My_button.sprite = Image_NonComplete;
        My_TradeItemsWindow.color = Color.white;

        GoldButtonExit();
        My_goldText.text = 0.ToString();
        Your_goldText.text = 0.ToString();


        Cancle();
        
    }

    public void GoldButton()
    {
        Gold_Window.SetActive(true);        

    }

    private void Update() //��� �ִ밪 ����
    {

        if (Gold_Window.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GoldSetting();
            }
        }

        if (GoldInput.text != "" && GoldInput.text != "-")
        {
            if (int.Parse(GoldInput.text) > net_UIPlayer.GetComponent<PlayerStat>().MONEY)
            {
                GoldInput.text = net_UIPlayer.GetComponent<PlayerStat>().MONEY.ToString();
            }
            else if (int.Parse(GoldInput.text) < 0)
            {
                GoldInput.text = 0.ToString();
            }
        }
    }

    public void GoldButtonExit()
    {
        Gold_Window.SetActive(false);
        GoldInput.text = 0.ToString();
    }

    public void GoldSetting()
    {
        int Gold = int.Parse(My_goldText.text) + int.Parse(GoldInput.text);
        My_goldText.text = Gold.ToString();
        net_UIPlayer.GetComponent<PlayerStat>().AddGold(-int.Parse(GoldInput.text));
        GoldButtonExit();
        net_UIPlayer.GoldSetting(int.Parse(My_goldText.text)); //���濡�� ����尡 ���̰� �Ѵ�.
    }

    public void YourGoldSet(int _Gold)
    {
        Debug.Log(_Gold);
        Your_goldText.text = _Gold.ToString();
    }

}
