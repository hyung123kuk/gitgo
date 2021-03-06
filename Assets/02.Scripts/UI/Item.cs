using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType  // 아이템 유형
    {
        Equipment,
        Used,
        Ingredient,
        ETC,
        Ride
    }
    public enum EquipType
    {
        Sword,
        Bow,
        Staff,
        boots,
        chest,
        cloak,
        gloves,
        helm,
        pants,
        shoulder,


    }
    public enum Type { normal, rare }
    public enum ArmorType { steel, leather, cloth, none }
    public ItemType itemType; // 아이템 유형
    public ArmorType armortype;
    public EquipType equipType;
    public Type ItemGrade;
    public int itemEquLevel; // 아이템 장착 레벨

    public enum SwordNames { Sword1, Sword5_normal, Sword5_rare, Sword10_normal, Sword10_rare, None };
    public SwordNames swordNames;

    public enum HelmetNames { Helmet1, Helmet3, Helmet7, None };
    public HelmetNames helmetNames;

    public enum ChestNames { Chest1, Chest3, Chest7, None };
    public ChestNames chestNames;

    public enum ShoulderNames { Shoulder1, Shoulder3, Shoulder7, None };
    public ShoulderNames shoulderNames;

    public enum GlovesNames { Gloves1, Gloves3, Gloves7, None };
    public GlovesNames glovesNames;

    public enum PantsNames { Pants1, Pants3, Pants7, None };
    public PantsNames pantsNames;

    public enum BootsNames { Boots1, Boots3, Boots7, None };
    public BootsNames bootsNames;

    public enum BackNames { Back1, Back3, Back7, None };
    public BackNames backNames;

    [TextArea]
    public string itemText;

    public string itemName; // 아이템의 이름

    public Sprite itemImage; // 아이템의 이미지(인벤 토리 안에서 띄울)
    public GameObject itemPrefab;  // 아이템의 프리팹 (아이템 생성시 프리팹으로 찍어냄)

    public float _PRICE;        //가격
    public float _STR;          //힘
    public float _DEX;          //덱
    public float _INT;          //지능
    public float _DAMAGE;       //공격력
    public float _DEFENCE;      //방어력
    public float _SKILL_COOLTIME_DEC_PER; //스킬 쿨타임 감소량
    public float _SKILL_ADD_DAMAGE_PER; // 스킬 추가 공격력
    public float _HP;               //체력
    public float _MP;               //마나
    public float _CRITICAL_PROBABILITY; //크리티컬확률
    public float _CRITICAL_ADD_DAMAGE_PER;//크리티커추가데미지
    public float _MOVE_SPEED;       //이동속도

}

