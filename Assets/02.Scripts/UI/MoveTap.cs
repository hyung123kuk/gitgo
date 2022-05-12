using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveTap : MonoBehaviour,IDragHandler,IBeginDragHandler
{
    private Vector2 vecDistance;

    public void OnBeginDrag(PointerEventData eventData)
    {
        vecDistance = new Vector2(transform.position.x - eventData.position.x, transform.position.y - eventData.position.y);
        transform.position = eventData.position;

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + vecDistance;
    }



   
}
