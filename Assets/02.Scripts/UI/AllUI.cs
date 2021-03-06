using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class AllUI : MonoBehaviourPun
{
    [SerializeField]
    private GameObject[] UIWIndow;
    [SerializeField]
    private Canvas inven;
    [SerializeField]
    public Canvas store;
    [SerializeField]
    private Canvas itemBuyQuestion;
    [SerializeField]
    private Canvas itemSellQuestion;
    [SerializeField]
    private Canvas skillWindow;
    [SerializeField]
    private Canvas statWindow;
    [SerializeField]
    private Canvas QuestStoreWindow;
    [SerializeField]
    private Canvas QusetExplainWindow;
    [SerializeField]
    private Canvas questWindow;
    [SerializeField]
    private NET_UIPlayer net_UiPlayer;
    [SerializeField]
    private NET_PartyRecieve net_partyRecieve;
    [SerializeField]
    NET_ChatManager Net_chatManager;

    [SerializeField]
    public MouseCursor MouseCursor;
    [SerializeField]
    public static bool isUI = false;
    public static AllUI allUI;

    public static bool ctrlDown=false; // 버튼 클릭시 상대캐릭터 누를수 있다.

    private void Awake()
    {
        allUI = this;
    }

    private void Start()
    {
        MouseCursor=GetComponent<MouseCursor>();
        net_partyRecieve = FindObjectOfType<NET_PartyRecieve>();
        ExitWindow.isExitMenu = false;
        Net_chatManager = FindObjectOfType<NET_ChatManager>();

    }

    // Update is called once per frame
    void Update()
    {

        

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            

            for (int i = 0; i < UIWIndow.Length; i++)
            {
                if (UIWIndow[i].activeSelf == true)
                    break;

                if (UIWIndow.Length - 1 == i)
                {
                    if (NET_UIPlayer.isMenu)
                    {
                        NET_UIPlayer[] instans = GameObject.FindObjectsOfType<NET_UIPlayer>();
                        foreach (NET_UIPlayer uiplayer in instans) {
                            if (uiplayer.GetComponent<PhotonView>().IsMine)
                            {
                                net_UiPlayer = uiplayer;
                                break;
                            }
                        }
                        net_UiPlayer.DestroyMenu();
                        NET_UIPlayer.isMenu = false;
                        break;
                    }



                    ExitWindow.ExWindow.ExitWindowOn();
                    CheckCursorLock();
                    return;
                }
            }
            
            AllWindowEnd();
            MouseCursor.transform_cursor.gameObject.SetActive(false);
            MouseCursor.SetNormalCursor();
            CheckCursorLock();

        }

        if (Net_chatManager.input.isFocused)
            return;



        if (ExitWindow.isExitMenu ==true)
            return;
        

        if (Input.GetKeyDown(KeyCode.I)) //인벤토리 켜기/끄기
        {
            inventory.iDown = !inventory.iDown;
            if (!inventory.iDown) //끔
            {

                inventory.inven.invenOff();
                inventory.inven.ToolTIpOff();
                CheckCursorLock();



            }
            else //킴
            {

                inventory.inven.invenOn();
                InvenTop();
                CheckCursorLock();
            }
        }

        if (Input.GetKeyDown(KeyCode.K)) //스킬창 켜기/ 끄기
        {
            SkillWindow.kDown = !SkillWindow.kDown;
            if (!SkillWindow.kDown) //끔
            {
                SkillWindow.skillwindow.SkillWindowOff();
                SkillWindow.skillwindow.SkillToolTipOff();
                CheckCursorLock();



            }
            else //킴
            {
                SkillWindow.skillwindow.SkillWindowOn();
                SkillWindowTop();
                CheckCursorLock();

            }
        }

        if (Input.GetKeyDown(KeyCode.T)) //스탯창 켜기/ 끄기
        {
            StatWindow.tDown = !StatWindow.tDown;
            if (!StatWindow.tDown) //끔
            {
                
                StatWindow.statWindow.StatWindowOff();
                CheckCursorLock();



            }
            else //킴
            {
                StatWindow.statWindow.StatWindowOn();
                StatWindowTop();
                CheckCursorLock();

            }
        }

        if (Input.GetKeyDown(KeyCode.U)) //스탯창 켜기/ 끄기
        {
            QuestWindow.isQuestWindow = !QuestWindow.isQuestWindow;
            if (!QuestWindow.isQuestWindow) //끔
            {

                QuestWindow.qWIndow.questWindowOff();
                CheckCursorLock();



            }
            else //킴
            {
                QuestWindow.qWIndow.questWindowOn();
                QuestWindowTop();
                CheckCursorLock();

            }
        }
        if (Input.GetKey(KeyCode.LeftControl)) //Ctrl 누르면 마우스 커서 나온다. 상대방 클릭가능
        {
            ctrlDown = true;
            CheckCursorLock();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            ctrlDown = false;
            CheckCursorLock();
        }

    }

    public void AllWindowEnd()
    {
        for (int i = 0; i < UIWIndow.Length; i++)
        {
            UIWIndow[i].SetActive(false);
        }
        inventory.iDown = false;
        SkillWindow.kDown = false;
        StatWindow.tDown = false;
        itemStore.sellButton = false;
        QuestExplain.isQuestExplain = false;
        QuestWindow.isQuestWindow = false;
        ExitWindow.isExitMenu = false;
        NET_UIPlayer.isMenu = false;
        
        NET_TradeRecieve.isNET_Trade_ReicieveWindow = false;
        NET_Trade.isNET_Trade_Window = false;
        NET_STAT.isNet_Stat = false;
        NET_PartyRecieve.ispartyRecieveWindow = false;

        net_partyRecieve.PartyRecieveWindowOff();

        if (NET_UIPlayer.TradeOn)
        {
            FindObjectOfType<NET_Trade>().FailTrade();
        }

        
    }

    public void CheckCursorLock()
    {

        if (!inventory.iDown&& !SkillWindow.kDown && !StatWindow.tDown && !QuestExplain.isQuestExplain &&!QuestWindow.isQuestWindow&& !ExitWindow.isExitMenu && !ctrlDown &&!NET_UIPlayer.isMenu && !NET_TradeRecieve.isNET_Trade_ReicieveWindow && !NET_Trade.isNET_Trade_Window && !NET_STAT.isNet_Stat && !NET_PartyRecieve.ispartyRecieveWindow)
        {
            isUI = false;
            Cursor.lockState = CursorLockMode.Locked;
            allUI.MouseCursor.transform_cursor.gameObject.SetActive(false); 
            allUI.MouseCursor.Init_Cursor();
        }
        else
        { 
            isUI = true;
            Cursor.lockState = CursorLockMode.Confined;
            allUI.MouseCursor.transform_cursor.gameObject.SetActive(true);
        }
    }

    public void InvenTop()
    {
        inven.sortingOrder = 1;
        store.sortingOrder--;
        itemBuyQuestion.sortingOrder--;
        itemSellQuestion.sortingOrder--;
        skillWindow.sortingOrder--;
        statWindow.sortingOrder--;
        QuestStoreWindow.sortingOrder--;
        QusetExplainWindow.sortingOrder--;
        questWindow.sortingOrder--;
    }
    public void StoreTop()
    {
        inven.sortingOrder --;
        store.sortingOrder =1;
        itemBuyQuestion.sortingOrder--;
        itemSellQuestion.sortingOrder--;
        skillWindow.sortingOrder--;
        statWindow.sortingOrder--;
        QuestStoreWindow.sortingOrder--;
        QusetExplainWindow.sortingOrder--;
        questWindow.sortingOrder--;
    }
    public void ItemBuyTop()
    {
        inven.sortingOrder--;
        store.sortingOrder--;
        itemBuyQuestion.sortingOrder=1;
        itemSellQuestion.sortingOrder--;
        skillWindow.sortingOrder--;
        statWindow.sortingOrder--;
        QuestStoreWindow.sortingOrder--;
        QusetExplainWindow.sortingOrder--;
        questWindow.sortingOrder--;
    }
    public void ItemSellTop()
    {
        inven.sortingOrder--;
        store.sortingOrder--;
        itemBuyQuestion.sortingOrder --;
        itemSellQuestion.sortingOrder=1;
        skillWindow.sortingOrder--;
        statWindow.sortingOrder--;
        QuestStoreWindow.sortingOrder--;
        QusetExplainWindow.sortingOrder--;
        questWindow.sortingOrder--;
    }
    public void SkillWindowTop()
    {
        inven.sortingOrder--;
        store.sortingOrder--;
        itemBuyQuestion.sortingOrder--;
        itemSellQuestion.sortingOrder --;
        skillWindow.sortingOrder=1;
        statWindow.sortingOrder--;
        QuestStoreWindow.sortingOrder--;
        QusetExplainWindow.sortingOrder--;
        questWindow.sortingOrder--;
    }

    public void StatWindowTop()
    {
        inven.sortingOrder--;
        store.sortingOrder--;
        itemBuyQuestion.sortingOrder--;
        itemSellQuestion.sortingOrder--;
        skillWindow.sortingOrder--;
        statWindow.sortingOrder=1;
        QuestStoreWindow.sortingOrder--;
        QusetExplainWindow.sortingOrder--;
        questWindow.sortingOrder--;
    }

    public void QuestStroeTop()
    {
        inven.sortingOrder--;
        store.sortingOrder--;
        itemBuyQuestion.sortingOrder--;
        itemSellQuestion.sortingOrder--;
        skillWindow.sortingOrder--;
        statWindow.sortingOrder --;
        QuestStoreWindow.sortingOrder=1;
        QusetExplainWindow.sortingOrder=2;
        questWindow.sortingOrder--;

    }
    public void QuestExplainTop()
    {
        inven.sortingOrder--;
        store.sortingOrder--;
        itemBuyQuestion.sortingOrder--;
        itemSellQuestion.sortingOrder--;
        skillWindow.sortingOrder--;
        statWindow.sortingOrder--;
        QuestStoreWindow.sortingOrder--;
        QusetExplainWindow.sortingOrder=1;
        questWindow.sortingOrder--;

    }
    public void QuestWindowTop()
    {
        inven.sortingOrder--;
        store.sortingOrder--;
        itemBuyQuestion.sortingOrder--;
        itemSellQuestion.sortingOrder--;
        skillWindow.sortingOrder--;
        statWindow.sortingOrder--;
        QuestStoreWindow.sortingOrder--;
        QusetExplainWindow.sortingOrder=2;
        questWindow.sortingOrder=1;

    }


}
