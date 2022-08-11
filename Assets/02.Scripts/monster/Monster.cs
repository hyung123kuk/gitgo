using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Monster : MonoBehaviourPun
{

    public PlayerStat playerStat;
    public PlayerST playerST;
    public GameObject hpBarPrefab;
    public GameObject Damage;
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

    [SerializeField]
    public Weapons weapons;

    [SerializeField]
    AttackDamage attackdamage;
    public float maxHealth; //�ִ�hp
    public float curHealth; //����hp

    public float explPower;
    public float upPower;

    public float itemUpPoint;
    public float coin;
    public float coinCount = 3;

    public bool isSpread = false;

    //����ŷ ����
    float allX = 0;
    float allY = 0;
    float allZ = 0;
    public float shakeTime = 0f;

    public float Exp;

    public float hitDamage;
    public virtual void Die() { }
    
   
    public void Start()
    {
        MonsterDropSet();
        tr = GetComponent<Transform>();

        Coin = Resources.Load<GameObject>("DROP/Coin");
        Portion = Resources.LoadAll<GameObject>("DROP/Portion");
        Geteff = Resources.Load<GameObject>("GetEff");
        Damage = Resources.Load<GameObject>("Damage");
        //playerST = FindObjectOfType<PlayerST>();
        //playerStat = FindObjectOfType<PlayerStat>();
        //weapons = FindObjectOfType<Weapons>();
        PlayerST[] attackDam111ages = FindObjectsOfType<PlayerST>();

        foreach (PlayerST attDam in attackDam111ages)
        {
            if (attDam.GetComponent<PhotonView>().IsMine)
            {
                playerST = attDam;
            }
        }
        PlayerStat[] attackDam11ages = FindObjectsOfType<PlayerStat>();

        foreach (PlayerStat attDam in attackDam11ages)
        {
            if (attDam.GetComponent<PhotonView>().IsMine)
            {
                playerStat = attDam;
            }
        }
        Weapons[] attackDam1ages = FindObjectsOfType<Weapons>();

        foreach (Weapons attDam in attackDam1ages)
        {
            if (attDam.GetComponent<PhotonView>().IsMine)
            {
                weapons = attDam;
            }
        }

        AttackDamage[] attackDamages = FindObjectsOfType<AttackDamage>();

        foreach (AttackDamage attDam in attackDamages)
        {
            if (attDam.GetComponent<PhotonView>().IsMine)
            {
                attackdamage = attDam;
            }
        }

        photonView.RPC("MosterHpSet", RpcTarget.MasterClient);
        

    }

    [PunRPC]
    public void MosterHpSet()
    {
        photonView.RPC("MonsterHp", RpcTarget.All,maxHealth - curHealth);
    }

    [PunRPC]
    public void MonsterHp(float Damagedhealth)
    {
        curHealth = curHealth - Damagedhealth;
    }

    [PunRPC]
    public virtual void MonsterRespawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("MonsterRespawn", RpcTarget.Others);
        }
        gameObject.SetActive(true);
    }

    [PunRPC]
    public virtual void MonsterPosition(Vector3 pos)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("MonsterPosition", RpcTarget.Others, pos);
        }
        if (tr)
        {
            tr.position = pos;
        }
    }




    [PunRPC]
    public virtual void OnDamage(float _attackdamage, bool critical, bool Local)
    {
        Debug.Log(Local);
        if (Local) // �����϶� �ٸ������� ���� ���þƴϸ� �ߺ� �������� ����
        {
            photonView.RPC("OnDamage", RpcTarget.Others, _attackdamage, critical, false);
            SetTarget(attackdamage.gameObject);
            Debug.Log("local");
        }
        
        GameObject damage = Instantiate<GameObject>(Damage, uiCanvas.transform);
        
        if (!Local)
        {
            curHealth -= _attackdamage;
            SetTarget(null);
            Debug.Log("!local");
        }


        var _damage = damage.GetComponent<DamageUI>();
        _damage.targetTr = this.gameObject.transform;
        Text damagevalue = damage.GetComponent<Text>();
        if (!critical)
        {
            damage.transform.GetChild(0).gameObject.SetActive(false);
            damage.GetComponent<Outline>().enabled = false;
            damagevalue.color = Color.red;
        }

        
        string dam = ((int)_attackdamage).ToString();

        damagevalue.text = dam;
        SetHpBar();

        shakeTime = Time.time;
        if (gameObject.activeSelf)
            StartCoroutine(HitShake());

        if (curHealth <= 0)
        {
            Die();
        }

    }



    public void HitMonster()
    {
        if (playerST.CharacterType == PlayerST.Type.Warrior || weapons.GetComponent<PhotonView>().IsMine
            || playerST.CharacterType == PlayerST.Type.Archer || playerST.CharacterType == PlayerST.Type.Mage)
        {
            OnDamage(attackdamage.attackDamage, attackdamage.critical, true); //�¾����� ������ Ʈ����ؼ� �ٸ��������� OnDamage�� ����ǰ�

            DamageSet();
            BossHpBarSet();
        }
    }


    public void DamageSet()
    {

        if (playerST.CharacterType == PlayerST.Type.Warrior)
        {
            if (attackdamage.DamageNum == 0)
            {
                weapons.damage = attackdamage.Attack_Dam();


            }
            else if (attackdamage.DamageNum == 1)
            {
                weapons.damage = attackdamage.Skill_1_Damamge();

            }
            else if (attackdamage.DamageNum == 2)
            {
                weapons.damage = attackdamage.Skill_2_Damamge();
            }
            else if (attackdamage.DamageNum == 3)
            {
                weapons.damage = attackdamage.Skill_3_Damamge();
            }
            else if (attackdamage.DamageNum == 4)
            {
                weapons.damage = attackdamage.Skill_4_Damamge();
            }
        }
        else if (playerST.CharacterType == PlayerST.Type.Archer || playerST.CharacterType == PlayerST.Type.Mage)
        {

            if (attackdamage.DamageNum == 0)
            {
                if (!weapons.ShotFull)
                {
                    Arrow arrow = FindObjectOfType<Arrow>();
                    if (arrow != null)
                        arrow.damage = attackdamage.Attack_Dam();
                }
                else
                {
                    Arrow arrow = FindObjectOfType<Arrow>();
                    if (arrow != null)
                        arrow.damage = 1.3f * attackdamage.Attack_Dam();
                }

            }
            else if (attackdamage.DamageNum == 1)
            {
                Arrow arrow = FindObjectOfType<Arrow>();
                if (arrow != null)
                    arrow.damage = attackdamage.Skill_1_Damamge();

                ArrowSkill arrowskill = FindObjectOfType<ArrowSkill>();
                if (arrowskill != null)
                    arrowskill.damage = attackdamage.Skill_1_Damamge();
            }
            else if (attackdamage.DamageNum == 2)
            {
                Arrow arrow = FindObjectOfType<Arrow>();
                if (arrow != null)
                    arrow.damage = attackdamage.Skill_2_Damamge();

                ArrowSkill arrowskill = FindObjectOfType<ArrowSkill>();
                if (arrowskill != null)
                    arrowskill.damage = attackdamage.Skill_2_Damamge();
            }
            else if (attackdamage.DamageNum == 3)
            {
                if (!weapons.energyfullCharging)
                {
                    Arrow arrow = FindObjectOfType<Arrow>();
                    if (arrow != null)
                        arrow.damage = attackdamage.Skill_3_Damamge();

                    ArrowSkill arrowskill = FindObjectOfType<ArrowSkill>();
                    if (arrowskill != null)
                        arrowskill.damage = attackdamage.Skill_3_Damamge();
                }
                else
                {
                    Arrow arrow = FindObjectOfType<Arrow>();
                    if (arrow != null)
                        arrow.damage = 1.5f * attackdamage.Skill_3_Damamge();

                    ArrowSkill arrowskill = FindObjectOfType<ArrowSkill>();
                    if (arrowskill != null)
                        arrowskill.damage = 1.5f * attackdamage.Skill_3_Damamge();
                }
            }
            else if (attackdamage.DamageNum == 4)
            {
                Arrow arrow = FindObjectOfType<Arrow>();
                if (arrow != null)
                    arrow.damage = attackdamage.Skill_4_Damamge();

                ArrowSkill arrowskill = FindObjectOfType<ArrowSkill>();
                if (arrowskill != null)
                    arrowskill.damage = attackdamage.Skill_4_Damamge();
            }
        }
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
        Debug.Log("1");
        hpBarImage.fillAmount = curHealth / maxHealth;
        if (gameObject.activeSelf)
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
                    PhotonNetwork.Instantiate("DROP/Portion/" + Portion[i].name, transform.position + new Vector3(point * 0.5f, itemUpPoint, point * 0.5f), Quaternion.identity);
                }
            }
        }


        for (int i = 0; i < coinCount + Random.Range(-1, 2); i++)
        {

            int point = Random.Range(-1, 2);
            GameObject coinInstan = PhotonNetwork.Instantiate("DROP/" + Coin.name, transform.position + new Vector3(point * 0.5f, itemUpPoint, point * 0.5f), Quaternion.identity);
            coinInstan.GetComponent<DropCoin>().SetCoin(coin);
        }



    }

    GameObject effTarget;
    public void SetTarget(GameObject target)
    {
        effTarget = target;
    }

    public void EffInsatangiate()
    {
        GameObject eff = PhotonNetwork.Instantiate(Geteff.name, transform.position, Quaternion.identity);
        eff.GetComponent<GetEff>().SetExp(Exp);
        eff.GetComponent<GetEff>().Target(effTarget);
    }

    public void MonsterDie()
    {
        if (effTarget != null)
        {
            EffInsatangiate();
        }


        SetColor(0);
        if (PhotonNetwork.IsMasterClient)
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

                isSpread = true;
            }
        }
    }

    public void MonsterAttack()
    {
        StopCoroutine("MonsterHpBarOn");
        StartCoroutine(MonsterHpBarOn());



    }

    public virtual void BossHpBarSet()
    {

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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "RESET")
        {
            photonView.RPC("ResetMonster", RpcTarget.All);

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "RESET")
        {
            photonView.RPC("ResetMonster", RpcTarget.All);

        }
    }


    [PunRPC]
    public void ResetMonster()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

}
