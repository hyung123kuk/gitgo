using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonsterManager : MonoBehaviourPun
{
    public GameObject SlimePrefabs;
    public GameObject BlueSlimePrefabs;
    public GameObject GoblinPrefabs;
    public GameObject GoblinArcherPrefabs;
    public GameObject SkeletonPrefabs;
    public GameObject TurtleSlimePrefabs;
    public GameObject GolemPrefabs;

    GameObject[] targetPool; 

    public GameObject[] Slime;
    public GameObject[] BlueSlime;
    public GameObject[] Goblin;
    public GameObject[] GoblinArcher;
    public GameObject[] Skeleton;
    public GameObject TurtleSlime;
    public GameObject Golem;

    public static MonsterManager monsterManager;

    private void Awake()
    {
        
       

        monsterManager = this;
        Slime = new GameObject[20];
        BlueSlime = new GameObject[20];
        Goblin = new GameObject[20];
        GoblinArcher = new GameObject[10];
        Skeleton = new GameObject[20];

        Invoke("Generate",0.2f);
    }

    void Generate()  //풀을 생성
    {
       if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        GameObject SlimePools = new GameObject("Slime Pool");
        GameObject BlueSlimePools = new GameObject("BlueSlime Pool");
        GameObject GoblinPools = new GameObject("Goblin Pool");
        GameObject GoblinArcherPools = new GameObject("GoblinArcher Pool");
        GameObject SkeletonPools = new GameObject("Skeleton Pool");


           
        for (int i = 0; i < Slime.Length; i++)
        {
           Slime[i] = PhotonNetwork.Instantiate("Monster/LV1.Slime",transform.position,Quaternion.identity);
           Slime[i].SetActive(false);
           Slime[i].transform.position = SpawnManager.spawnManager.SlimePoints[i].transform.position;
        }
        for (int i = 0; i < BlueSlime.Length; i++)
        {
            BlueSlime[i] = PhotonNetwork.Instantiate("Monster/LV3.BlueSlime", transform.position, Quaternion.identity);
            BlueSlime[i].SetActive(false);
            BlueSlime[i].transform.position = SpawnManager.spawnManager.BlueSlimePoints[i].transform.position;
        }
        for (int i = 0; i < Goblin.Length; i++)
        {
            Goblin[i] = PhotonNetwork.Instantiate("Monster/LV4.Goblin", transform.position, Quaternion.identity);
            Goblin[i].SetActive(false);
            Goblin[i].transform.position = SpawnManager.spawnManager.GoblinPoints[i].transform.position;
        }
        for (int i = 0; i < GoblinArcher.Length; i++)
        {
            GoblinArcher[i] = PhotonNetwork.Instantiate("Monster/LV6.GoblinArcher", transform.position, Quaternion.identity);
            GoblinArcher[i].SetActive(false);
            GoblinArcher[i].transform.position = SpawnManager.spawnManager.GoblinArcherPoints[i].transform.position;
        }
        for (int i = 0; i < Skeleton.Length; i++)
        {
            Skeleton[i] = PhotonNetwork.Instantiate("Monster/LV8.Skeleton", transform.position, Quaternion.identity);
            Skeleton[i].SetActive(false);
            Skeleton[i].transform.position = SpawnManager.spawnManager.SkeletonPoints[i].transform.position;
        }

        TurtleSlime = PhotonNetwork.Instantiate("Monster/LV5.Boss.TurtleSlime", transform.position, Quaternion.identity);
        TurtleSlime.SetActive(false);
        TurtleSlime.transform.position = SpawnManager.spawnManager.TurtleSlimePoint.transform.position;

        Golem = PhotonNetwork.Instantiate("Monster/LV10.Boss.Golem", transform.position, Quaternion.identity);
        Golem.SetActive(false);
        Golem.transform.position = SpawnManager.spawnManager.GolemPoint.transform.position;

        SpawnManager.spawnManager.start();
    }

    public GameObject MakeObj(string type) //풀을 반환
    {
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
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        return null;
    }

    
}
