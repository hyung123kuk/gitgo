using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class StoreSlot : MonoBehaviourPun, IPointerEnterHandler, IPointerExitHandler , IPointerClickHandler
{
    public Image itemImage;
    public Item item;
    public ToolTip tooltip;
    public itemBuyQuestion ItembuyQuestion;
    public PlayerST playerSt;
    private PlayerStat playerStat;
    

    void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
        PlayerST[] playerSts = FindObjectsOfType<PlayerST>();


        foreach (PlayerST myplayerSt in playerSts)
        {
            if (myplayerSt.GetComponent<PhotonView>().IsMine)
            {
                playerSt = myplayerSt;
                break;
            }
        }
        ItembuyQuestion = FindObjectOfType<itemBuyQuestion>();
        if (item != null)
        {
            itemImage.sprite = item.itemImage;
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


    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // Update is called once per frame
    void Update()
    {

    }

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

            tooltip.ToolTipOn(item, itemPosition,1); // 인벤토리는  0 , 아이템판매창은 1  // 판매골드가 다르게 나오기 때문이다.
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            
            tooltip.ToolTipOff();
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item!=null && (eventData.button == PointerEventData.InputButton.Right|| eventData.button == PointerEventData.InputButton.Left) &&playerStat.MONEY >=item._PRICE )
        {
           
            ItembuyQuestion.BuyQuestionOn(item);
        }
    }
}
