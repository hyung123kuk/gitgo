using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNormal : MonoBehaviour
{
    [SerializeField]
    private QuestExplain questExplain;
    [SerializeField]
    private QuestWindow questWindow;

    [SerializeField]
    private Text slimeText;
    private Text goblinText;
    private Text skeletonText;

    private Image slimeImage;
    private Image goblinImage;
    private Image skeletonImage;

    public int slimeQuestKill = 20;
    public int slimeKill = 0;
    public bool Quest_slime=false;
    public bool slime_Success=false;
    [SerializeField]
    public Item[] slime_item;

    public int goblinQuestKill = 20;
    public int goblinKill = 0;
    public bool Quest_goblin=false;
    public bool goblin_Success=false;
    [SerializeField]
    public Item[] goblin_item;

    public int skelletonQuestKill = 20;
    public int skelletonKill = 0;
    public bool Quest_skelleton=false;
    public bool skelleton_Success=false;
    [SerializeField]
    public Item[] skelleton_item;


    Color QuestSelColor = new Color(0.3f, 0.3f, 0.3f); //퀘스트 받았을때 이름 컬러
    Color ButtonSelColor = new Color(0.7f, 0.7f, 0.7f); //퀘스트 받았을때 버튼 컬러

    private void Start()
    {
        questExplain = FindObjectOfType<QuestExplain>();
        questWindow = FindObjectOfType<QuestWindow>();
        slimeText = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
        goblinText = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
        skeletonText = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>();

        slimeImage = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
        goblinImage = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<Image>();
        skeletonImage = transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetComponent<Image>();
    }

    public void SlimeQuestStart() 
    {
        UiSound.uiSound.Quest1();
        Quest_slime = true;
        slimeText.color = QuestSelColor;
        slimeImage.color = ButtonSelColor;
        questWindow.QuestRecieve(QuestSlot.QuestName.slime);

    }
    
    public void SlimeQuest() 
    {
        
        questExplain.SlimeExplain(slime_item, Quest_slime, slime_Success);
    }

    public void SlimeKillCount()
    {
        if (Quest_slime) //퀘스트를 받았을때
        {
            slimeKill++;
            if (slimeKill >= 20)
            {
                slimeKill = 20;
                slime_Success = true;
                questExplain.SlimeExplain(slime_item, Quest_slime, slime_Success);
            }
        }
    }


    public void GoblinQuestStart()
    {
        UiSound.uiSound.Quest1();
        Quest_goblin = true;
        goblinText.color = QuestSelColor;
        goblinImage.color = ButtonSelColor;
        questWindow.QuestRecieve(QuestSlot.QuestName.goblin);
    }

    public void GoblinQuest()
    {
        questExplain.GoblinExplain(goblin_item, Quest_goblin, goblin_Success);
    }

    public void GoblinKillCount()
    {
        if (Quest_goblin)
        {
            goblinKill++;
            if (goblinKill >= 20)
            {
                goblinKill = 20;
                goblin_Success = true;
                questExplain.SlimeExplain(goblin_item, Quest_goblin, goblin_Success);
            }
        }
    }


    public void SkelletonQuestStart()
    {
        UiSound.uiSound.Quest1();
        Quest_skelleton = true;
        skeletonText.color = QuestSelColor;
        skeletonImage.color = ButtonSelColor;
        questWindow.QuestRecieve(QuestSlot.QuestName.skelleton);
    }

    public void SkelletonQuest()
    {
        questExplain.SkeletonExplain(skelleton_item, Quest_skelleton, skelleton_Success);
    }

    public void SkelletonKillCount() 
    {
        if (Quest_skelleton)
        {
            skelletonKill++;
            if (skelletonKill >= 20)
            {
                skelletonKill = 20;
                skelleton_Success = true;
                questExplain.SlimeExplain(skelleton_item, Quest_skelleton, skelleton_Success);
            }
        }
    }

    public bool QuestEnd(Item[] items, int QuestNum) //퀘스트 끝났을때 보상 체크하고 메세지 세팅하는곳
    {
        if (ItemCompensation(items))
        {
            if (QuestNum == 0)
            {
                UiSound.uiSound.Quest2();
                Quest_slime = false;
                 slime_Success = false;
                slimeText.color = Color.white;
                slimeImage.color = Color.white;
                slimeKill = 0;
                return true;
            }
            else if (QuestNum == 1)
            {
                UiSound.uiSound.Quest2();
                Quest_goblin = false;
                goblin_Success = false;
                goblinText.color = Color.white;
                goblinImage.color = Color.white;
                goblinKill = 0;
                return true;
            }
            else if (QuestNum ==2)
            {
                UiSound.uiSound.Quest2();
                Quest_skelleton = false;
                skelleton_Success = false;
                skeletonText.color = Color.white;
                skeletonImage.color = Color.white;
                skelletonKill = 0;
                return true;
            }
        }

        return false;
    }
    public bool ItemCompensation(Item[] items) //보상 체크 하는 함수
    {
        int usedItemCount = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemType == Item.ItemType.Used && inventory.inven.HasSameSlot(items[i]))
            {
                usedItemCount++;
            }

        }

        if (!inventory.inven.HasEmptySlot(items.Length - usedItemCount))
        {

            Debug.Log("아이템 창이 없습니다.");
            LogManager.logManager.Log("빈창이 없습니다", true);
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


}
