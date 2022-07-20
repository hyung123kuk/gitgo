using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class MonsterBoss : Monster
{
    [SerializeField]
    public GameObject[] item;
    [SerializeField]
    protected GameObject bossHpBar;
    public Image BossHpBar;
    public Text BossHp;
    public string dropResourceString;
    public void StartBossMonster()
    {
        StartMonster();


    }
    public void BossMonsterHpBarSet()
    {
        BossHpBarSettting();

        bossHpBar.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "Lv." + level.text;
        bossHpBar.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = Monstername.text;
        BossHpBar = bossHpBar.transform.GetChild(2).GetChild(1).GetComponent<Image>();
        BossHp = bossHpBar.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>();

    }

    public virtual void BossHpBarSettting(){
    }

    public override void BossHpBarSet()
    {
        if (curHealth <= 0)
        {
            bossHpBar.SetActive(false);
            return;
        }
        Debug.Log("bossattac22k");
        if (bossHpBar != null)
        {
            StopCoroutine(BossHpOn());
            StartCoroutine(BossHpOn());
            BossHpBar.fillAmount = curHealth / maxHealth;
            BossHp.text = (int)curHealth + " / " + (int)maxHealth;
        }
    }


    public IEnumerator BossHpOn()
    {
        bossHpBar.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        bossHpBar.SetActive(false);
    }

    public void BossDrop()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!isSpread)
            {
                int rand = Random.Range(0, item.Length);

                for (int i = 0; i < item.Length; i++)
                {

                    if (rand == i)
                    {
                        int point = Random.Range(-1, 2);
                        string DropItem = item[i].name;
                       
                       GameObject ditem =  PhotonNetwork.Instantiate("DROP/" + dropResourceString+"/"+ DropItem, transform.position + Vector3.up + new Vector3(point * 0.5f, itemUpPoint, point * 0.5f), Quaternion.identity);
                        Debug.Log(ditem.transform);
                    }
                }

            }
        }
    }
}
