using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class itemStore : MonoBehaviour,IPointerClickHandler
{
    public static bool sellButton = false;
    public GameObject Store;
    [SerializeField]
    private AllUI allUI;
    [SerializeField]
    private RectTransform itemStoreGrid;
    [SerializeField]
    private inventory inven;

   


    void Start()
    {
        allUI = FindObjectOfType<AllUI>();
        inven = FindObjectOfType<inventory>();
    }
    private void Update()
    {
        

        if (itemStoreGrid.anchoredPosition.y < 0)   // GRID 마우스 휠 제한
        {
            itemStoreGrid.anchoredPosition = new Vector2(0,0);
        }
        if (itemStoreGrid.anchoredPosition.y > 1000f)
        {
            itemStoreGrid.anchoredPosition = new Vector2(0, 1000);
        }


    }



    public void storeOn()
    {
        Store.SetActive(true);
        inven.invenOn();
        allUI.StoreTop();
    }

    public void storeOff()
    {
        allUI.MouseCursor.SetNormalCursor();
        Store.SetActive(false);
        
        sellButton = false;
       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        allUI.StoreTop();
    }
    public void SellButton()
    {
        sellButton = true;
        allUI.MouseCursor.SetSellCursor();
    }


}
