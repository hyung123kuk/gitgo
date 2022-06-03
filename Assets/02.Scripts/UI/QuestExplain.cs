using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestExplain : MonoBehaviour, IPointerClickHandler
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
    [SerializeField]
    QuestWindow questWindow;
    [SerializeField]
    dungeonOpen d_open;
    
    bool mainQ =false;
    bool slimeQ=false;
    bool goblinQ=false;
    bool skelletonQ=false;

    bool QuestSuccess;
    public int QuestNum;


    public string main1Name = "���� : �⺻���� �߿伺!";
    public string main2Name = "���� : ����� �߿伺!";
    public string main3Name = "���� : �ǵ�! �źϽ�����!";
    public string main4Name = "���� : ������ �غ�!";
    public string main5Name = "���� : �ְ��� ����ü!!";
    public string slimeName = "������ ���";
    public string goblinName = "��� ���";
    public string skelltonName = "���̷��� ���";


    Color mainColor = new Color(0.17f, 0.85f, 0.16f);
    Color NonClearColor = new Color(0.3f, 0.3f, 0.3f); //Ŭ���� �������� ���ڻ�

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
        questWindow = FindObjectOfType<QuestWindow>();
        d_open = FindObjectOfType<dungeonOpen>();
        questExplain = this;

    }

    public void QuestExplainOn() //����Ʈ ����â�� ��������
    {

        AllUI.allUI.QuestExplainTop();
        questDesign.SetActive(true);
        isQuestExplain = true;
        AllColorReset(0);
        AllUI.allUI.CheckCursorLock();
    }

    public void QuestExplainOff() //����Ʈ ����â�� �ݾ�����
    {
        if(!UiSound.uiSound.audioSource.isPlaying)
        UiSound.uiSound.UiOptionSound();
        questDesign.SetActive(false);
        isQuestExplain = false;
        AllUI.allUI.CheckCursorLock();
    }
    private void Recieve(bool isRecieve) //����Ʈ�� ������������ Ȯ���ϴ� �Լ�
    {

        if (isRecieve)
            RecieveText.text = "�Ϸ�"; // ����Ʈ�� �޾����� �Ϸ��ư�� ���.
        else
        {
            RecieveText.text = "����"; // ����Ʈ�� ���� �ȹ޾����� ������ư�� ���.
            RecieveText.color = Color.white;
        }

    }

    private void Clear(bool clear) //����Ʈ�� Ŭ���� �ߴ��� Ȯ���ϴ� �Լ�
    {
        RecieveText.color = Color.white;
        if (clear)
        {
            clearText.text = "Clear"; //����Ʈ�� ���������� Ŭ���� �ߴٰ� �˷��ش�.
            UiSound.uiSound.UiOptionSound();
        }
        else
        {
            clearText.text = "";
            RecieveText.color = NonClearColor;
        }
        QuestSuccess = clear;
        
    }

    private void SuccessButtonSet() //����� �����ߴ��� �˸��� ���� ������ falst ��Ű�� �����Ѱ͸� true�ٲٱ����� ����� �Լ�
    {
        mainQ = false;
        slimeQ = false;
        goblinQ = false;
        skelletonQ = false;
    }

    void ItemSet(Item[] items) //������ �����ϴ� �������� ���� ���� �Լ�
    {
        int i = 0;
        foreach (Item item in items)
        {
            conpensation[i].sprite = item.itemImage;
            SetColor(1, i);
            i++;
        }
    }



    #region ����Ʈ�� ���� ����â
    public void Main1(Item[] items, bool isRecieve, bool clear=false)
    {
        QuestExplainOn();
        questName.text = main1Name;
        questName.color = mainColor;
        explainText.text = "KŰ�� ���� ������ ��ų��\n�����Կ� �巡�� �ϰ�,\n��ų�� ����ϼ���!!!";
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
        questName.text = main2Name;
        questName.color = mainColor;
        explainText.text = "���⸦ ���� �� ������\n ���⸦ �����ϼ���!!!";
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
        questName.text = main3Name;
        questName.color = mainColor;
        explainText.text = "����Ϳ� ��Ÿ����\n�źϽ������� ��������!!!";
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
        questName.text = main4Name;
        questName.color = mainColor;
        explainText.text = "8 ������ �޼��ϼ���!!!";
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
        questName.text = main5Name;
        questName.color = mainColor;
        explainText.text = "�ְ��� ����ü��\n���� ����ϼ���!!!";
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
        questName.text = slimeName;
        questName.color = Color.white;
        explainText.text = "�������� 20���� ���!\n" + questNormal.slimeKill + " / 20";
        Clear(clear);
        Recieve(isRecieve);
        ItemSet(items);
        SuccessButtonSet();
        
        slimeQ = true;
    }

    public void GoblinExplain(Item[] items, bool isRecieve, bool clear = false)
    {
        QuestExplainOn();
        questName.text = goblinName;
        questName.color = Color.white;
        explainText.text = "����� 20���� ���!\n" + questNormal.goblinKill + " / 20"; ;
        Clear(clear);
        Recieve(isRecieve);
        ItemSet(items);
        SuccessButtonSet();
        goblinQ = true;
    }

    public void SkeletonExplain(Item[] items, bool isRecieve, bool clear = false)
    {
        QuestExplainOn();
        questName.text = skelltonName;
        questName.color = Color.white;
        explainText.text = "���̷����� 20���� ���!\n" + questNormal.skelletonKill + " / 20"; ;
        Clear(clear);
        Recieve(isRecieve);
        ItemSet(items);
        SuccessButtonSet();
        skelletonQ = true;
    }
    #endregion



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

    public void ClearButton() //����Ʈ �Ϸ��ư�� �������� ��ư�Լ�
    {
        if (mainQ) //���� ����Ʈ �϶�
        {
            if (questStore.isMainRecive) //���� ����Ʈ �޾�����
            {       
                if (QuestSuccess) //����Ʈ ������ �Ϸ� ����
                {


                    if (QuestNum == 1)
                    {

                        if (questStore.QuestEnd(questTyping.main1_item)) //������ ����
                        {
                            UiSound.uiSound.Quest2();
                            questWindow.QuestClear(QuestSlot.QuestName.main);
                            questStore.MainText.text = main2Name;
                            questStore.mainNum = 2;
                        }
                    }
                    else if (QuestNum == 2)
                    {

                        if (questStore.QuestEnd(questTyping.main2_item)) //���������� ����
                        {
                            UiSound.uiSound.Quest2();
                            questWindow.QuestClear(QuestSlot.QuestName.main);
                            questStore.MainText.text = main3Name;
                            questStore.mainNum = 3;
                        }
                    }
                    else if (QuestNum == 3)
                    {
                        if (questStore.QuestEnd(questTyping.main3_item)) //�ս�������� ����
                        {
                            UiSound.uiSound.Quest2();
                            questWindow.QuestClear(QuestSlot.QuestName.main);
                            questStore.MainText.text = main4Name;
                            questStore.mainNum = 4;
                        }
                    }
                    else if (QuestNum == 4)
                    {
                        if (questStore.QuestEnd(questTyping.main4_item)) //8���� �޼� ����
                        {
                            UiSound.uiSound.Quest2();
                            questWindow.QuestClear(QuestSlot.QuestName.main);
                            questStore.MainText.text = main5Name;
                            questStore.mainNum = 5;
                            d_open.DunGeonOpen();
                        }
                    }
                    else if (QuestNum == 5)
                    {
                        if (questStore.QuestEnd(questTyping.main5_item)) //������� ����
                        {
                            UiSound.uiSound.Quest2();
                            questWindow.QuestClear(QuestSlot.QuestName.main);
                            questStore.MainText.text = "����Ʈ ��� �Ϸ�";
                            questStore.mainNum = 6;
                        }
                    }
                    QuestExplainOff();

                }
                else //����Ʈ ���� ���� ���ߴµ� �Ϸ� ����
                {
                    UiSound.uiSound.BuyfailSound();
                    LogManager.logManager.Log("����Ʈ�� �Ϸ����� ���߽��ϴ�.", true);
                }
            }
            else  //���� ����Ʈ�� ���� �ʾ�����
            {
                questStore.QuestStart();
                RecieveText.text = "�Ϸ�";
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
                    if (questNormal.QuestEnd(questNormal.slime_item, 0))
                    {
                        questWindow.QuestClear(QuestSlot.QuestName.slime);
                        QuestExplainOff();
                    }
                }
                else
                {
                    LogManager.logManager.Log("����Ʈ�� �Ϸ����� ���߽��ϴ�.", true);
                }
            }
            else
            {
                questNormal.SlimeQuestStart();
                RecieveText.text = "�Ϸ�";
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

                    if (questNormal.QuestEnd(questNormal.goblin_item, 1))
                    {
                        questWindow.QuestClear(QuestSlot.QuestName.goblin);
                        QuestExplainOff();
                    }
                }
                else
                {
                    LogManager.logManager.Log("����Ʈ�� �Ϸ����� ���߽��ϴ�.", true);
                }
            }
            else
            {
                questNormal.GoblinQuestStart();
                RecieveText.text = "�Ϸ�";
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

                    if (questNormal.QuestEnd(questNormal.skelleton_item, 2))
                    {
                        questWindow.QuestClear(QuestSlot.QuestName.skelleton);
                        QuestExplainOff();
                    }
                }
                else
                {
                    LogManager.logManager.Log("����Ʈ�� �Ϸ����� ���߽��ϴ�.", true);
                }
            }
            else
            {
                questNormal.SkelletonQuestStart();
                RecieveText.text = "�Ϸ�";
                RecieveText.color = NonClearColor;
                QuestExplainOff();
            }
        }

        AllUI.allUI.CheckCursorLock();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AllUI.allUI.QuestExplainTop();
    }
}
