using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    [SerializeField]
    private GameObject Coin;
    [SerializeField]
    private GameObject[] Portion;
    private Canvas uiCanvas;
    private Image hpBarImage;
    
    public float maxHealth; //최대hp
    public float curHealth; //현재hp

    public float explPower;
    public float upPower;

    public float itemUpPoint;
    public float coin;
    public float coinCount=3;
    public bool isSpread = false;
    public void Start()
    {
        MonsterDropSet();
    }

    public void MonsterDropSet()
    {
        Portion = Resources.LoadAll<GameObject>("DROP/Portion");
        Coin = Resources.Load<GameObject>("DROP/Coin");
    }

    public void StartMonster()
    {
        StartHpbar();        
    }

    private void StartHpbar()
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


    public void MonsterDrop()
    {
        

            for (int i = 0; i < Portion.Length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (Random.Range(0, 10) < 1)
                    {
                    int point = Random.Range(-1, 2);
                    Instantiate(Portion[i], transform.position + new Vector3(point * 0.5f, itemUpPoint, point * 0.5f), Quaternion.identity);
                    }
                }
            }

        coinCount += Random.Range(-1, 2);
        for (int i = 0; i < coinCount; i++)
        {
                    
             int point = Random.Range(-1, 2);
              GameObject coinInstan = Instantiate(Coin, transform.position + new Vector3(point * 0.5f, itemUpPoint, point * 0.5f), Quaternion.identity);
            coinInstan.GetComponent<DropCoin>().SetCoin(coin);
        }

        
        
    }

    public void MonsterDie()
    {
        if (!isSpread)
        {
            MonsterDrop();
            Collider[] cols = Physics.OverlapSphere(transform.position, 1.5f);
            foreach (Collider col in cols)
            {
                Rigidbody _rb = col.GetComponent<Rigidbody>();
                if (_rb != null)
                {
                    _rb.mass = 1.0f;
                    _rb.AddExplosionForce(explPower, transform.position + (Vector3.up * itemUpPoint), 1.5f, upPower);
                }
            }
        }
        isSpread = true;
    }





}
