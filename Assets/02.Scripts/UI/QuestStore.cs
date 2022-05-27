using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestStore : MonoBehaviour , IPointerClickHandler
{
    GameObject questStore;
    QuestTyping questTyping;
    Text MainText;
    Image MainImage;
    Text Quest1Text;
    Image Quest1Image;
    Text Quest2Text;
    Image Quest2Image;
    Text Quest3Text;
    Image Quest3Image;

    int mainNum = 1;
    public bool isMainRecive = false; //���� ����Ʈ �޾Ҵ°�?
    public bool MainSuccess = false; //���� ����Ʈ ���� �ߴ°�?

     Color MainbasicColor = new Color(0.17f,0.85f,0.16f); //���� �̸� �÷�
     Color MainSelColor= new Color(0.3f,0.6f,0.3f);   //���� �޾����� �̸� �÷�

     Color QuestSelColor = new Color(0.3f, 0.3f, 0.3f); //����Ʈ �޾����� �̸� �÷�
     
     Color ButtonSelColor = new Color(0.7f, 0.7f, 0.7f); //����Ʈ �޾����� ��ư �÷�
    void Awake()
    {
       questTyping = FindObjectOfType<QuestTyping>();
        MainText = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        MainImage = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        Quest1Text = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
        Quest1Image = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
        Quest2Text = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
        Quest2Image = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<Image>();
        Quest3Text = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>();
        Quest3Image = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetComponent<Image>();
        questStore = transform.GetChild(0).gameObject;

    }

    public void storeOn()
    {
        AllUI.allUI.QuestStroeTop();
        questStore.SetActive(true);
    }

    public void MainQuest()
    {

            if (mainNum == 1 && !isMainRecive) 
            {
                questTyping.mainnum = 1;
                questTyping.Textnum = 1;
                questTyping.QuestStartKey();
                MainSuccess = false;
                isMainRecive = true;

                MainText.color = MainSelColor;
                MainImage.color = ButtonSelColor;
            }
            else if(mainNum ==1 && MainSuccess)
            {
            if (ItemCompensation(questTyping.main1_item))
            {
                mainNum++;
                isMainRecive = false;
                MainText.color = MainbasicColor;
                MainImage.color = Color.white;
            }
               
            }


            else if (mainNum == 2 && !isMainRecive)  //�����۱��� (3����)
            {
                questTyping.mainnum = 2;
                questTyping.Textnum = 1;
                questTyping.QuestStartKey();
                MainSuccess = false;
                isMainRecive = true;

                MainText.color = MainSelColor;
                MainImage.color = ButtonSelColor;

            }
        else if (mainNum == 2 && MainSuccess)
        {
            if (ItemCompensation(questTyping.main1_item))
            {
                mainNum++;
                isMainRecive = false;
                MainText.color = MainbasicColor;
                MainImage.color = Color.white;

            }
        }
        else if (mainNum == 3 && !isMainRecive) //�� ����, ������ ���
            {
                questTyping.mainnum = 3;
                questTyping.Textnum = 1;
                questTyping.QuestStartKey();
                MainSuccess = false;
                isMainRecive = true;

                MainText.color = MainSelColor;
                MainImage.color = ButtonSelColor;

            }
        else if (mainNum == 3 && MainSuccess)
        {
            if (ItemCompensation(questTyping.main1_item))
            {
                mainNum++;
                isMainRecive = false;
                MainText.color = MainbasicColor;
                MainImage.color = Color.white;
            }
        }
        else if (mainNum == 4 && !isMainRecive) // 7���� �޼�
            {
                questTyping.mainnum = 4;
                questTyping.Textnum = 1;
                questTyping.QuestStartKey();
                MainSuccess = false;
                isMainRecive = true;

                MainText.color = MainSelColor;
                MainImage.color = ButtonSelColor;

            }
        else if (mainNum == 4 && MainSuccess)
        {
            if (ItemCompensation(questTyping.main1_item))
            {
                mainNum++;
                isMainRecive = false;
                MainText.color = MainbasicColor;
                MainImage.color = Color.white;
            }
        }
        else if (mainNum == 5 && !isMainRecive) //�������
            {
                questTyping.mainnum = 5;
                questTyping.Textnum = 1;
                questTyping.QuestStartKey();
                MainSuccess = false;
                isMainRecive = true;

                MainText.color = MainSelColor;
                MainImage.color = ButtonSelColor;

            }
        else if (mainNum == 5 && MainSuccess)
        {
            if (ItemCompensation(questTyping.main1_item))
            {
                mainNum++;
                isMainRecive = false;
                MainText.color = MainbasicColor;
                MainImage.color = Color.white;
                ItemCompensation(questTyping.main5_item);
            }
        }


    }

    public void MainQuestSuccess(int SuccessNum)
    {
        if(SuccessNum == mainNum && isMainRecive)
        {
            MainSuccess = true;
            mainNum++;
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

        }
        return true;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        AllUI.allUI.QuestStroeTop();
    }

}
