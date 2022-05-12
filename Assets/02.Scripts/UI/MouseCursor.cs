using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    public RectTransform transform_cursor;
    public RectTransform transform_icon;
    public Sprite CursorNormal;
    public Sprite CursorSell;


    private void Start()
    {
        Init_Cursor();
    }
    private void Update()
    {
       
        Update_MousePosition();
    }

    public void Init_Cursor()
    {
        Cursor.visible = false;
        
        transform_cursor.pivot = Vector2.up;
        if (transform_cursor)
        {
            if (transform_cursor.GetComponent<Graphic>())
                transform_cursor.GetComponent<Graphic>().raycastTarget = false;
        }
        if (transform_icon)
        {
            if (transform_icon.GetComponent<Graphic>())
                transform_icon.GetComponent<Graphic>().raycastTarget = false;
        }

    }

    
    private void Update_MousePosition()
    {

        Vector2 mousePos = Input.mousePosition;
        if (transform_cursor != null)
             transform_cursor.position = mousePos;
        if (transform_icon != null)
        {
            float w = transform_icon.rect.width;
            float h = transform_icon.rect.height;
                if(transform_cursor !=null)
                transform_icon.position = transform_cursor.position + (new Vector3(w, h) * 0.5f);
        }

        
    }

    public void SetSellCursor()
    {
        transform_cursor.gameObject.GetComponent<Image>().sprite = CursorSell;
    }
    public void SetNormalCursor()
    {
        transform_cursor.gameObject.GetComponent<Image>().sprite = CursorNormal;
    }

}
