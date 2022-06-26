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


    public float CursorX = 0.3f; 
    public float CursorY;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {
        
        Init_Cursor();
    }
    private void Update()
    {
       
        Update_MousePosition();
        Init_Cursor();
        

    }

    

    public void Init_Cursor()
    {
        Cursor.visible = false;
        
        transform_cursor.pivot = Vector2.up +new Vector2(CursorX, CursorY);
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
