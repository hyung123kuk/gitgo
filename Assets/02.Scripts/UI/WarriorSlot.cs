using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorSlot : MonoBehaviour
{
    public Slot weapon;
    public Slot chest;
    public Slot pants;
    public Slot helm;
    public Slot shoulder;
    public Slot boots;
    public Slot gloves;
    public Slot cloak;
    public Image InvenCharacterImage;
    private PlayerST playerSt;

    public Sprite manImage;
    public Sprite womanImage;

    private void Start()
    {
        playerSt = FindObjectOfType<PlayerST>();
        if (playerSt.CharacterType == PlayerST.Type.Warrior)
        {
            InvenCharacterImage.sprite = manImage;
            shoulder.gameObject.SetActive(true);


        }
        else if (playerSt.CharacterType == PlayerST.Type.Archer)
        {
            
            InvenCharacterImage.sprite = womanImage;
            shoulder.gameObject.SetActive(false);
            
        }
        else if (playerSt.CharacterType == PlayerST.Type.Mage)
        {
            InvenCharacterImage.sprite = manImage;
            shoulder.gameObject.SetActive(false);

        }

    }

}
