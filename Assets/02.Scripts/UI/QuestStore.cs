using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStore : MonoBehaviour
{
    
    [SerializeField]
    QuestTyping questTyping;
    int mainNum = 1;
    public bool MainSuccess = true;
    void Start()
    {
       questTyping = FindObjectOfType<QuestTyping>();
    }

    public void Main()
    {
        if (MainSuccess)
        {
            if (mainNum == 1)
            {
                questTyping.mainnum = 1;
                questTyping.Textnum = 1;
                questTyping.QuestStartKey();
            }
            else if (mainNum == 2)
            {

            }
            mainNum++;
        }
    }
}
