using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] SlimePoints;   //스폰포인트 
    public int SlimeObjs;             //현재 살아있는 몬스터수
    bool Slimeready;                  //스폰준비완료?
    public int slimeidx = 0;          //동시 소환 제어

    public Transform[] BlueSlimePoints;
    public int BlueSlimeObjs;
    bool BlueSlimeready;                  
    public int blueslimeidx = 0;
    
    public Transform[] GoblinPoints;
    public int GoblinObjs;
    bool Goblinready;
    public int goblinidx = 0;

    public Transform[] GoblinArcherPoints;
    public int GoblinArObjs;
    bool GoblinArcherready;
    public int goblinarcheridx = 0;

    public string[] monsters;

    public static SpawnManager spawnManager;
    private float slimetime;
    private float blueslimetime;
    private float goblintime;
    private float goblinarchertime;

    private void Awake()
    { 
        SlimeObjs = 20;
        BlueSlimeObjs = 20;
        GoblinObjs = 20;
        GoblinArObjs = 10;
        spawnManager = this;
        monsters = new string[] { "Slime", "BlueSlime", "Goblin", "GoblinArcher", "Skeleton", "TurtleSlime", "Golem" };
    }
    private void Start()
    {
        for (int i = 0; i < MonsterManager.monsterManager.Slime.Length; i++)
        {
            GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[0]);

            enemy.transform.position = SlimePoints[i].position;
            enemy.GetComponent<EnemySlime>().respawn = SlimePoints[i];
            SlimePoints[i].GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < MonsterManager.monsterManager.BlueSlime.Length; i++)
        {
            GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[1]);

            enemy.transform.position = BlueSlimePoints[i].position;
            enemy.GetComponent<EnemyBlueSlime>().respawn = BlueSlimePoints[i];
            BlueSlimePoints[i].GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < MonsterManager.monsterManager.Goblin.Length; i++)
        {
            GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[2]);

            enemy.transform.position = GoblinPoints[i].position;
            enemy.GetComponent<EnemyGoblin>().respawn = GoblinPoints[i];
            GoblinPoints[i].GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < MonsterManager.monsterManager.GoblinArcher.Length; i++)
        {
            GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[3]);

            enemy.transform.position = GoblinArcherPoints[i].position;
            enemy.GetComponent<EnemyRange>().respawn = GoblinArcherPoints[i];
            GoblinArcherPoints[i].GetChild(0).gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (SlimeObjs < 20)
        {
            SlimeObjs++;
            StartCoroutine(SlimeSpawn());
        }
        if (BlueSlimeObjs < 20)
        {
            BlueSlimeObjs++;
            StartCoroutine(BlueSlimeSpawn());
        }
        if (GoblinObjs < 20)
        {
            GoblinObjs++;
            StartCoroutine(GoblinSpawn());
        }
        if (GoblinArObjs < 10)
        {
            GoblinArObjs++;
            StartCoroutine(GoblinArSpawn());
        }
    }
    IEnumerator SlimeSpawn()
    {
        while (true)
        {
            slimetime = Random.Range(5, 9);
            yield return new WaitForSeconds(slimetime);

            for (int i = 0; i < MonsterManager.monsterManager.Slime.Length; i++)
            {
                slimeidx=i;
                if (SlimePoints[i].GetChild(0).gameObject.activeSelf)
                {
                    Slimeready = true;
                    break;
                }
            }

            if (Slimeready)
            {
                
                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[0]);

                enemy.transform.position = SlimePoints[slimeidx].position;
                enemy.GetComponent<EnemySlime>().respawn = SlimePoints[slimeidx];
                SlimePoints[slimeidx].GetChild(0).gameObject.SetActive(false);
                slimeidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //무한루프로 유니티 뻗는거 방지
        }
    }
    IEnumerator BlueSlimeSpawn()
    {
        while (true)
        {
            blueslimetime = Random.Range(5, 9);
            yield return new WaitForSeconds(blueslimetime);

            for (int i = 0; i < MonsterManager.monsterManager.BlueSlime.Length; i++)
            {
                blueslimeidx = i;
                if (BlueSlimePoints[i].GetChild(0).gameObject.activeSelf)
                {
                    BlueSlimeready = true;
                    break;
                }
            }

            if (BlueSlimeready)
            {

                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[1]);

                enemy.transform.position = BlueSlimePoints[blueslimeidx].position;
                enemy.GetComponent<EnemyBlueSlime>().respawn = BlueSlimePoints[blueslimeidx];
                BlueSlimePoints[blueslimeidx].GetChild(0).gameObject.SetActive(false);
                blueslimeidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //무한루프로 유니티 뻗는거 방지
        }
    }
    IEnumerator GoblinSpawn()
    {
        while (true)
        {
            goblintime = Random.Range(5, 9);
            yield return new WaitForSeconds(goblintime);

            for (int i = 0; i < MonsterManager.monsterManager.Goblin.Length; i++)
            {
                goblinidx = i;
                if (GoblinPoints[i].GetChild(0).gameObject.activeSelf)
                {
                    Goblinready = true;
                    break;
                }
            }

            if (Goblinready)
            {

                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[2]);

                enemy.transform.position = GoblinPoints[goblinidx].position;
                enemy.GetComponent<EnemyGoblin>().respawn = GoblinPoints[goblinidx];
                GoblinPoints[goblinidx].GetChild(0).gameObject.SetActive(false);
                goblinidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //무한루프로 유니티 뻗는거 방지
        }
    }
    IEnumerator GoblinArSpawn()
    {
        while (true)
        {
            goblinarchertime = Random.Range(5, 9);
            yield return new WaitForSeconds(goblinarchertime);

            for (int i = 0; i < MonsterManager.monsterManager.GoblinArcher.Length; i++)
            {
                goblinarcheridx = i;
                if (GoblinArcherPoints[i].GetChild(0).gameObject.activeSelf)
                {
                    GoblinArcherready = true;
                    break;
                }
            }

            if (GoblinArcherready)
            {

                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[3]);

                enemy.transform.position = GoblinArcherPoints[goblinarcheridx].position;
                enemy.GetComponent<EnemyRange>().respawn = GoblinArcherPoints[goblinarcheridx];
                GoblinArcherPoints[goblinarcheridx].GetChild(0).gameObject.SetActive(false);
                goblinarcheridx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //무한루프로 유니티 뻗는거 방지
        }
    }
}
