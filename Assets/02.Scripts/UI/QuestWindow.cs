using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour, IPointerClickHandler
{
    public static QuestWindow qWIndow;
    [SerializeField]
    GameObject questWindow;
    [SerializeField]
    QuestExplain questExplain;

    public static bool isQuestWindow =false;

    [SerializeField]
    QuestSlot[] Slots;

    Color MainbasicColor = new Color(0.17f, 0.85f, 0.16f); //메인 이름 컬러

    public int questOrder =0;
    private void Start()
    {
        qWIndow = this;
        
        questWindow = transform.GetChild(0).gameObject;
        StartCoroutine(startSet());
        questExplain = FindObjectOfType<QuestExplain>();
        Slots = GetComponentsInChildren<QuestSlot>();

    }
    IEnumerator startSet() //슬롯의 enable이 false상태여서 슬롯이 안불러와져 잠깐 켜놓고 불러오고 끔
    {
        questWindow.SetActive(true);
        yield return new WaitForSeconds(0.03f);
        questWindow.SetActive(false);
    }
    public void questWindowOff() //퀘스트 윈도우창 끄기
    {
        UiSound.uiSound.UiOptionSound();
        QuestStore.qustore.storeOff();
        questWindow.SetActive(false);       
        isQuestWindow = false;
        AllUI.allUI.CheckCursorLock();
    }
    public void questWindowOn() //퀘스트 윈도우창 키기
    {
        UiSound.uiSound.UiOptionSound();
        questWindow.SetActive(true);
        isQuestWindow = true;
        AllUI.allUI.QuestWindowTop();
        AllUI.allUI.CheckCursorLock();

    }


    public void SlotSet() //슬롯의 이미지나 텍스트를 세팅하는 함수
    {
        for( int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].questName == QuestSlot.QuestName.None)
            {
                Slots[i].questText.text = "";
                ColorSet(Slots[i], 0);
            }
            else if (Slots[i].questName == QuestSlot.QuestName.main)
            {
                ColorSet(Slots[i], 1);
                Slots[i].questText.color = MainbasicColor;
                int num = questExplain.QuestNum;
                if (num == 1)
                {
                    Slots[i].questText.text = questExplain.main1Name;
                }
                else if (num == 2)
                {
                    Slots[i].questText.text = questExplain.main2Name;
                }
                else if (num == 3)
                {
                    Slots[i].questText.text = questExplain.main3Name;
                }
                else if (num == 4)
                {
                    Slots[i].questText.text = questExplain.main4Name;
                }
                else if (num == 5)
                {
                    Slots[i].questText.text = questExplain.main5Name;
                }
            }
            else if (Slots[i].questName == QuestSlot.QuestName.slime)
            {
                ColorSet(Slots[i], 1);
                Slots[i].questText.color = Color.white;
                Slots[i].questText.text = questExplain.slimeName;
            }
            else if (Slots[i].questName == QuestSlot.QuestName.goblin)
            {
                ColorSet(Slots[i], 1);
                Slots[i].questText.color = Color.white;
                Slots[i].questText.text = questExplain.goblinName;
            }
            else if (Slots[i].questName == QuestSlot.QuestName.skelleton)
            {
                ColorSet(Slots[i], 1);
                Slots[i].questText.color = Color.white;
                Slots[i].questText.text = questExplain.skelltonName;
            }


        }
    }

    public void QuestRecieve(QuestSlot.QuestName ClearName) //퀘스트를 받았을때 슬롯에 세팅되는 함수
    {
        Slots[questOrder].questName = ClearName;
        questOrder++;
        SlotSet();

    }

    public void QuestClear(QuestSlot.QuestName ClearName) //퀘스트를 클리어 했을때 슬롯에 세팅되는 함수
    {
        int index =Slots.Length ;
        for(int i = 0; i < Slots.Length; i++)
        {
            if(Slots[i].questName == ClearName)
            {
                index = i;
                Slots[i].questName = QuestSlot.QuestName.None;
                break;
            }
        }
        if (index != questOrder-1) {

            for (int i = index; i < Slots.Length-1; i++)
            {
                Slots[i].questName = Slots[i + 1].questName;

            }
         }
        Slots[questOrder-1].questName = QuestSlot.QuestName.None;
        SlotSet();
        questOrder--;
    }

    void ColorSet(QuestSlot slot,float _a)
    {
        Color color = slot.questImage.color;
        color.a = _a;
        slot.questImage.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AllUI.allUI.QuestWindowTop();
    }
}
