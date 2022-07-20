using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.Video;

public class SkillSlot : MonoBehaviourPun, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler ,IPointerClickHandler
{
    public static SkillSlot skillSlot;

    [SerializeField]
    public Image imageSkill;
    [SerializeField]
    private PlayerST playerST;
    [SerializeField]
    public SkillUI skill;

    public bool Check; //�����԰� ��ųâ���� ���п뵵



    [SerializeField]
    private SkillUI[] Warrior_skills;
    [SerializeField]
    private SkillUI[] Archer_skills;
    [SerializeField]
    private SkillUI[] Mage_skills;
    [SerializeField]
    private SkillUI[] Common_skills;
    [SerializeField]
    public SkillToolTip skillToolTip;
    [SerializeField]
    private PlayerStat playerStat;
    [SerializeField]
    private SkillWindow skillWindow;
    [SerializeField]
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        skillWindow = FindObjectOfType<SkillWindow>();
        skillToolTip = FindObjectOfType<SkillToolTip>();
        skillSlot = this;
        

    }



    void Start()
    {

        PlayerST[] playerSts = FindObjectsOfType<PlayerST>();


        foreach (PlayerST myplayerSt in playerSts)
        {
            if (myplayerSt.GetComponent<PhotonView>().IsMine)
            {
                playerST = myplayerSt;
                break;
            }
        }


        PlayerStat[] playerStats = GameObject.FindObjectsOfType<PlayerStat>();


        foreach (PlayerStat myplayerStat in playerStats)
        {
            if (myplayerStat.GetComponent<PhotonView>().IsMine)
            {
                playerStat = myplayerStat;
                break;
            }
        }
        
        
        SkillSet();
        Invoke("SetSkillColor", 0.3f);
      


    }



    public void SetSkillColor()
    {
        Debug.Log(1);
        if (gameObject.layer == LayerMask.NameToLayer("SkillSlot"))
        {
            
            if (skill.Level > playerStat.Level)
            {
                imageSkill.color = new Color(1, 0, 0);

            }
            else
            {
                imageSkill.color = new Color(1, 1, 1);
            }
        }
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

        if (gameObject.tag == "DodgeSlot")
        {
            skill = Common_skills[0];
        }

        if (skill != null)
            imageSkill.sprite = skill.SkillImage;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (skill != null && skill.Level <= playerStat.Level)
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
        if (gameObject.layer == LayerMask.NameToLayer("quikSlot"))
            gameObject.GetComponent<QuikSlot>().slot.tooltip.ToolTipOff();
        if (DragSkillSlot.instance.dragSkillSlot == null)//��ų���� �巡�� �ƴҶ� ��� ����
            return;

        if (gameObject.layer == LayerMask.NameToLayer("quikSlot") && DragSkillSlot.instance.dragSkillSlot.gameObject.layer == LayerMask.NameToLayer("SkillSlot")) // ��ų���� -> ������
        {
            if (gameObject.GetComponent<QuikSlot>().slot.item != null) // �Һ�������� �ִٸ�
            {

                Slot instanceSlot = gameObject.GetComponent<QuikSlot>().slot;
                if (!instanceSlot.inven.HasEmptySlot() && !instanceSlot.inven.HasSameSlot(instanceSlot.item)) //�κ��� ��â ������ ������ ���� ��� ��ų �� ����
                {
                    
                    LogManager.logManager.Log("아이템창이 꽉찼습니다.", true);
                    return;
                }

                instanceSlot.inven.addItem(instanceSlot.item, instanceSlot.itemCount);
                instanceSlot.ClearSlot();
            }
            UiSound.uiSound.EquipSound();
            skill = DragSkillSlot.instance.dragSkillSlot.skill;
            imageSkill.sprite = skill.SkillImage;
            SetColor(1);



        }


        else if (gameObject.layer == LayerMask.NameToLayer("quikSlot") && DragSkillSlot.instance.dragSkillSlot.gameObject.layer == LayerMask.NameToLayer("quikSlot")) // ������ -> ������
        {
            if (gameObject.GetComponent<QuikSlot>().slot.item != null) // �Һ�������� �ִٸ�
            {


                Slot instanceSlot = DragSkillSlot.instance.dragSkillSlot.gameObject.GetComponent<QuikSlot>().slot;
                instanceSlot.AddItem(gameObject.GetComponent<QuikSlot>().slot.item, gameObject.GetComponent<QuikSlot>().slot.itemCount);

                gameObject.GetComponent<QuikSlot>().slot.item = null;
                gameObject.GetComponent<QuikSlot>().slot.itemCount = 0;
                gameObject.GetComponent<QuikSlot>().slot.text_Count.text = "";

                skill = DragSkillSlot.instance.dragSkillSlot.skill;
                DragSkillSlot.instance.dragSkillSlot.ClearSlot();
                imageSkill.sprite = skill.SkillImage;
                DragSkillSlot.instance.dragSkillSlot.SetColor(1);
                SetColor(1);
                UiSound.uiSound.EquipSound();
                instanceSlot.itemImage.sprite = instanceSlot.item.itemImage;



            }

            else if (skill != null)
            {
                SkillUI instanceSkill = DragSkillSlot.instance.dragSkillSlot.skill;
                Debug.Log("1");
                DragSkillSlot.instance.dragSkillSlot.skill = skill;
                DragSkillSlot.instance.dragSkillSlot.Setimage();
                UiSound.uiSound.EquipSound();
                skill = instanceSkill;
                imageSkill.sprite = skill.SkillImage;
                SetColor(1);


            }
            else if (skill == null)
            {
                SkillUI instanceSkill = DragSkillSlot.instance.dragSkillSlot.skill;
                DragSkillSlot.instance.dragSkillSlot.ClearSlot();
                UiSound.uiSound.EquipSound();
                skill = instanceSkill;
                imageSkill.sprite = skill.SkillImage;
                SetColor(1);

            }


            skillToolTip.ToolTipOff();

        }
        if (gameObject.layer == LayerMask.NameToLayer("quikSlot"))
            gameObject.GetComponent<QuikSlot>().setCoolImage();
        if (DragSkillSlot.instance.dragSkillSlot.gameObject.layer == LayerMask.NameToLayer("quikSlot"))
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skill != null)
        {

            Vector2 SkillPosition;
            SkillPosition = eventData.position;
            if (eventData.position.x + 400f > 1920f)
                SkillPosition.x = 1920f - 400f;
            if (eventData.position.y - 500f < 0f)
                SkillPosition.y = 500f;
            skillToolTip.ToolTipOn(skill, SkillPosition); // �κ��丮��  0 , �������Ǹ�â�� 1  // �ǸŰ�尡 �ٸ��� ������ �����̴�.
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (skill != null)
        {

            skillToolTip.ToolTipOff();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {


            Debug.Log(1);
            if (skill.skillCharacter == SkillUI.SkillCharacter.Common)
            {
                if(playerST.CharacterType == PlayerST.Type.Warrior)
                    videoPlayer.clip = skill.skillVideo[0];
                else if (playerST.CharacterType == PlayerST.Type.Archer)
                    videoPlayer.clip = skill.skillVideo[1];
                else if (playerST.CharacterType == PlayerST.Type.Mage)
                    videoPlayer.clip = skill.skillVideo[2];
            }
            else
            {
                videoPlayer.clip = skill.skillVideo[0];
            }

            Debug.Log(videoPlayer.clip);
            videoPlayer.Play();
        
    }
}
