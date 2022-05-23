using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
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

        Generate();
    }

    void Generate()  //풀을 생성
    {
        GameObject SlimePools = new GameObject("Slime Pool");
        GameObject BlueSlimePools = new GameObject("BlueSlime Pool");
        GameObject GoblinPools = new GameObject("Goblin Pool");
        GameObject GoblinArcherPools = new GameObject("GoblinArcher Pool");
        GameObject SkeletonPools = new GameObject("Skeleton Pool");
        for (int i = 0; i < Slime.Length; i++)
        {
            Slime[i] = Instantiate(SlimePrefabs, SlimePools.transform);
            Slime[i].SetActive(false);
        }
        for (int i = 0; i < BlueSlime.Length; i++)
        {
            BlueSlime[i] = Instantiate(BlueSlimePrefabs, BlueSlimePools.transform);
            BlueSlime[i].SetActive(false);
        }
        for (int i = 0; i < Goblin.Length; i++)
        {
            Goblin[i] = Instantiate(GoblinPrefabs, GoblinPools.transform);
            Goblin[i].SetActive(false);
        }
        for (int i = 0; i < GoblinArcher.Length; i++)
        {
            GoblinArcher[i] = Instantiate(GoblinArcherPrefabs, GoblinArcherPools.transform);
            GoblinArcher[i].SetActive(false);
        }
        for (int i = 0; i < Skeleton.Length; i++)
        {
            Skeleton[i] = Instantiate(SkeletonPrefabs, SkeletonPools.transform);
            Skeleton[i].SetActive(false);
        }

        TurtleSlime = Instantiate(TurtleSlimePrefabs);
        TurtleSlime.SetActive(false);

        Golem = Instantiate(GolemPrefabs);
        Golem.SetActive(false);

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
