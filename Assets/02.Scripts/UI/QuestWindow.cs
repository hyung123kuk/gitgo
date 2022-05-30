using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour
{
    [SerializeField]
    Image mainQuestImage;
    Text MainText;

    [SerializeField]
    QuestNormal questNormal;
    [SerializeField]
    QuestStore questStore;

    private void Start()
    {
        questNormal = FindObjectOfType<QuestNormal>();
        questStore = FindObjectOfType<QuestStore>();
    }


    void questOn(int a)
    {
        if (a == 1) //½½¶óÀÓ Äù½ºÆ® =1
        {
            questNormal.SlimeQuest();
        }
        else if (a == 2) //°íºí¸° Äù½ºÆ® =2
        {
            questNormal.GoblinQuest();
        }
        else if (a == 3) //½ºÄÌ·¹Åæ Äù½ºÆ® =3
        {
            questNormal.SkelletonQuest();
        }
    }
}
