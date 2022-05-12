using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public GameObject toolTip;

    public Text itemName;
    public Text itemLevel;
    public Text itemKinds;//아이템 종류 텍스트
    public Text itemPosition;
    public Text itemPrice;
    public Text itemAbility;
    public Text portionText;    
    public Text itemText;



    private PlayerST playerST;
    private PlayerStat playerStat;

    public Text[] DamDef;


    public Text[] AddAbility;

    private void Start()
    {
        playerST = FindObjectOfType<PlayerST>();
        playerStat = FindObjectOfType<PlayerStat>();
    }

    public void ToolTipOn(Item item,Vector2 toolTipPoint,int mode)
    {
        transform.position = toolTipPoint;
        if (mode == 0) // 인벤토리 창에서 뜨는 판매가
            itemPrice.text = "판매가 : " + Mathf.Round(item._PRICE * 0.66f).ToString();
        else if (mode == 1) //아이템 상점에서 뜨는 판매가
        {
            if (item._PRICE > playerStat.MONEY)
                itemPrice.text = "<color=#FF0000>구매가 : " + item._PRICE.ToString() + "</color>";
            else
                itemPrice.text = "구매가 : " + item._PRICE.ToString();
        }

        NameColor(item);
        LevelSet(item);
        ItemExplanationSet(item);
        TypeToKorean(item);
        EqueTypeToKorean(item);
        ItemAbilitySet(item);


        toolTip.SetActive(true);
    }
    public void ToolTipOff()
    {
        toolTip.SetActive(false);

    }


    private void ItemExplanationSet(Item item)
    {
        if (item.itemType == Item.ItemType.Equipment)
        {
            portionText.enabled = false;
            itemText.enabled = true;
            itemText.text = item.itemText; //아이템 설명창
        }
        else
        {
            itemText.enabled = false;
            portionText.enabled = true;
            portionText.text = item.itemText;
        }
    }

    private void LevelSet(Item item)
    {
        if (item.itemEquLevel != 0f)
        {
            itemLevel.enabled = true;
            if (item.itemEquLevel > playerStat.Level)
            {
                itemLevel.text = "<color=#FF0000>레벨 : " + item.itemEquLevel.ToString() + "</color>";
            }
            else
            {
                itemLevel.text = "레벨 : " + item.itemEquLevel.ToString();
            }
        }
        else
            itemLevel.enabled = false;
    }

    private void NameColor(Item item)
    {
        if (item.ItemGrade == Item.Type.normal)
        {
            if (item.itemEquLevel >= 6)
            {
                itemName.text = "<color=#9ACD32>" + item.itemName + "</color>";
            }
            else
            {
                itemName.text = "<color=#C0C0C0>" + item.itemName + "</color>";
            }
        }
        else if(item.ItemGrade == Item.Type.rare)
        {
            itemName.text = "<color=#FFD700>" + item.itemName + "</color>";
        }
        
    }

   

    void TypeToKorean(Item item)
    {       
        if (item.itemType == Item.ItemType.Equipment && (item.equipType == Item.EquipType.Sword || item.equipType == Item.EquipType.Bow || item.equipType == Item.EquipType.Staff))
        {
            itemKinds.text = "타입 : 무기";
        }
        else if (item.itemType == Item.ItemType.Equipment && item.armortype==Item.ArmorType.steel)
        {
            if(playerST.CharacterType == PlayerST.Type.Warrior)
                itemKinds.text = "타입 : 강철";
            else
                itemKinds.text = "<color=#FF0000>타입 : 강철</color>";
        }
        else if (item.itemType == Item.ItemType.Equipment && item.armortype == Item.ArmorType.leather)
        {
            if (playerST.CharacterType == PlayerST.Type.Archer)
                itemKinds.text = "타입 : 가죽";
            else
                itemKinds.text = "<color=#FF0000>타입 : 가죽</color>";
        }
        else if (item.itemType == Item.ItemType.Equipment && item.armortype == Item.ArmorType.cloth)
        {
            if (playerST.CharacterType == PlayerST.Type.Mage)
                itemKinds.text = "타입 : 천";
            else
                itemKinds.text = "<color=#FF0000>타입 : 천</color>";
        }
        else if(item.itemType == Item.ItemType.Used)
        {
            itemKinds.text = "타입 : 소모품";
        }
        else 
        {
            itemKinds.text = "타입 : 기타";
        }
    }

    void EqueTypeToKorean(Item item)
    {
        if (item.itemType == Item.ItemType.Equipment)
        {

            itemPosition.enabled = true;
            if (item.equipType == Item.EquipType.Sword)
            {
                if (playerST.CharacterType == PlayerST.Type.Warrior)
                { itemPosition.text = "종류 : 소드"; }
                else { itemPosition.text = "<color=#FF0000>종류 : 소드</color>"; }
            }
            else if (item.equipType == Item.EquipType.Bow)
            {
                if (playerST.CharacterType == PlayerST.Type.Archer)
                { itemPosition.text = "종류 : 보우"; }
                else { itemPosition.text = "<color=#FF0000>종류 : 보우</color>"; }
            }
            else if (item.equipType == Item.EquipType.Staff)
            {
                if (playerST.CharacterType == PlayerST.Type.Mage)
                { itemPosition.text = "종류 : 스태프"; }
                else { itemPosition.text = "<color=#FF0000>종류 : 스태프</color>"; }
            }
            else if (item.equipType == Item.EquipType.boots)
                itemPosition.text = "종류 : 신발";
            else if (item.equipType == Item.EquipType.chest)
                itemPosition.text = "종류 : 상의";
            else if (item.equipType == Item.EquipType.cloak)
                itemPosition.text = "종류 : 망토";
            else if (item.equipType == Item.EquipType.gloves)
                itemPosition.text = "종류 : 장갑";
            else if (item.equipType == Item.EquipType.helm)
                itemPosition.text = "종류 : 헬멧";
            else if (item.equipType == Item.EquipType.pants)
                itemPosition.text = "종류 : 하의";
            else if (item.equipType == Item.EquipType.shoulder)
                itemPosition.text = "종류 : 어깨";
        }
        else
        {
            itemPosition.enabled = false;
        }
    }

    void ItemAbilitySet(Item item)
    {
        for (int i = 0; i < DamDef.Length; i++)
            DamDef[i].text = "";
        for (int i = 0; i < AddAbility.Length; i++)
            AddAbility[i].text = "";


        int index = 0;
        if (item._DAMAGE != 0f)
        {
            DamDef[0].text = "<color=#FA8072>공격력 : " + item._DAMAGE + "</color>";
            index++;
        }

        if (item._DEFENCE != 0f)
        {
            DamDef[index].text = "<color=#ADD8E6>방어력 : " + item._DEFENCE + "</color>";
            index++;
        }


        if (item._HP != 0f)
        {
            DamDef[index].text = "<color=#8B0000>최대 HP : " + item._HP + "</color>";
            index++;
        }

        if (item._MP != 0f)
        {
            DamDef[index].text = "<color=#1E90FF>최대 MP : " + item._MP + "</color>";
            index++;
        }

        if (item._STR != 0f)
        {
            DamDef[index].text = "<color=#DC143C>힘 : " + item._STR + "  </color>";
        }

        if (item._DEX != 0f)
        {
            DamDef[index].text += "<color=#3CB371>민첩 : " + item._DEX + "  </color>";
        }

        if (item._INT != 0f)
        {
            DamDef[index].text += "<color=#00BFFF>지능 : " + item._INT + "  </color>";
        }

        index = 0;

        if (item._SKILL_COOLTIME_DEC_PER != 0f)
        {
            AddAbility[0].text = "<color=#00FF7F>스킬 쿨타임" + item._SKILL_COOLTIME_DEC_PER + "% 감소</color>";
            index++;
        }

        if (item._SKILL_ADD_DAMAGE_PER != 0f)
        {
            AddAbility[index].text = "<color=#00FF7F>스킬 데미지" + item._SKILL_ADD_DAMAGE_PER + "% 증가</color>";
            index++;
        }
        if (item._CRITICAL_PROBABILITY != 0f)
        {
            AddAbility[index].text = "<color=#00FF7F>크리티컬 확률" + item._CRITICAL_PROBABILITY + "% 증가</color>";
            index++;
        }
        if (item._CRITICAL_ADD_DAMAGE_PER != 0f)
        {
            AddAbility[index].text = "<color=#00FF7F>크리티컬 데미지" + item._CRITICAL_ADD_DAMAGE_PER + "% 증가</color>";
            index++;
        }
        if (item._MOVE_SPEED != 0f)
            AddAbility[index].text = "<color=#4169E1>이동속도" + item._MOVE_SPEED + "% 증가</color>";

    }


}
