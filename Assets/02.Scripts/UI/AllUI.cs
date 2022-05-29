using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllUI : MonoBehaviour
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
    public MouseCursor MouseCursor;
    [SerializeField]
    public static bool isUI = false;
    public static AllUI allUI;

    private void Awake()
    {
        allUI = this;
    }

    private void Start()
    {
        MouseCursor=GetComponent<MouseCursor>();
      
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for(int i = 0; i < UIWIndow.Length; i++)
            {
                
               UIWIndow[i].SetActive(false);
            }
            inventory.iDown = false;
            SkillWindow.kDown = false;
            StatWindow.tDown = false;
            itemStore.sellButton = false;
            QuestExplain.isQuestExplain = false;
            MouseCursor.transform_cursor.gameObject.SetActive(false);
            MouseCursor.SetNormalCursor();
            CheckCursorLock();

        }

        if (Input.GetKeyDown(KeyCode.I)) //ÀÎº¥Åä¸® ÄÑ±â/²ô±â
        {
            inventory.iDown = !inventory.iDown;
            if (!inventory.iDown) //²û
            {

                inventory.inven.invenOff();
                inventory.inven.ToolTIpOff();
                CheckCursorLock();



            }
            else //Å´
            {

                inventory.inven.invenOn();
                InvenTop();
                CheckCursorLock();
            }
        }

        if (Input.GetKeyDown(KeyCode.K)) //½ºÅ³Ã¢ ÄÑ±â/ ²ô±â
        {
            SkillWindow.kDown = !SkillWindow.kDown;
            if (!SkillWindow.kDown) //²û
            {
                SkillWindow.skillwindow.SkillWindowOff();
                SkillWindow.skillwindow.SkillToolTipOff();
                CheckCursorLock();



            }
            else //Å´
            {
                SkillWindow.skillwindow.SkillWindowOn();
                SkillWindowTop();
                CheckCursorLock();

            }
        }

        if (Input.GetKeyDown(KeyCode.T)) //½ºÅÈÃ¢ ÄÑ±â/ ²ô±â
        {
            StatWindow.tDown = !StatWindow.tDown;
            if (!StatWindow.tDown) //²û
            {
                
                StatWindow.statWindow.StatWindowOff();
                CheckCursorLock();



            }
            else //Å´
            {
                StatWindow.statWindow.StatWindowOn();
                StatWindowTop();
                CheckCursorLock();

            }
        }


    }


    public void CheckCursorLock()
    {

        if (inventory.iDown == false && SkillWindow.kDown == false && StatWindow.tDown == false && QuestExplain.isQuestExplain == false)
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

    }


}
