using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    public Image imageSkill;
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
        gameObject.GetComponent<QuikSlot>().slot.tooltip.ToolTipOff();
        if (DragSkillSlot.instance.dragSkillSlot == null)//��ų���� �巡�� �ƴҶ� ��� ����
            return;

        if (gameObject.layer == LayerMask.NameToLayer("quikSlot") && DragSkillSlot.instance.dragSkillSlot.gameObject.layer == LayerMask.NameToLayer("SkillSlot") ) // ��ų���� -> ������
        {
            if(gameObject.GetComponent<QuikSlot>().slot.item != null) // �Һ�������� �ִٸ�
            {
                Slot instanceSlot = gameObject.GetComponent<QuikSlot>().slot;
                if (!instanceSlot.inven.HasEmptySlot() && !instanceSlot.inven.HasSameSlot(instanceSlot.item)) //�κ��� ��â ������ ������ ���� ��� ��ų �� ����
                {
                    Debug.Log("��â�� �����ϴ�.");
                    return;
                }
                instanceSlot.inven.addItem(instanceSlot.item, instanceSlot.itemCount);
                instanceSlot.ClearSlot();
            }
            skill = DragSkillSlot.instance.dragSkillSlot.skill;
            imageSkill.sprite = skill.SkillImage;
            SetColor(1);
           


        }


        else if(gameObject.layer == LayerMask.NameToLayer("quikSlot")&& DragSkillSlot.instance.dragSkillSlot.gameObject.layer == LayerMask.NameToLayer("quikSlot")) // ������ -> ������
        {
            if (gameObject.GetComponent<QuikSlot>().slot.item != null) // �Һ�������� �ִٸ�
            {


                Slot instanceSlot = DragSkillSlot.instance.dragSkillSlot.gameObject.GetComponent<QuikSlot>().slot;
                instanceSlot.AddItem(gameObject.GetComponent<QuikSlot>().slot.item, gameObject.GetComponent<QuikSlot>().slot.itemCount);
                
                gameObject.GetComponent<QuikSlot>().slot.item=null;
                gameObject.GetComponent<QuikSlot>().slot.itemCount = 0;
                gameObject.GetComponent<QuikSlot>().slot.text_Count.text= "";

                skill = DragSkillSlot.instance.dragSkillSlot.skill;
                DragSkillSlot.instance.dragSkillSlot.ClearSlot();
                imageSkill.sprite = skill.SkillImage;
                DragSkillSlot.instance.dragSkillSlot.SetColor(1);
                SetColor(1);
                instanceSlot.itemImage.sprite = instanceSlot.item.itemImage;

             
                
            }

            else if (skill != null)
            {
                SkillUI instanceSkill = DragSkillSlot.instance.dragSkillSlot.skill;
                Debug.Log("1");
                DragSkillSlot.instance.dragSkillSlot.skill = skill;
                DragSkillSlot.instance.dragSkillSlot.Setimage();
                skill = instanceSkill;
                imageSkill.sprite = skill.SkillImage;
                SetColor(1);
          
            }
            else if (skill == null)
            {
                SkillUI instanceSkill = DragSkillSlot.instance.dragSkillSlot.skill;
                DragSkillSlot.instance.dragSkillSlot.ClearSlot();
                skill = instanceSkill;
                imageSkill.sprite = skill.SkillImage;
                SetColor(1);
                
            }
           



        }

        gameObject.GetComponent<QuikSlot>().setCoolImage();
        if(DragSkillSlot.instance.dragSkillSlot.gameObject.layer == LayerMask.NameToLayer("quikSlot") )
            DragSkillSlot.instance.dragSkillSlot.gameObject.GetComponent<QuikSlot>().setCoolImage();
            
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
    public void Setimage()
    {
        imageSkill.sprite = skill.SkillImage;
    }
}
