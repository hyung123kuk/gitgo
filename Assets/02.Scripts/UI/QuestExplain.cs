using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestExplain : MonoBehaviour
{
    public static bool isQuestExplain;
    [SerializeField]
    public static QuestExplain questExplain;
    [SerializeField]
    QuestStore questStore;
    [SerializeField]
    GameObject questDesign;
    [SerializeField]
    Text questName;
    [SerializeField]
    Text explainText;
    [SerializeField]
    Text clearText;
    [SerializeField]
    Text RecieveText;
    [SerializeField]
    Image[] conpensation;
    [SerializeField]
    QuestTyping questTyping;
    [SerializeField]
    QuestNormal questNormal;

    bool mainQ =false;
    bool slimeQ=false;
    bool goblinQ=false;
    bool skelletonQ=false;

    bool QuestSuccess;
    int QuestNum;

    Color mainColor = new Color(0.17f, 0.85f, 0.16f);
    Color NonClearColor = new Color(0.3f, 0.3f, 0.3f); //클리어 못했을때 글자색

    private void Awake()
    {
        
        questDesign = transform.GetChild(0).gameObject;
        questName = transform.GetChild(0).GetChild(2).GetComponent<Text>();
        explainText = transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>();
        clearText = transform.GetChild(0).GetChild(5).GetComponent<Text>();
        RecieveText = transform.GetChild(0).GetChild(6).GetChild(0).GetComponent<Text>();
        conpensation = transform.GetChild(0).GetChild(7).GetComponentsInChildren<Image>();
        questTyping = FindObjectOfType<QuestTyping>();
        questStore = FindObjectOfType<QuestStore>();
        questNormal = FindObjectOfType<QuestNormal>();
        questExplain = this;

    }

    public void QuestExplainOff()
    {
        questDesign.SetActive(false);
        isQuestExplain = false;
    }
    private void Recieve(bool isRecieve)
    {

        if (isRecieve)
            RecieveText.text = "완료"; // 퀘스트를 받았으면 완료버튼이 뜬다.
        else
        {
            RecieveText.text = "수락"; // 퀘스트를 아직 안받았으면 수락버튼이 뜬다.
            RecieveText.color = Color.white;
        }

    }

    private void Clear(bool clear)
    {
        RecieveText.color = Color.white;
        if (clear)
        {
            clearText.text = "Clear"; //퀘스트를 성공했으면 클리어 했다고 알려준다.
        }
        else
        {
            clearText.text = "";
            RecieveText.color = NonClearColor;
        }
        QuestSuccess = clear;
        
    }

    private void SuccessButtonSet()
    {
        mainQ = false;
        slimeQ = false;
        goblinQ = false;
        skelletonQ = false;
    }

    void ItemSet(Item[] items)
    {
        int i = 0;
        foreach (Item item in items)
        {
            conpensation[i].sprite = item.itemImage;
            SetColor(1, i);
            i++;
        }
    }

    public void QuestExplainOn()
    {

        questDesign.SetActive(true);
        isQuestExplain = true;
        AllColorReset(0);
        AllUI.allUI.CheckCursorLock();
    }

    public void Main1(Item[] items, bool isRecieve, bool clear=false)
    {
        QuestExplainOn();
        questName.text = "메인 : 기본기의 중요성!";
        questName.color = mainColor;
        explainText.text = "K키를 눌러 구르기 스킬을\n퀵슬롯에 드래그 하고,\n스킬을 사용하세요!!!";
        Clear(clear);
        Recieve(isRecieve);
        
        ItemSet(items);
        QuestNum = 1;
        SuccessButtonSet();
        mainQ = true;
    }

    public void Main2(Item[] items, bool isRecieve, bool clear = false)
    {
        QuestExplainOn();
        questName.text = "메인 : 장비의 중요성!";
        questName.color = mainColor;
        explainText.text = "무기를 구매 및 습득후\n 무기를 장착하세요!!!";
        Clear(clear);
        Recieve(isRecieve);
        
        ItemSet(items);
        QuestNum = 2;
        SuccessButtonSet();
        mainQ = true;
    }

    public void Main3(Item[] items, bool isRecieve, bool clear = false)
    {
        QuestExplainOn();
        questName.text = "메인 : 악동! 거북슬라임!";
        questName.color = mainColor;
        explainText.text = "사냥터에 나타나는\n거북슬라임을 잡으세요!!!";
        Clear(clear);
        Recieve(isRecieve);
     
        ItemSet(items);
        QuestNum = 3;
        SuccessButtonSet();
        mainQ = true;
    }

    public void Main4(Item[] items, bool isRecieve, bool clear = false)
    {
        QuestExplainOn();
        questName.text = "메인 : 마지막 준비!";
        questName.color = mainColor;
        explainText.text = "7 레벨을 달성하세요!!!";
        Clear(clear);
        Recieve(isRecieve);

        ItemSet(items);
        QuestNum = 4;
        SuccessButtonSet();
        mainQ = true;

    }

    public void Main5(Item[] items, bool isRecieve, bool clear = false)
    {
        QuestExplainOn();
        questName.text = "메인 : 최강의 생물체!";
        questName.color = mainColor;
        explainText.text = "최강의 생물체인\n골렘을 사냥하세요!!!";
        Clear(clear);
        Recieve(isRecieve);

        ItemSet(items);
        QuestNum = 5;
        SuccessButtonSet();
        mainQ = true;
    }

    public void SlimeExplain(Item[] items,bool isRecieve,bool clear = false)
    {
        QuestExplainOn();
        questName.text = "슬라임 잡기";
        questName.color = Color.white;
        explainText.text = "슬라임을 20마리 사냥!\n" + questNormal.slimeKill + " / 20";
        Clear(clear);
        Recieve(isRecieve);
        ItemSet(items);
        SuccessButtonSet();
        slimeQ = true;
    }

    public void GoblinExplain(Item[] items, bool isRecieve, bool clear = false)
    {
        QuestExplainOn();
        questName.text = "고블린 잡기";
        questName.color = Color.white;
        explainText.text = "고블린을 20마리 사냥!\n" + questNormal.goblinKill + " / 20"; ;
        Clear(clear);
        Recieve(isRecieve);
        ItemSet(items);
        SuccessButtonSet();
        goblinQ = true;
    }

    public void SkeletonExplain(Item[] items, bool isRecieve, bool clear = false)
    {
        QuestExplainOn();
        questName.text = "스켈레톤 잡기";
        questName.color = Color.white;
        explainText.text = "스켈레톤을 20마리 사냥!\n" + questNormal.skelletonKill + " / 20"; ;
        Clear(clear);
        Recieve(isRecieve);
        ItemSet(items);
        SuccessButtonSet();
        skelletonQ = true;
    }

    public void AllColorReset(int _alpha)
    {
        for (int i = 0; i < conpensation.Length; i++)
        {
            Color color = conpensation[i].color;
            color.a = _alpha;
            conpensation[i].color = color;
        }
    }
    public void SetColor(int _alpha, int i)
    {
        Color color = conpensation[i].color;
        color.a = _alpha;
        conpensation[i].color = color;
    }

    public void ClearButton()
    {
        if (mainQ)
        {
            if (questStore.isMainRecive) //메인 퀘스트 받았을때
            {
                if (QuestSuccess) //퀘스트 성공후 완료 누름
                {


                    if (QuestNum == 1)
                    {
                        questStore.QuestEnd(questTyping.main1_item);
                        questStore.MainText.text = "메인 : 장비의 중요성!";
                        questStore.mainNum = 2;
                    }
                    else if (QuestNum == 2)
                    {
                        questStore.QuestEnd(questTyping.main2_item);
                        questStore.MainText.text = "메인 : 악동! 거북슬라임!";
                        questStore.mainNum = 3;
                    }
                    else if (QuestNum == 3)
                    {
                        questStore.QuestEnd(questTyping.main3_item);
                        questStore.MainText.text = "메인 : 마지막 준비!";
                        questStore.mainNum = 4;
                    }
                    else if (QuestNum == 4)
                    {
                        questStore.QuestEnd(questTyping.main4_item);
                        questStore.MainText.text = "메인 : 최강의 생물체!";
                        questStore.mainNum = 5;
                    }
                    else if (QuestNum == 5)
                    {
                        questStore.QuestEnd(questTyping.main5_item);
                        questStore.MainText.text = "퀘스트 모두 완료";
                        questStore.mainNum = 6;
                    }
                    QuestExplainOff();

                }
                else //퀘스트 아직 성공 못했는데 완료 누름
                {
                    LogManager.logManager.Log("퀘스트를 완료하지 못했습니다.", true);
                }
            }
            else  //아직 퀘스트를 받지 않았을때
            {
                questStore.QuestStart();
                RecieveText.text = "완료";
                RecieveText.color = NonClearColor;
                QuestExplainOff();
            }
        }
        else if(slimeQ)
        {
            if(questNormal.Quest_slime)
            {
                if (questNormal.slime_Success)
                {
                    questNormal.QuestEnd(questNormal.slime_item,0);
                    QuestExplainOff();
                }
                else
                {
                    LogManager.logManager.Log("퀘스트를 완료하지 못했습니다.", true);
                }
            }
            else
            {
                questNormal.SlimeQuestStart();
                RecieveText.text = "완료";
                RecieveText.color = NonClearColor;
                QuestExplainOff();
            }
        }

        else if (goblinQ)
        {
            if (questNormal.Quest_goblin)
            {
                if (questNormal.goblin_Success)
                {
                    questNormal.QuestEnd(questNormal.goblin_item,1);
                    QuestExplainOff();
                }
                else
                {
                    LogManager.logManager.Log("퀘스트를 완료하지 못했습니다.", true);
                }
            }
            else
            {
                questNormal.GoblinQuestStart();
                RecieveText.text = "완료";
                RecieveText.color = NonClearColor;
                QuestExplainOff();
            }
        }
        else if (skelletonQ)
        {
            if (questNormal.Quest_skelleton)
            {
                if (questNormal.skelleton_Success)
                {
                    questNormal.QuestEnd(questNormal.skelleton_item,2);
                    QuestExplainOff();
                }
                else
                {
                    LogManager.logManager.Log("퀘스트를 완료하지 못했습니다.", true);
                }
            }
            else
            {
                questNormal.SkelletonQuestStart();
                RecieveText.text = "완료";
                RecieveText.color = NonClearColor;
                QuestExplainOff();
            }
        }

        AllUI.allUI.CheckCursorLock();
    }
    

}
