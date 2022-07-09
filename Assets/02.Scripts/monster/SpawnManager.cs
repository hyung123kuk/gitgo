using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPun
{
    public Transform[] SlimePoints;   //��������Ʈ 
    public int SlimeObjs;             //���� ����ִ� ���ͼ�
    bool Slimeready;                  //�����غ�Ϸ�?
    public int slimeidx = 0;          //���� ��ȯ ����

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

    public Transform[] SkeletonPoints;
    public int SkeletonObjs;
    bool Skeletonready;
    public int Skeletonidx = 0;

    public Transform TurtleSlimePoint;
    public int TurtleSlimeObjs;
    bool TurtleSlimeready;
    public int TurtleSlimeidx = 0;

    public Transform GolemPoint;
    public int GolemObjs;
    bool Golemready;
    public int Golemidx = 0;

    public string[] monsters;

    public static SpawnManager spawnManager;
    private float slimetime;
    private float blueslimetime;
    private float goblintime;
    private float goblinarchertime;
    private float turtleslimetime;
    private float skeletontime;
    private float golemtime;


    private void Awake()
    {


        SlimeObjs = 20;
        BlueSlimeObjs = 20;
        GoblinObjs = 20;
        GoblinArObjs = 10;
        TurtleSlimeObjs = 1;
        SkeletonObjs = 20;
        GolemObjs = 1;
        spawnManager = this;
        monsters = new string[] { "Slime", "BlueSlime", "Goblin", "GoblinArcher", "Skeleton", "TurtleSlime", "Golem" };
    }
    public void start() //��ȯ�س���
    {
        for (int i = 0; i < MonsterManager.monsterManager.Slime.Length; i++)
        {
            GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[0]);

            enemy.transform.position = SlimePoints[i].position;
            enemy.GetComponent<EnemySlime>().respawn = SlimePoints[i];
            SlimePoints[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < MonsterManager.monsterManager.BlueSlime.Length; i++)
        {
            GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[1]);

            enemy.transform.position = BlueSlimePoints[i].position;
            enemy.GetComponent<EnemyBlueSlime>().respawn = BlueSlimePoints[i];
            BlueSlimePoints[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < MonsterManager.monsterManager.Goblin.Length; i++)
        {
            GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[2]);

            enemy.transform.position = GoblinPoints[i].position;
            enemy.GetComponent<EnemyGoblin>().respawn = GoblinPoints[i];
            GoblinPoints[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < MonsterManager.monsterManager.GoblinArcher.Length; i++)
        {
            GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[3]);

            enemy.transform.position = GoblinArcherPoints[i].position;
            enemy.GetComponent<EnemyRange>().respawn = GoblinArcherPoints[i];
            GoblinArcherPoints[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < MonsterManager.monsterManager.Skeleton.Length; i++)
        {
            GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[4]);

            enemy.transform.position = SkeletonPoints[i].position;
            enemy.GetComponent<EnemySkeleton>().respawn = SkeletonPoints[i];
            SkeletonPoints[i].gameObject.SetActive(false);
        }
        GameObject boss1 = MonsterManager.monsterManager.MakeObj(monsters[5]); // ��Ʋ������

        boss1.transform.position = TurtleSlimePoint.position; //��ġ����
        boss1.GetComponent<EnemyBoss1>().respawn = TurtleSlimePoint; //��������ġ����
        TurtleSlimePoint.gameObject.SetActive(false);  //���Ͱ� ����ִ��� ����ġ�뵵


        GameObject boss2 = MonsterManager.monsterManager.MakeObj(monsters[6]); // ��

        boss2.transform.position = GolemPoint.position; //��ġ����
        boss2.GetComponent<EnemyBoss2>().respawn = GolemPoint; //��������ġ����
        GolemPoint.gameObject.SetActive(false);  //���Ͱ� ����ִ��� ����ġ�뵵

    }
    
    private void Update()
    {

        if (!PhotonNetwork.IsMasterClient)
            return;

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
        if (SkeletonObjs < 20)
        {
            SkeletonObjs++;
            StartCoroutine(SkeletonSpawn());
        }
        if (TurtleSlimeObjs < 1)
        {
            TurtleSlimeObjs++;
            StartCoroutine(TurtleSlimeSpawn());
        }
        if (GolemObjs < 1)
        {
            GolemObjs++;
            StartCoroutine(GolemSpawn());
        }
       
         setMosterNumber(SlimeObjs, BlueSlimeObjs, GoblinObjs, GoblinArObjs, SkeletonObjs, TurtleSlimeObjs, GolemObjs);
        
    }

    [PunRPC]
    public void setMosterNumber(int slime, int blueslime , int gobline , int archer , int skeleton , int tutle, int golem)
    {
        if (PhotonNetwork.IsMasterClient)
        {
        
            photonView.RPC("setMosterNumber", RpcTarget.Others, slime, blueslime, gobline, archer, skeleton, tutle, golem);
            
        }
        else
        {
            SlimeObjs = slime;
            BlueSlimeObjs = blueslime;
            GoblinObjs = gobline;
            GoblinArObjs = archer;
            SkeletonObjs = skeleton;
            TurtleSlimeObjs = tutle;
            GolemObjs = golem;
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
                if (SlimePoints[i].gameObject.activeSelf)
                {
                    Slimeready = true;
                    break;
                }
            }

            if (Slimeready)
            {
                
                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[0]);
                Debug.Log(enemy);
                enemy.transform.position = SlimePoints[slimeidx].position;

                enemy.GetComponent<Monster>().MonsterPosition(SlimePoints[slimeidx].position);

                enemy.GetComponent<EnemySlime>().respawn = SlimePoints[slimeidx];
                SlimePoints[slimeidx].gameObject.SetActive(false);
                slimeidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //���ѷ����� ����Ƽ ���°� ����
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
                if (BlueSlimePoints[i].gameObject.activeSelf)
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
                BlueSlimePoints[blueslimeidx].gameObject.SetActive(false);
                blueslimeidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //���ѷ����� ����Ƽ ���°� ����
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
                if (GoblinPoints[i].gameObject.activeSelf)
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
                GoblinPoints[goblinidx].gameObject.SetActive(false);
                goblinidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //���ѷ����� ����Ƽ ���°� ����
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
                if (GoblinArcherPoints[i].gameObject.activeSelf)
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
                GoblinArcherPoints[goblinarcheridx].gameObject.SetActive(false);
                goblinarcheridx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //���ѷ����� ����Ƽ ���°� ����
        }
    }
    IEnumerator TurtleSlimeSpawn()
    {
        while (true)
        {
            turtleslimetime = 200;
            yield return new WaitForSeconds(turtleslimetime);

            for (int i = 0; i < 1; i++)
            {
                TurtleSlimeidx = i;
                if (TurtleSlimePoint.gameObject.activeSelf)
                {
                    TurtleSlimeready = true;
                    break;
                }
            }

            if (TurtleSlimeready)
            {

                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[5]);

                enemy.transform.position = TurtleSlimePoint.position;
                enemy.GetComponent<EnemyRange>().respawn = TurtleSlimePoint;
                TurtleSlimePoint.gameObject.SetActive(false);
                TurtleSlimeidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //���ѷ����� ����Ƽ ���°� ����
        }
    }
    IEnumerator SkeletonSpawn()
    {
        while (true)
        {
            skeletontime = Random.Range(5,7);
            yield return new WaitForSeconds(skeletontime);

            for (int i = 0; i < MonsterManager.monsterManager.Skeleton.Length; i++)
            {
                Skeletonidx = i;
                if (SkeletonPoints[i].gameObject.activeSelf)
                {
                    Skeletonready = true;
                    break;
                }
            }

            if (GoblinArcherready)
            {

                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[4]);

                enemy.transform.position = SkeletonPoints[goblinarcheridx].position;
                enemy.GetComponent<EnemyRange>().respawn = SkeletonPoints[goblinarcheridx];
                SkeletonPoints[goblinarcheridx].gameObject.SetActive(false);
                Skeletonidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //���ѷ����� ����Ƽ ���°� ����
        }
    }

    IEnumerator GolemSpawn()
    {
        while (true)
        {
            golemtime = 200f;
            yield return new WaitForSeconds(golemtime);

            for (int i = 0; i < 1; i++)
            {
                Golemidx = i;
                if (GolemPoint.gameObject.activeSelf)
                {
                    Golemready = true;
                    break;
                }
            }

            if (Golemready)
            {

                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[6]);

                enemy.transform.position = GolemPoint.position;
                enemy.GetComponent<EnemyRange>().respawn = GolemPoint;
                GolemPoint.gameObject.SetActive(false);
                Golemidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //���ѷ����� ����Ƽ ���°� ����
        }
    }

   
}
