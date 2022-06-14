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
    private Canvas QusetExplainWindow;
    [SerializeField]
    private Canvas questWindow;


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
        ExitWindow.isExitMenu = false;
       

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



        if (ExitWindow.isExitMenu ==true)
            return;
        

        if (Input.GetKeyDown(KeyCode.I)) //�κ��丮 �ѱ�/����
        {
            inventory.iDown = !inventory.iDown;
            if (!inventory.iDown) //��
            {

                inventory.inven.invenOff();
                inventory.inven.ToolTIpOff();
                CheckCursorLock();



            }
            else //Ŵ
            {

                inventory.inven.invenOn();
                InvenTop();
                CheckCursorLock();
            }
        }

        if (Input.GetKeyDown(KeyCode.K)) //��ųâ �ѱ�/ ����
        {
            SkillWindow.kDown = !SkillWindow.kDown;
            if (!SkillWindow.kDown) //��
            {
                SkillWindow.skillwindow.SkillWindowOff();
                SkillWindow.skillwindow.SkillToolTipOff();
                CheckCursorLock();



            }
            else //Ŵ
            {
                SkillWindow.skillwindow.SkillWindowOn();
                SkillWindowTop();
                CheckCursorLock();

            }
        }

        if (Input.GetKeyDown(KeyCode.T)) //����â �ѱ�/ ����
        {
            StatWindow.tDown = !StatWindow.tDown;
            if (!StatWindow.tDown) //��
            {
                
                StatWindow.statWindow.StatWindowOff();
                CheckCursorLock();



            }
            else //Ŵ
            {
                StatWindow.statWindow.StatWindowOn();
                StatWindowTop();
                CheckCursorLock();

            }
        }

        if (Input.GetKeyDown(KeyCode.U)) //����â �ѱ�/ ����
        {
            QuestWindow.isQuestWindow = !QuestWindow.isQuestWindow;
            if (!QuestWindow.isQuestWindow) //��
            {

                QuestWindow.qWIndow.questWindowOff();
                CheckCursorLock();



            }
            else //Ŵ
            {
                QuestWindow.qWIndow.questWindowOn();
                QuestWindowTop();
                CheckCursorLock();

            }
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
        
    }

    public void CheckCursorLock()
    {

        if (!inventory.iDown&& !SkillWindow.kDown && !StatWindow.tDown && !QuestExplain.isQuestExplain &&!QuestWindow.isQuestWindow&& !ExitWindow.isExitMenu)
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
