using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item; // ȹ���� ������
    public int itemCount; // ȹ���� �������� ����
    public Image itemImage;  // �������� �̹���
    public WarriorSlot WarriorSlot;
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private AllUI allUI;
    [SerializeField]
    private GameUI gameUI;

    public inventory inven;

    public Slot empSlot;
    
    public PlayerST playerSt;
    public ToolTip tooltip;
    private PlayerStat playerStat;
    private itemSellQuestion item_sell_question;
    public GameObject itemSellScope;

    private void Start()
    {
        gameUI = FindObjectOfType<GameUI>();
        playerSt = FindObjectOfType<PlayerST>();
        allUI = FindObjectOfType<AllUI>();
        inven = FindObjectOfType<inventory>();
        playerStat = FindObjectOfType<PlayerStat>();
        item_sell_question = FindObjectOfType<itemSellQuestion>();
       
        if (item != null)
        {
            itemImage.sprite = item.itemImage;
            

            if (item.itemType != Item.ItemType.Equipment)
            {

                text_Count.text = itemCount.ToString();
            }
            else
            {
                text_Count.text = "";

            }
            SetColor(1);
            ItemLimitColorRed();
        }


    }

    public void ItemLimitColorRed()
    {
        if (item.itemEquLevel > playerStat.Level ||
           (item.armortype == Item.ArmorType.cloth && playerSt.CharacterType == PlayerST.Type.Archer) ||
            (item.armortype == Item.ArmorType.cloth && playerSt.CharacterType == PlayerST.Type.Warrior) ||
           (item.armortype == Item.ArmorType.leather && playerSt.CharacterType == PlayerST.Type.Warrior) ||
           (item.armortype == Item.ArmorType.leather && playerSt.CharacterType == PlayerST.Type.Mage) ||
           (item.armortype == Item.ArmorType.steel && playerSt.CharacterType == PlayerST.Type.Mage) ||
           (item.armortype == Item.ArmorType.steel && playerSt.CharacterType == PlayerST.Type.Archer) ||
           (item.equipType == Item.EquipType.Sword && playerSt.CharacterType == PlayerST.Type.Archer) ||
           (item.equipType == Item.EquipType.Sword && playerSt.CharacterType == PlayerST.Type.Mage) ||
           (item.equipType == Item.EquipType.Bow && playerSt.CharacterType == PlayerST.Type.Mage) ||
            (item.equipType == Item.EquipType.Bow && playerSt.CharacterType == PlayerST.Type.Warrior) ||
           (item.equipType == Item.EquipType.Staff && playerSt.CharacterType == PlayerST.Type.Archer)||
            (item.equipType == Item.EquipType.Staff && playerSt.CharacterType == PlayerST.Type.Warrior)
           )
        {
            itemImage.color = new Color(241 / 255f, 24 / 255f, 24 / 255f);
            
        }

        else
        {
            
            itemImage.color = Color.white;
        }



    }


    // ������ �̹����� ������ ����
    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // �κ��丮�� ���ο� ������ ���� �߰�
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
          
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "";
            
        }

        SetColor(1);
    }

    // �ش� ������ ������ ���� ������Ʈ
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();
        tooltip.ToolTipOff();
        if (itemCount <= 0)
            ClearSlot();
    }
    public void UseItem()
    {
        if (item.itemName == "<color=#FFD700>������ �ź��� ����</color>")
        {
            playerStat.RecoverHp(50);
            playerStat.RecoverMp(50);
            
        }
        else if (item.itemName == "<color=#FF0000>ü�� ȸ�� ����</color>")
        {
            playerStat.RecoverHp(25);
        }
        else if (item.itemName == "<color=#1E90FF>�Ķ� ���� ��</color>")
        {
            playerStat.RecoverMp(25);
        }
        gameUI.bar_set();
    }

    // �ش� ���� �ϳ� ����
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        tooltip.ToolTipOff();
        text_Count.text = "";
        
    }
    public void OnPointerClick(PointerEventData eventData) //��Ŭ�� ����
    {
        
        if(item !=null && eventData.button== PointerEventData.InputButton.Left && itemStore.sellButton)
        {
            item_sell_question.SellQuestionOn(GetComponent<Slot>());
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
            if (item != null) 
            {
                
                if ( gameObject.layer== LayerMask.NameToLayer("equip") ) //�����ϰ� �ִ� ������ ���� ����
                {
                    if (!inven.HasEmptySlot())
                    { Debug.Log("��â�� �����ϴ�."); return; } //�󽽷� ������ ������.
                    if (gameObject.tag == "weaponslot")
                    {
                        if (item.equipType == Item.EquipType.Sword && playerSt.CharacterType == PlayerST.Type.Warrior) //���� ���� �⺻���� ����
                            playerSt.WeaponChange(playerSt.basicSword);
                        else if (item.equipType == Item.EquipType.Bow && playerSt.CharacterType == PlayerST.Type.Archer) // �ü� ���� �⺻���� ����
                            ;
                        else if (item.equipType == Item.EquipType.Staff && playerSt.CharacterType == PlayerST.Type.Mage) //���� ���� �⺻���� ����
                            ;

                    }
                    
                    EmptySlotEq(); //�� ���� ã�� �ֱ�
                    tooltip.ToolTipOff();
                    playerStat.StatAllUpdate();
                    gameUI.bar_set();

                }



                else if (item.itemType == Item.ItemType.Equipment && item.itemEquLevel <= playerStat.Level) //������ �����ϱ�
                {
                 
                    WarriorSword(); //���� �����ϱ� (�����)
                    ArcherBow();    //���� �����ϱ� (�ü���)
                    MageStaff();    //���� �����ϱ� (�����)



                    if (item != null && playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel) //���� �� ����
                    {
                        shoulder(); //��� �����ϱ�(�����)
                        Chest(); //���� �����ϱ�
                        Cloak(); //���� �����ϱ�
                        Boots(); //�Ź� �����ϱ�
                        gloves();//�尩 �����ϱ�
                        helm();  //���� �����ϱ�
                        pants(); //���� �����ϱ�
                    }
                    else if(item != null && playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather) //�ü� �� ����
                    {
                        Chest(); //���� �����ϱ�
                        Cloak(); //���� �����ϱ�
                        Boots(); //�Ź� �����ϱ�
                        gloves();//�尩 �����ϱ�
                        helm();  //���� �����ϱ�
                        pants(); //���� �����ϱ�
                    }
                    else if(item != null && playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth) // ������ �� ����
                    {
                        Chest(); //���� �����ϱ�
                        Cloak(); //���� �����ϱ�
                        Boots(); //�Ź� �����ϱ�
                        gloves();//�尩 �����ϱ�
                        helm();  //���� �����ϱ�
                        pants(); //���� �����ϱ�
                    }
                    
                    tooltip.ToolTipOff();
                    playerStat.StatAllUpdate();
                    gameUI.bar_set();


                }
                else if ( item.itemType == Item.ItemType.Used)
                {
                    // �Һ�
                    
                    Debug.Log(item.itemName + " �� ����߽��ϴ�.");
                    UseItem();
                    SetSlotCount(-1);
                }
            }
        }
    }

    private void shoulder()
    {
        if (item != null && item.equipType == Item.EquipType.shoulder )
        {
            if (WarriorSlot.shoulder.item != null) // ( ������ �Ǿ������� )
            {                
                SwapSlot(WarriorSlot.shoulder);              
            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.shoulder);             
            }
        }
    }

    private void pants()
    {
        if (item != null && item.equipType == Item.EquipType.pants )
        {
            if (WarriorSlot.pants.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.pants);

            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.pants);
            }
       }
    }

    private void helm()
    {
        if (item != null && item.equipType == Item.EquipType.helm )
        {
            if (WarriorSlot.helm.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.helm);

            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.helm);
            }
        }
    }

    private void gloves()
    {
        if (item != null && item.equipType == Item.EquipType.gloves)
        {
            if (WarriorSlot.gloves.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.gloves);
            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.gloves);
            }
        }
    }

    private void Boots()
    {
        if (item != null && item.equipType == Item.EquipType.boots )
        {
            if (WarriorSlot.boots.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.boots);
           }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.boots);
            }
        }
    }

    private void Cloak()
    {
        if (item != null && item.equipType == Item.EquipType.cloak )
        {
            if (WarriorSlot.cloak.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.cloak);
            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.cloak);
            }
        }
    }

    private void Chest()
    {
        if (item != null && item.equipType == Item.EquipType.chest)
        {
            if (WarriorSlot.chest.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.chest);
            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.chest);
            }
        }
    }

    private void WarriorSword()
    {
        if (item != null && item.equipType == Item.EquipType.Sword && playerSt.CharacterType == PlayerST.Type.Warrior)
        {
            playerSt.WeaponChange(item.SwordNames);
            if (WarriorSlot.weapon.item != null) // ( ������ �Ǿ������� )
            {                
                SwapSlot(WarriorSlot.weapon);
            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.weapon);
            }
        }
    }
    private void ArcherBow()
    {
        if (item != null && item.equipType == Item.EquipType.Bow && playerSt.CharacterType == PlayerST.Type.Archer)
        {
            //playerSt.WeaponChange(item.SwordNames); (�ü� ���� ���������� ��������)


            if (WarriorSlot.weapon.item != null) // ( ������ �Ǿ������� )
            {

                SwapSlot(WarriorSlot.weapon);

            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.weapon);
            }

        }
    }

    private void MageStaff()
    {
        if (item != null && item.equipType == Item.EquipType.Staff && playerSt.CharacterType == PlayerST.Type.Mage)
        {
            //playerSt.WeaponChange(item.SwordNames); (������ ���� ���������� ��������)
            if (WarriorSlot.weapon.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.weapon);
            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.weapon);
            }
        }
    }



    private void SwapSlot(Slot swapSlot)
    {
        Item swapItem = item;
        item = swapSlot.item;
        swapSlot.item = swapItem;
        itemImage.sprite = item.itemImage;
        swapSlot.itemImage.sprite = swapSlot.item.itemImage;
    } //���� ������ ���� �ٲٱ�

    private void EqItem(Slot EqSlot )
    {        
        EqSlot.item = item;
        item = null;
        EqSlot.itemImage.sprite = EqSlot.item.itemImage;
        EqSlot.SetColor(1);
        SetColor(0);
    } // ������ ����


    private void EmptySlotEq() //�� ���� ã�Ƽ� �ֱ�
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.slots[i].item == null)
            {
                SetColor(0);
                inventory.slots[i].item = item;
                item = null;
                inventory.slots[i].itemImage.sprite = inventory.slots[i].item.itemImage;
                inventory.slots[i].SetColor(1);
                break;
            }
        }
    } 
    


    public void OnBeginDrag(PointerEventData eventData)
    {
        
        if (item!=null)
        {
            item_sell_question.itemSellScope.SetActive(true);
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
            allUI.InvenTop();
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot == null)
            return;
        //�Ǹ�    
        if (gameObject.tag == "sellslot")
        {
            item_sell_question.SellQuestionOn(DragSlot.instance.dragSlot.GetComponent<Slot>());

        }
        //�����Կ� ��� �ֱ� ����
        else if (gameObject.layer == LayerMask.NameToLayer("quikSlot") && DragSlot.instance.dragSlot.item.itemType != Item.ItemType.Used) { }
        //���������� �κ�â���� �ű涧 ���� �ٸ��� �ֱ� ����
        else if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("quikSlot") && item != null && item.itemType == Item.ItemType.Equipment) { }

        //��� ����
        else if (gameObject.layer == LayerMask.NameToLayer("equip") && DragSlot.instance.dragSlot.item.itemType != Item.ItemType.Equipment) { Debug.Log("���ƴѰ�->���â"); } //������ ��� �ƴ� ��-> ���â���� �巡�׽� ������ ���� ��
        else if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("equip") && item != null && item.itemType != Item.ItemType.Equipment) { Debug.Log("���â->���ƴѰ�"); } //���â -> ������ ��� �ƴ� �� �巡�׽� ������ ���� ��
        else if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("equip") && item == null) //���â -> �� �������� ������ Null���� ���� ( ����� ������ 0���� �ٲ������
        {

            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Sword)
            {
                playerSt.WeaponChange(playerSt.basicSword);
                ChangeSlot();
            }
            else if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Bow)
            {
                //�ü� �Ϲ� ����� ��ü
                ChangeSlot();
            }
            else if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Staff)
            {
                //���� �Ϲ� ����� ��ü
                ChangeSlot();
            }
            else
            {
                ChangeSlot();
            }
        }
        else if (gameObject.tag == "weaponslot" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level) // �Ϲݽ��� ���� -> ����â���� �ű涧
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Sword && playerSt.CharacterType == PlayerST.Type.Warrior)
            {
                DragSlot.instance.dragSlot.playerSt.WeaponChange(DragSlot.instance.dragSlot.item.SwordNames);
                ChangeSlot();
            }
            else if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Bow && playerSt.CharacterType == PlayerST.Type.Archer)
            {
                //�ü� ���� ��ü
                ChangeSlot();
            }
            else if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Staff && playerSt.CharacterType == PlayerST.Type.Mage)
            {
                //���� ���� ��ü
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "weaponslot" && item.itemEquLevel <= playerStat.Level) //����â -> �Ϲ� �������� �ű拚
        {
            if (item.equipType == Item.EquipType.Sword && playerSt.CharacterType == PlayerST.Type.Warrior)
            {
                playerSt.WeaponChange(item.SwordNames);
                ChangeSlot();
            }
            else if (item.equipType == Item.EquipType.Bow && playerSt.CharacterType == PlayerST.Type.Archer)
            {
                //�ü� �Ϲ� ����� ��ü

                ChangeSlot();
            }
            else if (item.equipType == Item.EquipType.Staff && playerSt.CharacterType == PlayerST.Type.Mage)
            {
                //���� �Ϲ� ����� ��ü

                ChangeSlot();
            }
        }
        else if (gameObject.tag == "chest" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.chest &&
               (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel || //ĳ���Ͱ� ����� �巡�� �������� ��ö �̰ų�
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather || //ĳ���Ͱ� �ü��� �巡�� �������� ���� �̰ų�
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth))      //ĳ���Ͱ� ������� �巡�� �������� õ �̰ų�
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "chest" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.chest &&
                (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel || //ĳ���Ͱ� ����� �ٲ� ����â�� �������� ��ö�̰ų�
                playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather || //ĳ���Ͱ� �ü��� �ٲ� ����â�� �������� �����̰ų�
                playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth))      //ĳ���Ͱ� ����� �ٲ� ����â�� �������� õ�̰ų�
            {
                ChangeSlot();
            }
        }
        else if (gameObject.tag == "pants" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.pants &&
               (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel ||
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather ||
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth))
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "pants" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.pants &&
                (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel ||
                playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather ||
                playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth))
            {
                ChangeSlot();
            }
        }

        else if (gameObject.tag == "helm" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.helm &&
               (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel ||
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather ||
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth))
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "helm" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.helm &&
                (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel ||
                playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather ||
                playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth))
            {
                ChangeSlot();
            }
        }

        else if (gameObject.tag == "shoulder" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.shoulder &&
               (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel ||
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather ||
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth))
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "shoulder" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.shoulder &&
                (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel ||
                playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather ||
                playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth))
            {
                ChangeSlot();
            }
        }

        else if (gameObject.tag == "boots" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.boots &&
               (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel ||
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather ||
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth))
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "boots" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.boots &&
                (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel ||
                playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather ||
                playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth))
            {
                ChangeSlot();
            }
        }

        else if (gameObject.tag == "gloves" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.gloves &&
               (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel ||
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather ||
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth))
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "gloves" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.gloves &&
                (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel ||
                playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather ||
                playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth))
            {
                ChangeSlot();
            }
        }

        else if (gameObject.tag == "cloak" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.cloak &&
               (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel ||
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather ||
               DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth))
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "cloak" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.cloak &&
                (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel ||
                playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather ||
                playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth))
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot != null && DragSlot.instance.dragSlot.gameObject.layer != LayerMask.NameToLayer("equip") && gameObject.layer != LayerMask.NameToLayer("equip")) //���� ��� �ƴϸ� ��ü
        {
            if (DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Used && DragSlot.instance.dragSlot.item == item) //���� �������� ��ģ��.
            {
                SetSlotCount(DragSlot.instance.dragSlot.itemCount);
                DragSlot.instance.dragSlot.SetSlotCount(DragSlot.instance.dragSlot.itemCount * -1);

            }
            else
            {
                ChangeSlot();
            }
            if (gameObject.GetComponent<QuikSlot>()!=null&& gameObject.GetComponent<QuikSlot>().skill.skill != null) //�� ���Կ� ��ų ������ ������ �ʱ�ȭ
            {
                gameObject.GetComponent<QuikSlot>().skill.skill = null;
            }

        }
        tooltip.ToolTipOff();
        playerStat.StatAllUpdate();
        gameUI.bar_set();
        if (item != null)
        {
            ItemLimitColorRed();
        }
        if (DragSlot.instance.dragSlot.item != null)
        {
            DragSlot.instance.dragSlot.ItemLimitColorRed();
        }
        
       
            item_sell_question.itemSellScope.SetActive(false);
        
    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if(_tempItem !=null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }


    // ���� �κ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
           
            Vector2 itemPosition;
            itemPosition = eventData.position;           
            if (eventData.position.x + 400f > 1920f)
                itemPosition.x =  1920f - 400f;
            if (eventData.position.y - 500f < 0f)
                itemPosition.y =  500f;                
            tooltip.ToolTipOn(item,itemPosition,0); // �κ��丮��  0 , �������Ǹ�â�� 1  // �ǸŰ�尡 �ٸ��� ������ �����̴�.
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
           
            tooltip.ToolTipOff();
        }
       
    }
}