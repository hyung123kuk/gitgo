using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuikSlot : MonoBehaviour
{

    
    [SerializeField]
    public Slot slot;
    [SerializeField]
    public SkillSlot skill;
    [SerializeField]
    public Image CoolTimeImage;
    [SerializeField]
    private PlayerST playerST;
    [SerializeField]
    private Weapons weapons;


    void Start()
    {
        slot=gameObject.GetComponent<Slot>();
        skill = gameObject.GetComponent<SkillSlot>();
        playerST = FindObjectOfType<PlayerST>();
        weapons = FindObjectOfType<Weapons>();

    }

    public void setCoolImage()
    {
        if(skill.skill == null)
        {
            SetColor(0);
        }


        if (skill.skill != null)
        {
            CoolTimeImage.sprite = skill.skill.SkillImage;
            SetColor(1);
        }

    }

    
    void Update()
    {



        if (slot.item!=null && Input.GetButtonDown(gameObject.tag)) //������ ���
        {
            Debug.Log(slot.item.itemName + " �� ����߽��ϴ�.");
            slot.UseItem();
            slot.SetSlotCount(-1);
        }

        else if(skill.skill != null && skill.skill.skillCharacter==SkillUI.SkillCharacter.Archer && skill.skill.skillNum == 4) // �ü� ������ ��ų
        {
            if(Input.GetButton(gameObject.tag))
            {
                weapons.archer_skill4_Order = 0;
                weapons.EnergyArrow();
            }
            else if(Input.GetButtonUp(gameObject.tag))
            {
                weapons.archer_skill4_Order = 1;
                weapons.EnergyArrow();
            }
        }

        else if (skill.skill != null && skill.skill.skillCharacter == SkillUI.SkillCharacter.Mage && skill.skill.skillNum == 4) // ������ ������ ��ų
        {
            if (Input.GetButton(gameObject.tag))
            {
                weapons.mage_skill4_Order = 0;
                weapons.Meteo();
            }
            else if (Input.GetButtonUp(gameObject.tag))
            {
                weapons.mage_skill4_Order = 1;
                weapons.Meteo();
            }
        }


        else if(skill.skill!=null && Input.GetButtonDown(gameObject.tag))
        {
            Debug.Log("��ų���");
            #region ���� ��ų
            if(skill.skill.skillCharacter == SkillUI.SkillCharacter.Warrior)
            {
                if(skill.skill.skillNum == 1) { playerST.Block(); }
                else if (skill.skill.skillNum == 2) { playerST.Buff(); }
                else if (skill.skill.skillNum == 3) { playerST.Rush(); }
                else if (skill.skill.skillNum == 4) { playerST.Aura();  }

            }
            #endregion
            #region �ü� ��ų
            if (skill.skill.skillCharacter == SkillUI.SkillCharacter.Archer)
            {
                if (skill.skill.skillNum == 1) { playerST.Smoke(); }
                else if (skill.skill.skillNum == 2) { playerST.PoisonArrow(); }
                else if (skill.skill.skillNum == 3) { weapons.BombArrow(); }

            }
            #endregion
            #region ���� ��ų
            if (skill.skill.skillCharacter == SkillUI.SkillCharacter.Mage)
            {
                if (skill.skill.skillNum == 1) { playerST.Flash(); }
                else if (skill.skill.skillNum == 2) { weapons.LightningBall(); }
                else if (skill.skill.skillNum == 3) { weapons.IceAge(); }
                else if (skill.skill.skillNum == 4) { weapons.Meteo(); }
            }
            #endregion

        }


    }

    public void SetColor(float _alpha)
    {
        Color color = CoolTimeImage.color;
        color.a = _alpha;
        CoolTimeImage.color = color;
    }
}

