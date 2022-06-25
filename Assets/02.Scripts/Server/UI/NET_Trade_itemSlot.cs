using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NET_Trade_itemSlot : MonoBehaviour
{
    public int TradeSlotNum;
    public Item item;
    public Image itemImage;
    public Text Image_itemCount;
    public int itemCount;
 
    public void SetItem(Item _item , int itemnum)
    {
        if (_item == null)
        {
        
            item = null;
            Image_itemCount.text = "";
            SetColor(0);
        }
        else
        {
            itemCount = itemnum;
            item = _item;
            itemImage.sprite = _item.itemImage;
            Image_itemCount.text = itemnum.ToString();
            SetColor(1);
        }
    }


    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }


    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        Image_itemCount.text = "";
        SetColor(0);

      

    }
}
