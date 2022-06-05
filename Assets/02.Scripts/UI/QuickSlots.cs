using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlots : MonoBehaviour
{
    [SerializeField]
    QuikSlot[] quikSlots;

    private void OnEnable()
    {
        quikSlots = GetComponentsInChildren<QuikSlot>();
    }
    public QuikSlot[] GetSlots() { return quikSlots; }
    [SerializeField]
    Item[] items;
    [SerializeField]
    SkillUI[] skills;

    public void LoadToQuick(int _arrNum, string _Name, int _itemCount, bool _isItem)
    {
        if (_isItem)
        {
            for (int i = 0; i < items.Length; i++)
            {

                if (items[i].itemName == _Name)
                {
                    quikSlots[_arrNum].addItem(quikSlots[_arrNum], items[i], _itemCount);
                }


            }
        }
        else
        {
            for(int i = 0; i < skills.Length; i++)
            {
                if(skills[i].skillName == _Name)
                {
                    quikSlots[_arrNum].addSkill(quikSlots[_arrNum], skills[i]);
                }
            }
        }

    }



}
