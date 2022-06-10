using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestStore : MonoBehaviour , IPointerClickHandler
{
    [SerializeField]
    public static QuestStore qustore;
    [SerializeField]
    QuestWindow questWindow;
    GameObject questStore;
    QuestTyping questTyping;
    QuestExplain questExplain;
    public Text MainText;
    Image MainImage;
    Text Quest1Text;
    Image Quest1Image;
    Text Quest2Text;
    Image Quest2Image;
    Text Quest3Text;
    Image Quest3Image;
    PlayerStat playerstat;

    public int mainNum = 1;
    public bool isMainRecive = false; //���� ����Ʈ �޾Ҵ°�?
    public bool MainSuccess = false; //���� ����Ʈ ���� �ߴ°�?

     Color MainbasicColor = new Color(0.17f,0.85f,0.16f); //���� �̸� �÷�
     Color MainSelColor= new Color(0.3f,0.6f,0.3f);   //���� �޾����� �̸� �÷�

    Color ButtonSelColor = new Color(0.7f, 0.7f, 0.7f); //����Ʈ �޾����� ��ư �÷�
  
    void Awake()
    {
       questTyping = FindObjectOfType<QuestTyping>();
        questWindow = FindObjectOfType<QuestWindow>();
        questExplain = FindObjectOfType<QuestExplain>();
        MainText = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        MainImage = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        Quest1Text = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
        Quest1Image = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
        Quest2Text = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
        Quest2Image = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<Image>();
        Quest3Text = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>();
        Quest3Image = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetComponent<Image>();
        questStore = transform.GetChild(0).gameObject;
        
         qustore = this;
    }

    private void Start()
    {
        playerstat = FindObjectOfType<PlayerStat>();
    }
    public void QuestStoreLoad()
    {
        if (mainNum == 1)
        {
            MainText.text = questExplain.main1Name;
        }
        else if (mainNum == 2)
        {
            MainText.text = questExplain.main2Name;
        }
        else if (mainNum == 3)
        { 
            MainText.text = questExplain.main3Name;
        }
        else if (mainNum == 4)
        {
            MainText.text = questExplain.main4Name;
        }
        else if (mainNum == 5)
        {
            MainText.text = questExplain.main5Name;
        }
        if (isMainRecive)
        {
            MainText.color = MainSelColor;
            MainImage.color = ButtonSelColor;
        }
        else
        {
            MainText.color = MainbasicColor;
            MainImage.color = Color.white;

        }
    }

    public void storeOn()
    {
        
        questWindow.questWindowOn();
        AllUI.allUI.QuestStroeTop();
        questStore.SetActive(true);
    }
    public void storeOff()
    {
        
        questStore.SetActive(false);
    }

    public void QuestStart()
    {
        UiSound.uiSound.Quest1();
        MainSuccess = false;
        isMainRecive = true;

        MainText.color = MainSelColor;
        MainImage.color = ButtonSelColor;
        questWindow.QuestRecieve(QuestSlot.QuestName.main);
    }
    public bool QuestEnd(Item[] items)
    {
        if (ItemCompensation(items))
        {
            mainNum++;
            isMainRecive = false;
            MainText.color = MainbasicColor;
            MainImage.color = Color.white;
            return true;
        }
        return false;
    }

    public void TypingStart()
    {
        questTyping.Textnum = 1;
        questTyping.QuestStartKey();
    }


    public void MainQuest() //��������Ʈ â ����
    {

        if (mainNum == 1 && !isMainRecive)
        {
            questTyping.mainnum = 1;
            TypingStart();

        }
        else if (mainNum == 1 && isMainRecive)
        {
            QuestExplain.questExplain.Main1(questTyping.main1_item, true, MainSuccess);


        }


        else if (mainNum == 2 && !isMainRecive)  //�����۱��� (3����)
        {
            questTyping.mainnum = 2;
            TypingStart();

        }
        else if (mainNum == 2 && isMainRecive)
        {
            QuestExplain.questExplain.Main2(questTyping.main2_item, true, MainSuccess);
        }


        else if (mainNum == 3 && !isMainRecive) //�� ����, ������ ���
        {
            questTyping.mainnum = 3;
            TypingStart();

        }
        else if (mainNum == 3 && isMainRecive)
        {
            QuestExplain.questExplain.Main3(questTyping.main3_item, true, MainSuccess);
        }


        else if (mainNum == 4 && !isMainRecive) // 7���� �޼�
        {
            questTyping.mainnum = 4;
            TypingStart();

        }
        else if (mainNum == 4 && isMainRecive)
        {
            if (playerstat.Level >= 8)
            {
                MainQuestSuccess(4);
            }
            QuestExplain.questExplain.Main4(questTyping.main4_item, true, MainSuccess);
        }


        else if (mainNum == 5 && !isMainRecive) //�������
        {
            questTyping.mainnum = 5;
            TypingStart();

        }
        else if (mainNum == 5 && isMainRecive)
        {
            QuestExplain.questExplain.Main5(questTyping.main5_item, true, MainSuccess);
        }
        else {
            LogManager.logManager.Log("��� ���� ����Ʈ�� �Ϸ� �Ͽ����ϴ�.", true);
        }


    }

    public void MainQuestSuccess(int SuccessNum)
    {
        if(SuccessNum == mainNum && isMainRecive)
        {
            MainSuccess = true;
            MainQuest();

        }
    }

    public bool ItemCompensation(Item[] items)
    {
        int usedItemCount = 0;
        for(int i = 0; i < items.Length; i++)
        {
                if (items[i].itemType == Item.ItemType.Used && inventory.inven.HasSameSlot(items[i]))
                {
                    usedItemCount++;
                }

        }

        if (!inventory.inven.HasEmptySlot(items.Length - usedItemCount))
        {

            Debug.Log("������ â�� �����ϴ�.");
            LogManager.logManager.Log("��â�� �����ϴ�", true);
            return false;
        }


        foreach (Item item in items)
        {
            if (item.itemType == Item.ItemType.Used)
            {                
                    inventory.inven.addItem(item, 10); 
            }
            else if (item.itemType == Item.ItemType.Equipment)
            {
                inventory.inven.addItem(item);
            }
            else if(item.itemType == Item.ItemType.Ride)
            {
                inventory.inven.addItem(item);
            }

        }
        return true;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        AllUI.allUI.QuestStroeTop();
    }

}
