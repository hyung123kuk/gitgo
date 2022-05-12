using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    private Image imageSkill;
    [SerializeField]
    private PlayerST playerST;
    [SerializeField]
    public SkillUI skill;

    [SerializeField]
    private SkillUI[] Warrior_skills;
    [SerializeField]
    private SkillUI[] Archer_skills;
    [SerializeField]
    private SkillUI[] Mage_skills;





    void Start()
    {
        playerST = FindObjectOfType<PlayerST>();
        SkillSet();

    }

    private void SkillSet()
    {
        if (playerST.CharacterType == PlayerST.Type.Warrior)
        {
            if (gameObject.tag == "SKILLSLOT1")
            {
                skill = Warrior_skills[0];
            }
            else if (gameObject.tag == "SKILLSLOT2")
            {
                skill = Warrior_skills[1];
            }
            else if (gameObject.tag == "SKILLSLOT3")
            {
                skill = Warrior_skills[2];
            }
            else if (gameObject.tag == "SKILLSLOT4")
            {
                skill = Warrior_skills[3];
            }
        }

        if (playerST.CharacterType == PlayerST.Type.Archer)
        {
            if (gameObject.tag == "SKILLSLOT1")
            {
                skill = Archer_skills[0];
            }
            else if (gameObject.tag == "SKILLSLOT2")
            {
                skill = Archer_skills[1];
            }
            else if (gameObject.tag == "SKILLSLOT3")
            {
                skill = Archer_skills[2];
            }
            else if (gameObject.tag == "SKILLSLOT4")
            {
                skill = Archer_skills[3];
            }
        }
        if (playerST.CharacterType == PlayerST.Type.Mage)
        {
            if (gameObject.tag == "SKILLSLOT1")
            {
                skill = Mage_skills[0];
            }
            else if (gameObject.tag == "SKILLSLOT2")
            {
                skill = Mage_skills[1];
            }
            else if (gameObject.tag == "SKILLSLOT3")
            {
                skill = Mage_skills[2];
            }
            else if (gameObject.tag == "SKILLSLOT4")
            {
                skill = Mage_skills[3];
            }
        }
        if(skill!=null)
        imageSkill.sprite = skill.SkillImage;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (skill != null)
        {
            DragSkillSlot.instance.dragSkillSlot = this;
            DragSkillSlot.instance.DragSetImage(imageSkill);
            DragSkillSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skill != null)
        {
            DragSkillSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSkillSlot.instance.SetColor(0);
        DragSkillSlot.instance.dragSkillSlot = null;

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSkillSlot.instance.dragSkillSlot == null)
            return;
        if (gameObject.layer == LayerMask.NameToLayer("quikSlot"))
        {
            if(gameObject.GetComponent<QuikSlot>().slot.item !=null) //소비아이템이 이미 있을때
            {
                Slot instanceSlot = gameObject.GetComponent<QuikSlot>().slot;
                if (!instanceSlot.inven.HasEmptySlot() && !instanceSlot.inven.HasSameSlot(instanceSlot.item)) //인벤에 빈창 없으면 아이템 들어갈곳 없어서 스킬 못 넣음
                {
                    Debug.Log("빈창이 없습니다.");
                    return;
                }
                instanceSlot.inven.addItem(instanceSlot.item, instanceSlot.itemCount);
                instanceSlot.ClearSlot();
                


            }

            skill = DragSkillSlot.instance.dragSkillSlot.skill;
            imageSkill.sprite = skill.SkillImage;
            SetColor(1);
            if(DragSkillSlot.instance.dragSkillSlot.gameObject.layer == LayerMask.NameToLayer("quikSlot"))
            {
                DragSkillSlot.instance.dragSkillSlot.ClearSlot();
            }
        }
        
    }

    public void SetColor(float _alpha)
    {
        Color color = imageSkill.color;
        color.a = _alpha;
        imageSkill.color = color;
    }
    public void ClearSlot()
    {
        skill = null;
        imageSkill.sprite = null;
        SetColor(0);

    }
}
