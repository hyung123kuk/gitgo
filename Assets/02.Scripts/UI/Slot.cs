using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Slot : MonoBehaviourPun, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage;  // 아이템의 이미지
    public WarriorSlot WarriorSlot;
    [SerializeField]
    public Text text_Count;
    [SerializeField]
    private AllUI allUI;
    [SerializeField]
    private GameUI gameUI;

    public inventory inven;

    public Slot empSlot;

    [SerializeField]
    public PlayerST playerSt;
    public WarriorEquipChange warriorEquipChange;
    public ArcherEquipChange archerEquipChange;
    public MageEquipChange mageEquipChange;
    public ToolTip tooltip;
    [SerializeField]
    private PlayerStat playerStat;
    public itemSellQuestion item_sell_question;
    public GameObject itemSellScope;
    [SerializeField]
    public NET_Trade net_trade;
    public int TradeSlotNum;

    private void Awake()
    {
        gameUI = FindObjectOfType<GameUI>();
        net_trade = FindObjectOfType<NET_Trade>();
        allUI = FindObjectOfType<AllUI>();
        inven = FindObjectOfType<inventory>();

        item_sell_question = FindObjectOfType<itemSellQuestion>();
        tooltip = FindObjectOfType<ToolTip>();


    }

    private void Start()
    {
        playerSt = FindObjectOfType<PlayerST>();
        archerEquipChange = FindObjectOfType<ArcherEquipChange>();
        warriorEquipChange = FindObjectOfType<WarriorEquipChange>();
        mageEquipChange = FindObjectOfType<MageEquipChange>();

        playerStat = FindObjectOfType<PlayerStat>();
        if (item != null)
        {
            itemImage.sprite = item.itemImage;


            if (item.itemType == Item.ItemType.Used)
            {

                text_Count.text = itemCount.ToString();
            }
            else
            {
                text_Count.text = "";

            }
            SetColor(1);
            Invoke("ItemLimitColorRed", 0.3f);
            ItemLimitColorRed();
        }
    }

    public void ItemLimitColorRed()
    {
        if (item == null)
            return;

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
           (item.equipType == Item.EquipType.Staff && playerSt.CharacterType == PlayerST.Type.Archer) ||
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


    // 아이템 이미지의 투명도 조절
    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 인벤토리에 새로운 아이템 슬롯 추가
    public void AddItem(Item _item, int _count = 1)
    {

        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;



        if (item.itemType == Item.ItemType.Used)
        {

            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "";
            if (gameObject.tag == "weaponslot")
            {
                playerSt.WeaponChange(item.swordNames);
                playerStat.StatAllUpdate();
                gameUI.bar_set();
            }


        }

        SetColor(1);

    }

    // 해당 슬롯의 아이템 갯수 업데이트
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
        if (item.itemName == "<color=#FFD700>마계의 신비한 영약</color>")
        {
            SoundManager.soundManager.DrinkSound();
            playerStat.RecoverHp(50);
            playerStat.RecoverMp(50);

        }
        else if (item.itemName == "<color=#FF0000>체력 회복 물약</color>")
        {
            SoundManager.soundManager.DrinkSound();
            playerStat.RecoverHp(25);
        }
        else if (item.itemName == "<color=#1E90FF>파란 마나 꽃</color>")
        {
            SoundManager.soundManager.DrinkSound();
            playerStat.RecoverMp(25);
        }
        else if (item.itemName == "코인")
        {
            UiSound.uiSound.GetCoinSound();
            playerStat.AddGold(100);
        }
        gameUI.bar_set();
    }

    // 해당 슬롯 하나 삭제
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        tooltip.ToolTipOff();
        text_Count.text = "";

    }
    public void OnPointerClick(PointerEventData eventData) //우클릭 장착
    {

        if (item != null && eventData.button == PointerEventData.InputButton.Left && itemStore.sellButton) //아이템 판매 버튼을 클릭했을경우 빠르게 판매 하도록
        {
            item_sell_question.SellQuestionOn(GetComponent<Slot>());
            return;
        }

        if (item != null && gameObject.layer == LayerMask.NameToLayer("TRADESLOT")) //트레이드 슬롯에서 아이템을 사용하는것을 막는다.
        {
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {

            if (item != null)
            {

                if (gameObject.layer == LayerMask.NameToLayer("equip")) //장착하고 있는 아이템 장착 빼기
                {
                    if (!inven.HasEmptySlot())
                    {
                        LogManager.logManager.Log("빈창이 없습니다", true);
                        Debug.Log("빈창이 없습니다."); return;
                    } //빈슬롯 없으면 못뺀다.
                    if (gameObject.tag == "weaponslot")
                    {
                        if (item.equipType == Item.EquipType.Sword && playerSt.CharacterType == PlayerST.Type.Warrior) //전사 무기 기본으로 세팅
                            playerSt.WeaponChange(playerSt.basicSword);
                        else if (item.equipType == Item.EquipType.Bow && playerSt.CharacterType == PlayerST.Type.Archer) // 궁수 무기 기본으로 세팅
                            playerSt.WeaponChange(playerSt.basicSword);
                        else if (item.equipType == Item.EquipType.Staff && playerSt.CharacterType == PlayerST.Type.Mage) //법사 무기 기본으로 세팅
                            playerSt.WeaponChange(playerSt.basicSword);
                    }
                    if (gameObject.tag == "helm")
                    {
                        if (item != null && playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel) //전사 머리 기본
                        {
                            warriorEquipChange.photonView.RPC("WarriorHelmetChange", RpcTarget.AllBuffered, 0);
                        }
                        else if (item != null && playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather) //궁수 머리 기본
                        {
                            archerEquipChange.photonView.RPC("ArcherHelmetChange", RpcTarget.AllBuffered, 0);
                        }
                        else if (item != null && playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth) //법사 머리 기본
                        {
                            mageEquipChange.photonView.RPC("MageHelmetChange", RpcTarget.AllBuffered, 0);
                        }
                    }
                    if (gameObject.tag == "shoulder")
                    {
                        if (item != null && playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel) //전사 어깨 기본
                        {
                            warriorEquipChange.photonView.RPC("WarriorShoulderChange", RpcTarget.AllBuffered, 0);
                        }
                    }
                    if (gameObject.tag == "chest")
                    {
                        if (item != null && playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel) //전사 상의 기본
                        {
                            warriorEquipChange.photonView.RPC("WarriorChestChange", RpcTarget.AllBuffered, 0);
                        }
                        else if (item != null && playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather) //궁수 상의 기본
                        {
                            archerEquipChange.photonView.RPC("ArcherChestChange", RpcTarget.AllBuffered, 0);
                        }
                        else if (item != null && playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth) //법사 상의 기본
                        {
                            mageEquipChange.photonView.RPC("MageChestChange", RpcTarget.AllBuffered, 0);
                        }
                    }
                    if (gameObject.tag == "gloves")
                    {
                        if (item != null && playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel) //전사 장갑 기본
                        {
                            warriorEquipChange.photonView.RPC("WarriorGlovesChange", RpcTarget.AllBuffered, 0);
                        }
                        else if (item != null && playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather) //궁수 장갑 기본
                        {
                            archerEquipChange.photonView.RPC("ArcherGlovesChange", RpcTarget.AllBuffered, 0);
                        }
                        else if (item != null && playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth) //법사 장갑 기본
                        {
                            mageEquipChange.photonView.RPC("MageGlovesChange", RpcTarget.AllBuffered, 0);
                        }
                    }
                    if (gameObject.tag == "pants")
                    {
                        if (item != null && playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel) //전사 하의 기본
                        {
                            warriorEquipChange.photonView.RPC("WarriorPantsChange", RpcTarget.AllBuffered, 0);
                        }
                        else if (item != null && playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather) //궁수 하의 기본
                        {
                            archerEquipChange.photonView.RPC("ArcherPantsChange", RpcTarget.AllBuffered, 0);
                        }
                        else if (item != null && playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth) //법사 하의 기본
                        {
                            mageEquipChange.photonView.RPC("MagePantsChange", RpcTarget.AllBuffered, 0);
                        }
                    }
                    if (gameObject.tag == "boots")
                    {
                        if (item != null && playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel) //전사 신발 기본
                        {
                            warriorEquipChange.photonView.RPC("WarriorBootsChange", RpcTarget.AllBuffered, 0);
                        }
                        else if (item != null && playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather) //궁수 신발 기본
                        {
                            archerEquipChange.photonView.RPC("ArcherBootsChange", RpcTarget.AllBuffered, 0);
                        }
                        else if (item != null && playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth) //법사 신발 기본
                        {
                            mageEquipChange.photonView.RPC("MageBootsChange", RpcTarget.AllBuffered, 0);
                        }
                    }
                    if (gameObject.tag == "cloak")
                    {
                        if (item != null && playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel) //전사 망토 기본
                        {
                            warriorEquipChange.photonView.RPC("WarriorBackChange", RpcTarget.AllBuffered, 0);
                        }
                        else if (item != null && playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather) //궁수 망토 기본
                        {
                            archerEquipChange.photonView.RPC("ArcherBackChange", RpcTarget.AllBuffered, 0);
                        }
                        else if (item != null && playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth) //법사 망토 기본
                        {
                            mageEquipChange.photonView.RPC("MageBackChange", RpcTarget.AllBuffered, 0);
                        }
                    }
                    UiSound.uiSound.EquipSound();
                    EmptySlotEq(); //빈 슬롯 찾아 넣기
                    tooltip.ToolTipOff();
                    playerStat.StatAllUpdate();
                    gameUI.bar_set();

                }



                else if (item.itemType == Item.ItemType.Equipment && item.itemEquLevel <= playerStat.Level) //아이템 장착하기
                {

                    WarriorSword(); //무기 장착하기 (전사용)
                    ArcherBow();    //무기 장착하기 (궁수용)
                    MageStaff();    //무기 장착하기 (법사용)



                    if (item != null && playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel) //전사 방어구 장착
                    {

                        shoulder(); //어깨 장착하기(전사용)
                        Chest(); //상의 장착하기
                        Cloak(); //망토 장착하기
                        Boots(); //신발 장착하기
                        gloves();//장갑 장착하기
                        helm();  //모자 장착하기
                        pants(); //하의 장착하기
                    }
                    else if (item != null && playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather) //궁수 방어구 장착
                    {

                        Chest(); //상의 장착하기
                        Cloak(); //망토 장착하기
                        Boots(); //신발 장착하기
                        gloves();//장갑 장착하기
                        helm();  //모자 장착하기
                        pants(); //하의 장착하기
                    }
                    else if (item != null && playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth) // 마법사 방어구 장착
                    {

                        Chest(); //상의 장착하기
                        Cloak(); //망토 장착하기
                        Boots(); //신발 장착하기
                        gloves();//장갑 장착하기
                        helm();  //모자 장착하기
                        pants(); //하의 장착하기
                    }
                    UiSound.uiSound.EquipSound();
                    tooltip.ToolTipOff();
                    playerStat.StatAllUpdate();
                    gameUI.bar_set();


                }
                else if (item.itemType == Item.ItemType.Used)
                {


                    Debug.Log(item.itemName + " 을 사용했습니다.");
                    UseItem();
                    SetSlotCount(-1);
                }
                else if (item.itemType == Item.ItemType.Ride)
                {
                    playerSt.HorseRide();
                }
            }
        }
    }

    private void shoulder()
    {
        if (item != null && item.equipType == Item.EquipType.shoulder)
        {
            if (item.armortype == Item.ArmorType.steel)
            {
                warriorEquipChange.photonView.RPC("WarriorShoulderChange", RpcTarget.AllBuffered, item.shoulderNames);
            }

            if (WarriorSlot.shoulder.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.shoulder);
            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.shoulder);
            }
        }
    }

    private void pants()
    {
        if (item != null && item.equipType == Item.EquipType.pants)
        {
            if (item.armortype == Item.ArmorType.steel)
            {
                warriorEquipChange.photonView.RPC("WarriorPantsChange", RpcTarget.AllBuffered, item.pantsNames);
            }
            else if (item.armortype == Item.ArmorType.leather)
            {
                archerEquipChange.photonView.RPC("ArcherPantsChange", RpcTarget.AllBuffered, item.pantsNames);
            }
            else if (item.armortype == Item.ArmorType.cloth)
            {
                mageEquipChange.photonView.RPC("MagePantsChange", RpcTarget.AllBuffered, item.pantsNames);
            }

            if (WarriorSlot.pants.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.pants);

            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.pants);
            }
        }
    }

    private void helm()
    {
        if (item != null && item.equipType == Item.EquipType.helm)
        {
            if (item.armortype == Item.ArmorType.steel)
            {
                warriorEquipChange.photonView.RPC("WarriorHelmetChange", RpcTarget.AllBuffered, item.helmetNames);
            }
            else if (item.armortype == Item.ArmorType.leather)
            {
                archerEquipChange.photonView.RPC("ArcherHelmetChange", RpcTarget.AllBuffered, item.helmetNames);
            }
            else if (item.armortype == Item.ArmorType.cloth)
            {
                mageEquipChange.photonView.RPC("MageHelmetChange", RpcTarget.AllBuffered, item.helmetNames);
            }

            if (WarriorSlot.helm.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.helm);

            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.helm);
            }
        }
    }

    private void gloves()
    {
        if (item != null && item.equipType == Item.EquipType.gloves)
        {
            if (item.armortype == Item.ArmorType.steel)
            {
                warriorEquipChange.photonView.RPC("WarriorGlovesChange", RpcTarget.AllBuffered, item.glovesNames);
            }
            else if (item.armortype == Item.ArmorType.leather)
            {
                archerEquipChange.photonView.RPC("ArcherGlovesChange", RpcTarget.AllBuffered, item.glovesNames);
            }
            else if (item.armortype == Item.ArmorType.cloth)
            {
                mageEquipChange.photonView.RPC("MageGlovesChange", RpcTarget.AllBuffered, item.glovesNames);
            }

            if (WarriorSlot.gloves.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.gloves);
            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.gloves);
            }
        }
    }

    private void Boots()
    {
        if (item != null && item.equipType == Item.EquipType.boots)
        {
            if (item.armortype == Item.ArmorType.steel)
            {
                warriorEquipChange.photonView.RPC("WarriorBootsChange", RpcTarget.AllBuffered, item.bootsNames);
            }
            else if (item.armortype == Item.ArmorType.leather)
            {
                archerEquipChange.photonView.RPC("ArcherBootsChange", RpcTarget.AllBuffered, item.bootsNames);
            }
            else if (item.armortype == Item.ArmorType.cloth)
            {
                mageEquipChange.photonView.RPC("MageBootsChange", RpcTarget.AllBuffered, item.bootsNames);
            }

            if (WarriorSlot.boots.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.boots);
            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.boots);
            }
        }
    }

    private void Cloak()
    {
        if (item != null && item.equipType == Item.EquipType.cloak)
        {
            if (item.armortype == Item.ArmorType.steel)
            {
                warriorEquipChange.photonView.RPC("WarriorBackChange", RpcTarget.AllBuffered, item.backNames);
            }
            else if (item.armortype == Item.ArmorType.leather)
            {
                archerEquipChange.photonView.RPC("ArcherBackChange", RpcTarget.AllBuffered, item.backNames);
            }
            else if (item.armortype == Item.ArmorType.cloth)
            {
                mageEquipChange.photonView.RPC("MageBackChange", RpcTarget.AllBuffered, item.backNames);
            }

            if (WarriorSlot.cloak.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.cloak);
            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.cloak);
            }
        }
    }

    private void Chest()
    {
        if (item != null && item.equipType == Item.EquipType.chest)
        {
            if (item.armortype == Item.ArmorType.steel)
            {
                warriorEquipChange.photonView.RPC("WarriorChestChange", RpcTarget.AllBuffered, item.chestNames);
            }
            else if (item.armortype == Item.ArmorType.leather)
            {
                archerEquipChange.photonView.RPC("ArcherChestChange", RpcTarget.AllBuffered, item.chestNames);
            }
            else if (item.armortype == Item.ArmorType.cloth)
            {
                mageEquipChange.photonView.RPC("MageChestChange", RpcTarget.AllBuffered, item.chestNames);
            }


            if (WarriorSlot.chest.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.chest);
            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.chest);
            }
        }
    }

    private void WarriorSword()
    {
        if (item != null && item.equipType == Item.EquipType.Sword && playerSt.CharacterType == PlayerST.Type.Warrior)
        {
            playerSt.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, item.swordNames);
            if (WarriorSlot.weapon.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.weapon);
            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.weapon);
            }
        }
    }
    private void ArcherBow()
    {
        if (item != null && item.equipType == Item.EquipType.Bow && playerSt.CharacterType == PlayerST.Type.Archer)
        {
            playerSt.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, item.swordNames);
            if (WarriorSlot.weapon.item != null) // ( 장착이 되어있을때 )
            {

                SwapSlot(WarriorSlot.weapon);

            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.weapon);
            }

        }
    }

    private void MageStaff()
    {
        if (item != null && item.equipType == Item.EquipType.Staff && playerSt.CharacterType == PlayerST.Type.Mage)
        {
            playerSt.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, item.swordNames);
            if (WarriorSlot.weapon.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.weapon);
            }
            else // ( 장착이 되어 있지 않을때 )
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
    } //슬롯 아이템 서로 바꾸기

    private void EqItem(Slot EqSlot)
    {
        EqSlot.item = item;
        item = null;
        EqSlot.itemImage.sprite = EqSlot.item.itemImage;
        EqSlot.SetColor(1);
        SetColor(0);
    } // 아이템 장착


    private void EmptySlotEq() //빈 슬롯 찾아서 넣기
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

        if (item != null)
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
        itemSellScope.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {

        if (DragSlot.instance.dragSlot == null)
            return;
        if (DragSlot.instance.dragSlot.gameObject == gameObject) //물약 마구드래그시 사라지는거 막기
            return;
        //판매    
        if (gameObject.tag == "sellslot")
        {
            item_sell_question.SellQuestionOn(DragSlot.instance.dragSlot.GetComponent<Slot>());

        }
        //퀵슬롯에 장비 넣기 막기
        else if (gameObject.layer == LayerMask.NameToLayer("quikSlot") && DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment) { }
        //퀵슬릇에서 인벤창으로 옮길때 장비  넣기 막기
        else if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("quikSlot") && item != null && item.itemType == Item.ItemType.Equipment) { }




        //장비 착용
        else if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("equip") && gameObject.layer == LayerMask.NameToLayer("equip")) { Debug.Log("장비창->장비창"); } // 장비창-> 장비창으로 옮기는거 막기
        else if (gameObject.layer == LayerMask.NameToLayer("equip") && DragSlot.instance.dragSlot.item.itemType != Item.ItemType.Equipment) { Debug.Log("장비아닌거->장비창"); } //슬롯의 장비 아닌 것-> 장비창으로 드래그시 장착을 막는 곳
        else if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("equip") && item != null && item.itemType != Item.ItemType.Equipment) { Debug.Log("장비창->장비아닌거"); } //장비창 -> 슬롯의 장비 아닌 것 드래그시 장착을 막는 곳
        else if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("equip") && item == null) //장비창 -> 빈 슬롯으로 보낼때 Null에러 막기 ( 무기는 프리팹 0제로 바꿔줘야함
        {

            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Sword)
            {
                playerSt.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, playerSt.basicSword);
                ChangeSlot();
            }
            else if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Bow)
            {
                playerSt.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, playerSt.basicSword);
                ChangeSlot();
            }
            else if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Staff)
            {
                playerSt.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, playerSt.basicSword);
                ChangeSlot();
            }


        }
        else if (gameObject.tag == "weaponslot" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level) // 일반슬롯 무기 -> 무기창으로 옮길때
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Sword && playerSt.CharacterType == PlayerST.Type.Warrior)
            {
                DragSlot.instance.dragSlot.playerSt.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.swordNames);
                ChangeSlot();
            }
            else if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Bow && playerSt.CharacterType == PlayerST.Type.Archer)
            {
                DragSlot.instance.dragSlot.playerSt.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.swordNames);
                ChangeSlot();
            }
            else if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Staff && playerSt.CharacterType == PlayerST.Type.Mage)
            {
                DragSlot.instance.dragSlot.playerSt.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.swordNames);
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "weaponslot" && item.itemEquLevel <= playerStat.Level) //무기창 -> 일반 슬롯으로 옮길떄
        {
            if (item.equipType == Item.EquipType.Sword && playerSt.CharacterType == PlayerST.Type.Warrior)
            {
                playerSt.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, item.swordNames);

                ChangeSlot();
            }
            else if (item.equipType == Item.EquipType.Bow && playerSt.CharacterType == PlayerST.Type.Archer)
            {
                playerSt.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, item.swordNames);

                ChangeSlot();
            }
            else if (item.equipType == Item.EquipType.Staff && playerSt.CharacterType == PlayerST.Type.Mage)
            {
                playerSt.photonView.RPC("WeaponChange", RpcTarget.AllBuffered, item.swordNames);

                ChangeSlot();
            }
        }
        else if (gameObject.tag == "chest" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.chest)
            {                                                                                              //캐릭터가 전사고 드래그 아이템이 강철
                if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel)
                {
                    DragSlot.instance.dragSlot.warriorEquipChange.photonView.RPC("WarriorChestChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.chestNames);
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather)
                {
                    DragSlot.instance.dragSlot.archerEquipChange.photonView.RPC("ArcherChestChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.chestNames);
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth)
                {
                    DragSlot.instance.dragSlot.mageEquipChange.photonView.RPC("MageChestChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.chestNames);
                    ChangeSlot();
                }
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "chest" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.chest)
            {
                if (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel)
                {
                    warriorEquipChange.photonView.RPC("WarriorChestChange", RpcTarget.AllBuffered, item.chestNames);
                    ChangeSlot();
                }
                else if (playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather)
                {
                    archerEquipChange.photonView.RPC("ArcherChestChange", RpcTarget.AllBuffered, item.chestNames);
                    ChangeSlot();
                }
                else if (playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth)
                {
                    mageEquipChange.photonView.RPC("MageChestChange", RpcTarget.AllBuffered, item.chestNames);
                    ChangeSlot();
                }
            }
        }
        else if (gameObject.tag == "pants" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.pants)
            {                                                                                              //캐릭터가 전사고 드래그 아이템이 강철
                if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel)
                {
                    DragSlot.instance.dragSlot.warriorEquipChange.photonView.RPC("WarriorPantsChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.pantsNames);
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather)
                {
                    DragSlot.instance.dragSlot.archerEquipChange.photonView.RPC("ArcherPantsChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.pantsNames);
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth)
                {
                    DragSlot.instance.dragSlot.mageEquipChange.photonView.RPC("MagePantsChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.pantsNames);
                    ChangeSlot();
                }
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "pants" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.pants)
            {
                if (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel)
                {
                    warriorEquipChange.photonView.RPC("WarriorPantsChange", RpcTarget.AllBuffered, item.pantsNames);
                    ChangeSlot();
                }
                else if (playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather)
                {
                    archerEquipChange.photonView.RPC("ArcherPantsChange", RpcTarget.AllBuffered, item.pantsNames);
                    ChangeSlot();
                }
                else if (playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth)
                {
                    mageEquipChange.photonView.RPC("MagePantsChange", RpcTarget.AllBuffered, item.pantsNames);
                    ChangeSlot();
                }
            }
        }

        else if (gameObject.tag == "helm" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.helm)
            {                                                                                              //캐릭터가 전사고 드래그 아이템이 강철
                if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel)
                {
                    DragSlot.instance.dragSlot.warriorEquipChange.photonView.RPC("WarriorHelmetChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.helmetNames);
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather)
                {
                    DragSlot.instance.dragSlot.archerEquipChange.photonView.RPC("ArcherHelmetChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.helmetNames);
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth)
                {
                    DragSlot.instance.dragSlot.mageEquipChange.photonView.RPC("MageHelmetChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.helmetNames);
                    ChangeSlot();
                }
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "helm" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.helm)
            {
                if (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel)
                {
                    warriorEquipChange.photonView.RPC("WarriorHelmetChange", RpcTarget.AllBuffered, item.helmetNames);
                    ChangeSlot();
                }
                else if (playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather)
                {
                    archerEquipChange.photonView.RPC("ArcherHelmetChange", RpcTarget.AllBuffered, item.helmetNames);
                    ChangeSlot();
                }
                else if (playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth)
                {
                    mageEquipChange.photonView.RPC("MageHelmetChange", RpcTarget.AllBuffered, item.helmetNames);
                    ChangeSlot();
                }
            }
        }

        else if (gameObject.tag == "shoulder" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.shoulder)
            {                                                                                              //캐릭터가 전사고 드래그 아이템이 강철
                if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel)
                {
                    DragSlot.instance.dragSlot.warriorEquipChange.photonView.RPC("WarriorShoulderChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.shoulderNames);
                    ChangeSlot();
                }
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "shoulder" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.shoulder)
            {
                if (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel)
                {
                    warriorEquipChange.photonView.RPC("WarriorShoulderChange", RpcTarget.AllBuffered, item.shoulderNames);
                    ChangeSlot();
                }
            }
        }

        else if (gameObject.tag == "boots" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.boots)
            {                                                                                              //캐릭터가 전사고 드래그 아이템이 강철
                if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel)
                {
                    DragSlot.instance.dragSlot.warriorEquipChange.photonView.RPC("WarriorBootsChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.bootsNames);
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather)
                {
                    DragSlot.instance.dragSlot.archerEquipChange.photonView.RPC("ArcherBootsChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.bootsNames);
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth)
                {
                    DragSlot.instance.dragSlot.mageEquipChange.photonView.RPC("MageBootsChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.bootsNames);
                    ChangeSlot();
                }
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "boots" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.boots)
            {
                if (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel)
                {
                    warriorEquipChange.photonView.RPC("WarriorBootsChange", RpcTarget.AllBuffered, item.bootsNames);
                    ChangeSlot();
                }
                else if (playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather)
                {
                    archerEquipChange.photonView.RPC("ArcherBootsChange", RpcTarget.AllBuffered, item.bootsNames);
                    ChangeSlot();
                }
                else if (playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth)
                {
                    mageEquipChange.photonView.RPC("MageBootsChange", RpcTarget.AllBuffered, item.bootsNames);
                    ChangeSlot();
                }
            }
        }

        else if (gameObject.tag == "gloves" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.gloves)
            {                                                                                              //캐릭터가 전사고 드래그 아이템이 강철
                if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel)
                {
                    DragSlot.instance.dragSlot.warriorEquipChange.photonView.RPC("WarriorGlovesChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.glovesNames);
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather)
                {
                    DragSlot.instance.dragSlot.archerEquipChange.photonView.RPC("ArcherGlovesChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.glovesNames);
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth)
                {
                    DragSlot.instance.dragSlot.mageEquipChange.photonView.RPC("MageGlovesChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.glovesNames);
                    ChangeSlot();
                }
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "gloves" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.gloves)
            {
                if (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel)
                {
                    warriorEquipChange.photonView.RPC("WarriorGlovesChange", RpcTarget.AllBuffered, item.glovesNames);
                    ChangeSlot();
                }
                else if (playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather)
                {
                    archerEquipChange.photonView.RPC("ArcherGlovesChange", RpcTarget.AllBuffered, item.glovesNames);
                    ChangeSlot();
                }
                else if (playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth)
                {
                    mageEquipChange.photonView.RPC("MageGlovesChange", RpcTarget.AllBuffered, item.glovesNames);
                    ChangeSlot();
                }
            }
        }

        else if (gameObject.tag == "cloak" && DragSlot.instance.dragSlot.item.itemEquLevel <= playerStat.Level)
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.cloak)
            {                                                                                              //캐릭터가 전사고 드래그 아이템이 강철
                if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Warrior && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.steel)
                {
                    DragSlot.instance.dragSlot.warriorEquipChange.photonView.RPC("WarriorBackChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.backNames);
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Archer && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.leather)
                {
                    DragSlot.instance.dragSlot.archerEquipChange.photonView.RPC("ArcherBackChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.backNames);
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.playerSt.CharacterType == PlayerST.Type.Mage && DragSlot.instance.dragSlot.item.armortype == Item.ArmorType.cloth)
                {
                    DragSlot.instance.dragSlot.mageEquipChange.photonView.RPC("MageBackChange", RpcTarget.AllBuffered, DragSlot.instance.dragSlot.item.backNames);
                    ChangeSlot();
                }
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "cloak" && item.itemEquLevel <= playerStat.Level)
        {
            if (item.equipType == Item.EquipType.cloak)
            {
                if (playerSt.CharacterType == PlayerST.Type.Warrior && item.armortype == Item.ArmorType.steel)
                {
                    warriorEquipChange.photonView.RPC("WarriorBackChange", RpcTarget.AllBuffered, item.backNames);
                    ChangeSlot();
                }
                else if (playerSt.CharacterType == PlayerST.Type.Archer && item.armortype == Item.ArmorType.leather)
                {
                    archerEquipChange.photonView.RPC("ArcherBackChange", RpcTarget.AllBuffered, item.backNames);
                    ChangeSlot();
                }
                else if (playerSt.CharacterType == PlayerST.Type.Mage && item.armortype == Item.ArmorType.cloth)
                {
                    mageEquipChange.photonView.RPC("MageBackChange", RpcTarget.AllBuffered, item.backNames);
                    ChangeSlot();
                }
            }
        }
        else if (DragSlot.instance.dragSlot != null && DragSlot.instance.dragSlot.gameObject.layer != LayerMask.NameToLayer("equip") && gameObject.layer != LayerMask.NameToLayer("equip")) //서로 장비슬롯이 아니면 교체
        {


            if (DragSlot.instance.dragSlot.item != null && item != null && DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Used && DragSlot.instance.dragSlot.item.itemName == item.itemName) // 드래그한 두개의 사용 아이템이 같을때 합쳐진다.
            {

                SetSlotCount(DragSlot.instance.dragSlot.itemCount);
                DragSlot.instance.dragSlot.SetSlotCount(DragSlot.instance.dragSlot.itemCount * -1);

            }



            else if (gameObject.GetComponent<QuikSlot>() != null && gameObject.GetComponent<QuikSlot>().skill.skill != null) //퀵 슬롯으로 옮길때 안에 스킬이 있으면
            {
                if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("inven")) //인벤에서 옮길때
                {
                    gameObject.GetComponent<QuikSlot>().skill.skill = null;

                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("quikSlot")) // 퀵슬롯 끼리 옮길때 (물약을 드래그해 스킬이있는 퀵슬롯에 드랍할때)
                {
                    DragSlot.instance.dragSlot.gameObject.GetComponent<QuikSlot>().skill.skill = gameObject.GetComponent<QuikSlot>().skill.skill;
                    gameObject.GetComponent<QuikSlot>().skill.ClearSlot();
                    DragSlot.instance.dragSlot.gameObject.GetComponent<QuikSlot>().skill.imageSkill.sprite = DragSlot.instance.dragSlot.gameObject.GetComponent<QuikSlot>().skill.skill.SkillImage;

                    gameObject.GetComponent<QuikSlot>().slot.AddItem(DragSlot.instance.dragSlot.gameObject.GetComponent<QuikSlot>().slot.item, DragSlot.instance.dragSlot.gameObject.GetComponent<QuikSlot>().slot.itemCount);
                    DragSlot.instance.dragSlot.gameObject.GetComponent<QuikSlot>().slot.item = null;
                    DragSlot.instance.dragSlot.gameObject.GetComponent<QuikSlot>().slot.itemCount = 0;
                    DragSlot.instance.dragSlot.gameObject.GetComponent<QuikSlot>().slot.text_Count.text = "";



                    UiSound.uiSound.EquipSound();
                }
                gameObject.GetComponent<QuikSlot>().setCoolImage();
                if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("quikSlot"))
                    DragSlot.instance.dragSlot.gameObject.GetComponent<QuikSlot>().setCoolImage();


            }
            else
            {
                Debug.Log("hello");
                ChangeSlot();
            }

        }

        if (gameObject.layer == LayerMask.NameToLayer("quikSlot"))
            gameObject.GetComponent<QuikSlot>().skill.skillToolTip.ToolTipOff();

        if (gameObject.layer == LayerMask.NameToLayer("TRADESLOT") || DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("TRADESLOT")) //거래창 슬롯이었을때 상대 거래창 초기화
        {

            net_trade.RaiseIteam();
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
        UiSound.uiSound.EquipSound();
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }


    // 툴팁 부분
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {

            Vector2 itemPosition;
            itemPosition = eventData.position;
            if (eventData.position.x + 400f > 1920f)
                itemPosition.x = 1920f - 400f;
            if (eventData.position.y - 500f < 0f)
                itemPosition.y = 500f;
            tooltip.ToolTipOn(item, itemPosition, 0); // 인벤토리는  0 , 아이템판매창은 1  // 판매골드가 다르게 나오기 때문이다.

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