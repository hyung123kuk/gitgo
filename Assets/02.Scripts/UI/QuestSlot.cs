using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    [SerializeField]
    QuestNormal questNormal;
    [SerializeField]
    QuestStore questStore;


    [SerializeField]
    public Image questImage;
    [SerializeField]
    public Text questText;
    public enum QuestName { None, main, slime, goblin, skelleton }
    [SerializeField]
    public QuestName questName;



    


    void Start()
    {
        questNormal = FindObjectOfType<QuestNormal>();
        questStore = FindObjectOfType<QuestStore>();
        questImage = GetComponent<Image>();
        questText = GetComponentInChildren<Text>();
        questName = QuestName.None;
    }



    public void OpenQuest() //ĳ���� ����Ʈ ���Կ��� ����Ʈ�� ��������
    {
        UiSound.uiSound.UiOptionSound();
        if (questName == QuestName.slime)
        {
            questNormal.SlimeQuest();
        }
        else if (questName == QuestName.goblin) 
        {
            questNormal.GoblinQuest();
        }
        else if (questName == QuestName.skelleton) 
        {
            questNormal.SkelletonQuest();
        }
        else if(questName == QuestName.main)
        {
            questStore.MainQuest();
        }
    }
}
