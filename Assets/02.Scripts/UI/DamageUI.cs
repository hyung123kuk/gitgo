using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    [SerializeField]
    private Camera uiCamera;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private RectTransform rectParent;
    [SerializeField]
    private RectTransform rectDamage;

    public Vector3 offset = Vector3.zero;
    public Vector3 UpPosition = Vector3.zero;
    public Transform targetTr;
    public float damp = 0.02f;

    public void Start()
    {
        canvas = GameObject.Find("EnemyHp_Bar_UI").GetComponent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectDamage = this.gameObject.GetComponent<RectTransform>();
        Destroy(gameObject,0.8f);
    }


    public void LateUpdate()
    {

        offset = Vector3.Lerp(offset, UpPosition, Time.deltaTime * damp);
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);

        rectDamage.localPosition = localPos;

        
    }

    
}
