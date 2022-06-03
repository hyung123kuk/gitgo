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
    public Image hpBarImage;
    public Image backHpBarImage;
    public Image hpBarFlame;
    public Image levelFlame;
    public Text level;
    public Text Monstername;
    private Transform tr;
    public GameObject Geteff;
    public float maxHealth; //최대hp
    public float curHealth; //현재hp

    public float explPower;
    public float upPower;

    public float itemUpPoint;
    public float coin;
    public float coinCount=3;

    public bool isSpread = false;
    
    //쉐이킹 관련
    float allX = 0;
    float allY = 0;
    float allZ = 0;
    public float shakeTime = 0f;

    public float Exp;


    

    public void Start()
    {
        MonsterDropSet();
        tr = GetComponent<Transform>();
        Geteff = Resources.Load<GameObject>("GetEff");
 
    }



    public void ShakeOn()
    {         
            shakeTime = Time.time;
            StartCoroutine(HitShake());        
    }

    IEnumerator HitShake()
    {
        yield return new WaitForSeconds(0.05f);
        float progress = 0;
        float increment = 0.03f;
        while (progress <= 0.15f)
        {

            float x = Random.Range(-0.1f, 0.1f);

            float z = Random.Range(-0.1f, 0.1f);
            allX += x;
            allZ += z;
            tr.position += new Vector3(x, 0, z);
            progress += increment;
            yield return new WaitForSeconds(increment);


        }        
        transform.position -= new Vector3(allX, allY, allZ);
        allX = 0; allY = 0; allZ = 0;
    }




    public void MonsterDropSet()
    {
        Portion = Resources.LoadAll<GameObject>("DROP/Portion");
        Coin = Resources.Load<GameObject>("DROP/Coin");
       
    }

    public void StartMonster()
    {
        StartHpbar();
        SetColor(0);
        isSpread = false;
    }

    private void StartHpbar()
    {
        curHealth = maxHealth;
        hpBarPrefab = Resources.Load<GameObject>("Enemy_HpBar");

        uiCanvas = GameObject.Find("EnemyHp_Bar_UI").GetComponent<Canvas>();
        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        hpBarFlame = hpBar.GetComponentsInChildren<Image>()[0];
        backHpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[2];
        level = hpBar.transform.GetChild(2).GetChild(0).GetComponent<Text>();
        levelFlame = hpBar.transform.GetChild(2).GetComponent<Image>();
        Monstername = hpBar.transform.GetChild(3).GetComponent<Text>();

        var _hpbar = hpBar.GetComponent<EnemyHpBar>();
        _hpbar.targetTr = this.gameObject.transform;
        _hpbar.offset = hpBarOffset;
    }

    public void SetHpBar()
    {
        hpBarImage.fillAmount = curHealth / maxHealth;
        StartCoroutine(MonsterHpBarOn());
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

        
        for (int i = 0; i < coinCount + Random.Range(-1, 2); i++)
        {
                    
             int point = Random.Range(-1, 2);
              GameObject coinInstan = Instantiate(Coin, transform.position + new Vector3(point * 0.5f, itemUpPoint, point * 0.5f), Quaternion.identity);
            coinInstan.GetComponent<DropCoin>().SetCoin(coin);
        }

        
        
    }

    public void MonsterDie()
    {
        GameObject eff = Instantiate(Geteff, transform.position, Quaternion.identity);
        eff.GetComponent<GetEff>().SetExp(Exp);
        SetColor(0);
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

    public void MonsterAttack()
    {
        StartCoroutine(MonsterHpBarOn());
            
    }

    IEnumerator MonsterHpBarOn()
    {
        SetColor(1);
        yield return new WaitForSeconds(5.0f);
        SetColor(0);
    }

    public void SetColor(float _alpha)
    {
        Color color = backHpBarImage.color;
        color.a = _alpha;
        backHpBarImage.color = color;

        color = levelFlame.color;
        color.a = _alpha;
        levelFlame.color = color;

        color = hpBarFlame.color;
        color.a = _alpha;
        hpBarFlame.color = color;

        color = hpBarImage.color;
        color.a = _alpha;
        hpBarImage.color = color;

        color = level.color;
        color.a = _alpha;
        level.color = color;

        color = Monstername.color;
        color.a = _alpha;
        Monstername.color = color;

        
    }

}
