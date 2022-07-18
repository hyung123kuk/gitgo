using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPun
{
    public Transform[] SlimePoints = new Transform[10];   //스폰포인트 
    public int SlimeObjs;             //현재 살아있는 몬스터수
    bool Slimeready;                  //스폰준비완료?
    public int slimeidx = 0;          //동시 소환 제어

    public Transform[] BlueSlimePoints = new Transform[10];
    public int BlueSlimeObjs;
    bool BlueSlimeready;                  
    public int blueslimeidx = 0;
    
    public Transform[] GoblinPoints = new Transform[10];
    public int GoblinObjs;
    bool Goblinready;
    public int goblinidx = 0;

    public Transform[] Goblin2Points = new Transform[10];
    public int Goblin2Objs;
    bool Goblin2ready;
    public int goblin2idx = 0;

    public Transform[] GoblinArcherPoints = new Transform[10];
    public int GoblinArObjs;
    bool GoblinArcherready;
    public int goblinarcheridx = 0;

    public Transform[] SkeletonPoints = new Transform[10];
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
    private float goblin2time;
    private float goblinarchertime;
    private float turtleslimetime;
    private float skeletontime;
    private float golemtime;


    private void Awake()
    {
        SlimeObjs = 10;
        BlueSlimeObjs = 10;
        GoblinObjs = 10;
        Goblin2Objs = 10;
        GoblinArObjs = 10;
        TurtleSlimeObjs = 1;
        SkeletonObjs = 10;
        GolemObjs = 1;
        spawnManager = this;
        monsters = new string[] { "Slime", "BlueSlime", "Goblin", "Goblin2", "GoblinArcher", "Skeleton", "TurtleSlime", "Golem" };
    }
    public void start() //소환해놓기
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
        for (int i = 0; i < MonsterManager.monsterManager.Goblin2.Length; i++)
        {
            GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[3]);

            enemy.transform.position = Goblin2Points[i].position;
            enemy.GetComponent<EnemyGoblin2>().respawn = Goblin2Points[i];
            Goblin2Points[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < MonsterManager.monsterManager.GoblinArcher.Length; i++)
        {
            GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[4]);

            enemy.transform.position = GoblinArcherPoints[i].position;
            enemy.GetComponent<EnemyRange>().respawn = GoblinArcherPoints[i];
            GoblinArcherPoints[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < MonsterManager.monsterManager.Skeleton.Length; i++)
        {
            GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[5]);

            enemy.transform.position = SkeletonPoints[i].position;
            enemy.GetComponent<EnemySkeleton>().respawn = SkeletonPoints[i];
            SkeletonPoints[i].gameObject.SetActive(false);
        }
        GameObject boss1 = MonsterManager.monsterManager.MakeObj(monsters[6]); // 터틀슬라임

        boss1.transform.position = TurtleSlimePoint.position; //위치지정
        boss1.GetComponent<EnemyBoss1>().respawn = TurtleSlimePoint; //리스폰위치지정
        TurtleSlimePoint.gameObject.SetActive(false);  //몬스터가 살아있는지 스위치용도


        GameObject boss2 = MonsterManager.monsterManager.MakeObj(monsters[7]); // 골렘

        boss2.transform.position = GolemPoint.position; //위치지정
        boss2.GetComponent<EnemyBoss2>().respawn = GolemPoint; //리스폰위치지정
        GolemPoint.gameObject.SetActive(false);  //몬스터가 살아있는지 스위치용도

    }
    
    private void Update()
    {

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (SlimeObjs < 10)
        {
            SlimeObjs++;
            StartCoroutine(SlimeSpawn());
        }
        if (BlueSlimeObjs < 10)
        {
            BlueSlimeObjs++;
            StartCoroutine(BlueSlimeSpawn());
        }
        if (GoblinObjs < 10)
        {
            GoblinObjs++;
            StartCoroutine(GoblinSpawn());
        }
        if (Goblin2Objs < 10)
        {
            Goblin2Objs++;
            StartCoroutine(Goblin2Spawn());
        }
        if (GoblinArObjs < 10)
        {
            GoblinArObjs++;
            StartCoroutine(GoblinArSpawn());
        }
        if (SkeletonObjs < 10)
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
       
         setMosterNumber(SlimeObjs, BlueSlimeObjs, GoblinObjs, Goblin2Objs, GoblinArObjs, SkeletonObjs, TurtleSlimeObjs, GolemObjs);
        
    }

    [PunRPC]
    public void setMosterNumber(int slime, int blueslime , int gobline , int gobline2, int archer , int skeleton , int tutle, int golem)
    {
        if (PhotonNetwork.IsMasterClient)
        {
        
            photonView.RPC("setMosterNumber", RpcTarget.Others, slime, blueslime, gobline, gobline2 , archer, skeleton, tutle, golem);
            
        }
        else
        {
            SlimeObjs = slime;
            BlueSlimeObjs = blueslime;
            GoblinObjs = gobline;
            Goblin2Objs = gobline2;
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
            int i;
            slimetime = Random.Range(5, 9);
            yield return new WaitForSeconds(slimetime);

            for (i = 0; i < MonsterManager.monsterManager.Slime.Length; i++)
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
                Debug.Log(enemy); //enemy가 널리퍼
                enemy.transform.position = SlimePoints[i].position;

                enemy.GetComponent<Monster>().MonsterPosition(SlimePoints[i].position);

                enemy.GetComponent<EnemySlime>().respawn = SlimePoints[i];
                SlimePoints[i].gameObject.SetActive(false);
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
            int i;
            blueslimetime = Random.Range(5, 9);
            yield return new WaitForSeconds(blueslimetime);

            for (i = 0; i < MonsterManager.monsterManager.BlueSlime.Length; i++)
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
                Debug.Log(enemy); //enemy가 널리퍼
                Debug.Log(BlueSlimePoints[i]);
                enemy.transform.position = BlueSlimePoints[i].position;

                enemy.GetComponent<Monster>().MonsterPosition(BlueSlimePoints[i].position);
                
                enemy.GetComponent<EnemyBlueSlime>().respawn = BlueSlimePoints[i];
                BlueSlimePoints[i].gameObject.SetActive(false);
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
            int i;
            goblintime = Random.Range(5, 9);
            yield return new WaitForSeconds(goblintime);

            for (i = 0; i < MonsterManager.monsterManager.Goblin.Length; i++)
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

                enemy.transform.position = GoblinPoints[i].position;    
                enemy.GetComponent<EnemyGoblin>().respawn = GoblinPoints[i];
                GoblinPoints[i].gameObject.SetActive(false);
                goblinidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //무한루프로 유니티 뻗는거 방지
        }
    }
    IEnumerator Goblin2Spawn()
    {
        while (true)
        {
            int i;
            goblin2time = Random.Range(5, 9);
            yield return new WaitForSeconds(goblin2time);

            for (i = 0; i < MonsterManager.monsterManager.Goblin2.Length; i++)
            {
                goblin2idx = i;
                if (Goblin2Points[i].gameObject.activeSelf)
                {
                    Goblin2ready = true;
                    break;
                }
            }

            if (Goblin2ready)
            {

                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[3]);

                enemy.transform.position = Goblin2Points[i].position;
                enemy.GetComponent<EnemyGoblin2>().respawn = Goblin2Points[i];
                Goblin2Points[i].gameObject.SetActive(false);
                goblin2idx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //무한루프로 유니티 뻗는거 방지
        }
    }
    IEnumerator GoblinArSpawn()
    {
        while (true)
        {
            int i;
            goblinarchertime = Random.Range(5, 9);
            yield return new WaitForSeconds(goblinarchertime);

            for (i = 0; i < MonsterManager.monsterManager.GoblinArcher.Length; i++)
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

                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[4]);

                enemy.transform.position = GoblinArcherPoints[i].position;
                enemy.GetComponent<EnemyRange>().respawn = GoblinArcherPoints[i];
                GoblinArcherPoints[i].gameObject.SetActive(false);
                goblinarcheridx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //무한루프로 유니티 뻗는거 방지
        }
    }
    IEnumerator TurtleSlimeSpawn()
    {
        while (true)
        {
            int i;
            turtleslimetime = 200;
            yield return new WaitForSeconds(turtleslimetime);

            for (i = 0; i < 1; i++)
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

                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[6]);

                enemy.transform.position = TurtleSlimePoint.position;
                enemy.GetComponent<EnemyRange>().respawn = TurtleSlimePoint;
                TurtleSlimePoint.gameObject.SetActive(false);
                TurtleSlimeidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //무한루프로 유니티 뻗는거 방지
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

            if (Skeletonready)
            {

                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[5]);

                enemy.transform.position = SkeletonPoints[goblinarcheridx].position;
                enemy.GetComponent<EnemyRange>().respawn = SkeletonPoints[goblinarcheridx];
                SkeletonPoints[goblinarcheridx].gameObject.SetActive(false);
                Skeletonidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //무한루프로 유니티 뻗는거 방지
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

                GameObject enemy = MonsterManager.monsterManager.MakeObj(monsters[7]);

                enemy.transform.position = GolemPoint.position;
                enemy.GetComponent<EnemyRange>().respawn = GolemPoint;
                GolemPoint.gameObject.SetActive(false);
                Golemidx = 0;
                break;
            }
            InfiniteLoopDetector.Run(); //무한루프로 유니티 뻗는거 방지
        }
    }

   
}
