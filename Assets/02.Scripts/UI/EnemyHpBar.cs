using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField]
    private Camera uiCamera;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private RectTransform rectParent;
    [SerializeField]
    private RectTransform rectHp;

    public Vector3 offset = Vector3.zero;
    public Transform targetTr;


    public void Start()
    {
        canvas = GameObject.Find("EnemyHp_Bar_UI").GetComponent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }


    public void LateUpdate()
    {
        if (targetTr == null)
        {
            Destroy(gameObject);
            return;
        }

        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);
        if(screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        rectHp.localPosition = localPos;

    }
}
