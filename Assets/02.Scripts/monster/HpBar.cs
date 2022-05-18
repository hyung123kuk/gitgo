using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);

    private Canvas uiCanvas;
    private Image hpBarImage;
    public float maxHealth; //최대hp
    public float curHealth; //현재hp



    public void StartHpBar()
    {
        curHealth = maxHealth;
        hpBarPrefab = Resources.Load<GameObject>("Enemy_HpBar");
     
        uiCanvas = GameObject.Find("EnemyHp_Bar_UI").GetComponent<Canvas>();
        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[2];

        var _hpbar = hpBar.GetComponent<EnemyHpBar>();
        _hpbar.targetTr = this.gameObject.transform;
        _hpbar.offset = hpBarOffset;
    }

    public void SetHpBar()
    {
        hpBarImage.fillAmount = curHealth / maxHealth;

    }

    


}
