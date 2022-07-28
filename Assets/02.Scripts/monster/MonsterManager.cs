using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MonsterManager : MonoBehaviourPunCallbacks
{
    public GameObject SlimePrefabs;
    public GameObject BlueSlimePrefabs;
    public GameObject GoblinPrefabs;
    public GameObject Goblin2Prefabs;
    public GameObject GoblinArcherPrefabs;
    public GameObject SkeletonPrefabs;
    public GameObject TurtleSlimePrefabs;
    public GameObject GolemPrefabs;

    GameObject[] targetPool;

    public GameObject[] Slime;
    public GameObject[] BlueSlime;
    public GameObject[] Goblin;
    public GameObject[] Goblin2; //여자고블린 추가
    public GameObject[] GoblinArcher;
    public GameObject[] Skeleton;
    public GameObject TurtleSlime;
    public GameObject Golem;
    public GameObject ResetBox;
    public static MonsterManager monsterManager;

    private void Awake()
    {
        monsterManager = this;
        Slime = new GameObject[10];
        BlueSlime = new GameObject[10];
        Goblin = new GameObject[10];
        Goblin2 = new GameObject[10];
        GoblinArcher = new GameObject[10];
        Skeleton = new GameObject[10];
        Invoke("Generate", 0.2f); //마스터 플레이어가 나갔을때 몬스터가 생성이 안된다면 이 0.2초사이에 마스터플레이어가 나갔을 것입니다.. 수정 필요하긴합니다 이부분.
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {

        Debug.Log("마스터클라이언트 나감");
        //ResetBox.SetActive(true);
        Invoke("RemoveMonster", 0.1f);
        Invoke("Generate", 0.2f);
        //Invoke("Generate", 0.1f);

    }

    public void RemoveMonster()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < Slime.Length; i++)
            {
                //Slime[i].GetComponent<PhotonView>().RequestOwnership();
                if(Slime[i])
                Slime[i].SetActive(false);
                //PhotonNetwork.Destroy(Slime[i]);
            }
            for (int i = 0; i < BlueSlime.Length; i++)
            {
                //BlueSlime[i].GetComponent<PhotonView>().RequestOwnership();
                if (BlueSlime[i])
                    BlueSlime[i].SetActive(false);
                //PhotonNetwork.Destroy(BlueSlime[i]);
            }
            for (int i = 0; i < Goblin.Length; i++)
            {
                //Goblin[i].GetComponent<PhotonView>().RequestOwnership();
                if (Goblin[i])
                    Goblin[i].SetActive(false);
                //PhotonNetwork.Destroy(Goblin[i]);
            }
            for (int i = 0; i < Goblin2.Length; i++)
            {
                //Goblin2[i].GetComponent<PhotonView>().RequestOwnership();
                if (Goblin2[i])
                    Goblin2[i].SetActive(false);
                //PhotonNetwork.Destroy(Goblin2[i]);
            }
            for (int i = 0; i < GoblinArcher.Length; i++)
            {
                //GoblinArcher[i].GetComponent<PhotonView>().RequestOwnership();
                if (GoblinArcher[i])
                    GoblinArcher[i].SetActive(false);
                //PhotonNetwork.Destroy(GoblinArcher[i]);
            }
            for (int i = 0; i < Skeleton.Length; i++)
            {
                //Skeleton[i].GetComponent<PhotonView>().RequestOwnership();
                if (Skeleton[i])
                    Skeleton[i].SetActive(false);
                //PhotonNetwork.Destroy(Skeleton[i]);
            }
            //TurtleSlime.GetComponent<PhotonView>().RequestOwnership();
            if (TurtleSlime)
                TurtleSlime.SetActive(false);
            //PhotonNetwork.Destroy(TurtleSlime);
            //Golem.GetComponent<PhotonView>().RequestOwnership();
            if (Golem)
                Golem.SetActive(false);
            //PhotonNetwork.Destroy(Golem);
        }
    }
    public void startSetting()
    {
        Debug.Log("리모트용 리스폰세팅");
        for (int i = 0; i < Slime.Length; i++)
        {
            Slime[i].GetComponent<EnemySlime>().respawn = SpawnManager.spawnManager.SlimePoints[i];
        }
        for (int i = 0; i < BlueSlime.Length; i++)
        {
            BlueSlime[i].GetComponent<EnemyBlueSlime>().respawn = SpawnManager.spawnManager.BlueSlimePoints[i];
        }
        for (int i = 0; i < Goblin.Length; i++)
        {
            Goblin[i].GetComponent<EnemyGoblin>().respawn = SpawnManager.spawnManager.GoblinPoints[i];
        }
        for (int i = 0; i < Goblin2.Length; i++)
        {
            Goblin2[i].GetComponent<EnemyGoblin2>().respawn = SpawnManager.spawnManager.Goblin2Points[i];
        }
        for (int i = 0; i < GoblinArcher.Length; i++)
        {
            GoblinArcher[i].GetComponent<EnemyRange>().respawn = SpawnManager.spawnManager.GoblinArcherPoints[i];
        }
        for (int i = 0; i < Skeleton.Length; i++)
        {
            Skeleton[i].GetComponent<EnemySkeleton>().respawn = SpawnManager.spawnManager.SkeletonPoints[i];
        }
        TurtleSlime.GetComponent<EnemyBoss1>().respawn = SpawnManager.spawnManager.TurtleSlimePoint;
        Golem.GetComponent<EnemyBoss2>().respawn = SpawnManager.spawnManager.GolemPoint;
    }
    public void Generate()  //Ǯ       
    {
        ResetBox.SetActive(false);
        GameObject SlimePools = new GameObject("Slime Pool");
        GameObject BlueSlimePools = new GameObject("BlueSlime Pool");
        GameObject GoblinPools = new GameObject("Goblin Pool");
        GameObject Goblin2Pools = new GameObject("Goblin2 Pool");
        GameObject GoblinArcherPools = new GameObject("GoblinArcher Pool");
        GameObject SkeletonPools = new GameObject("Skeleton Pool");

        if (!PhotonNetwork.IsMasterClient)
        {
            EnemySlime[] slimes = GameObject.FindObjectsOfType<EnemySlime>();
            EnemyBlueSlime[] blueslimes = GameObject.FindObjectsOfType<EnemyBlueSlime>();
            EnemyGoblin[] goblins = GameObject.FindObjectsOfType<EnemyGoblin>();
            EnemyGoblin2[] goblins2 = GameObject.FindObjectsOfType<EnemyGoblin2>();
            EnemyRange[] Ranges = GameObject.FindObjectsOfType<EnemyRange>();
            EnemySkeleton[] skelletons = GameObject.FindObjectsOfType<EnemySkeleton>();
            EnemyBoss1 turtle = GameObject.FindObjectOfType<EnemyBoss1>();
            EnemyBoss2 golem = GameObject.FindObjectOfType<EnemyBoss2>();


            for (int i = 0; i < slimes.Length; i++)
            {
                slimes[i].transform.parent = SlimePools.transform;
                Slime[i] = slimes[i].gameObject;
            }

            for (int i = 0; i < blueslimes.Length; i++)
            {
                blueslimes[i].transform.parent = BlueSlimePools.transform;
                BlueSlime[i] = blueslimes[i].gameObject;
            }


            for (int i = 0; i < goblins.Length; i++)
            {
                goblins[i].transform.parent = GoblinPools.transform;
                Goblin[i] = goblins[i].gameObject;
            }

            for (int i = 0; i < goblins2.Length; i++)
            {
                goblins2[i].transform.parent = Goblin2Pools.transform;
                Goblin2[i] = goblins2[i].gameObject;
            }


            for (int i = 0; i < Ranges.Length; i++)
            {
                Ranges[i].transform.parent = GoblinArcherPools.transform;
                GoblinArcher[i] = Ranges[i].gameObject;
            }


            for (int i = 0; i < skelletons.Length; i++)
            {
                skelletons[i].transform.parent = SkeletonPools.transform;
                Skeleton[i] = skelletons[i].gameObject;
            }
            TurtleSlime = turtle.gameObject;
            Golem = golem.gameObject;

            //startSetting();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < Slime.Length; i++)
            {
                Slime[i] = PhotonNetwork.Instantiate("Monster/LV1.Slime", transform.position, Quaternion.identity);
                Slime[i].transform.parent = SlimePools.transform;
                Slime[i].SetActive(false);
                Slime[i].transform.position = SpawnManager.spawnManager.SlimePoints[i].transform.position;
            }
            for (int i = 0; i < BlueSlime.Length; i++)
            {
                BlueSlime[i] = PhotonNetwork.Instantiate("Monster/LV3.BlueSlime", transform.position, Quaternion.identity);
                BlueSlime[i].transform.parent = BlueSlimePools.transform;
                BlueSlime[i].SetActive(false);
                BlueSlime[i].transform.position = SpawnManager.spawnManager.BlueSlimePoints[i].transform.position;
            }
            for (int i = 0; i < Goblin.Length; i++)
            {
                Goblin[i] = PhotonNetwork.Instantiate("Monster/LV5.Goblin(M)", transform.position, Quaternion.identity);
                Goblin[i].transform.parent = GoblinPools.transform;
                Goblin[i].SetActive(false);
                Goblin[i].transform.position = SpawnManager.spawnManager.GoblinPoints[i].transform.position;
            }
            for (int i = 0; i < Goblin2.Length; i++)
            {
                Goblin2[i] = PhotonNetwork.Instantiate("Monster/LV4.Goblin(W)", transform.position, Quaternion.identity);
                Goblin2[i].transform.parent = Goblin2Pools.transform;
                Goblin2[i].SetActive(false);
                Goblin2[i].transform.position = SpawnManager.spawnManager.Goblin2Points[i].transform.position;
            }
            for (int i = 0; i < GoblinArcher.Length; i++)
            {
                GoblinArcher[i] = PhotonNetwork.Instantiate("Monster/LV7.Rich", transform.position, Quaternion.identity);
                GoblinArcher[i].transform.parent = GoblinArcherPools.transform;
                GoblinArcher[i].SetActive(false);
                GoblinArcher[i].transform.position = SpawnManager.spawnManager.GoblinArcherPoints[i].transform.position;
            }
            for (int i = 0; i < Skeleton.Length; i++)
            {
                Skeleton[i] = PhotonNetwork.Instantiate("Monster/LV8.Skeleton", transform.position, Quaternion.identity);
                Skeleton[i].transform.parent = SkeletonPools.transform;
                Skeleton[i].SetActive(false);
                Skeleton[i].transform.position = SpawnManager.spawnManager.SkeletonPoints[i].transform.position;
            }

            TurtleSlime = PhotonNetwork.Instantiate("Monster/LV5.Boss.Golem", transform.position, Quaternion.identity);
            TurtleSlime.SetActive(false);
            TurtleSlime.transform.position = SpawnManager.spawnManager.TurtleSlimePoint.transform.position;

            Golem = PhotonNetwork.Instantiate("Monster/LV10.Boss.EvilGod", transform.position, Quaternion.identity);
            Golem.SetActive(false);
            Golem.transform.position = SpawnManager.spawnManager.GolemPoint.transform.position;

            SpawnManager.spawnManager.start();
        }


    }


    public GameObject MakeObj(string type) //Ǯ     ȯ
    {

        Debug.Log("makeobj함수 들어오나?");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log(PhotonNetwork.IsMasterClient); //마스터 클라이언트에서만 일어나나?
            switch (type)
            {
                case "Slime":
                    targetPool = Slime;
                    break;
                case "BlueSlime":
                    targetPool = BlueSlime;
                    break;
                case "Goblin":
                    targetPool = Goblin;
                    break;
                case "Goblin2":
                    targetPool = Goblin2;
                    break;
                case "GoblinArcher":
                    targetPool = GoblinArcher;
                    break;
                case "Skeleton":
                    targetPool = Skeleton;
                    break;
                case "TurtleSlime":
                    if (!TurtleSlime.activeSelf)
                    {
                        TurtleSlime.SetActive(true);
                        return TurtleSlime;
                    }
                    break;
                case "Golem":
                    if (!Golem.activeSelf)
                    {
                        Golem.SetActive(true);
                        return Golem;
                    }
                    break;
            }

            for (int i = 0; i < targetPool.Length; i++)
            {
                Debug.Log(targetPool); //타겟풀 누구?
                if (targetPool[i] && !targetPool[i].activeSelf)
                {
                    //targetPool[i].SetActive(true);
                    targetPool[i].GetComponent<Monster>().MonsterRespawn();
                    return targetPool[i];
                }
            }
            // return null;
            Debug.Log("여기 오나?");
        }
        return null;
    }

}