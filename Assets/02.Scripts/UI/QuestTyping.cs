using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTyping : MonoBehaviour
{
    static public QuestTyping questTyping;
    [SerializeField]
     public GameObject Quest;
    
    public Text tx;
    public bool Texting=false;
    public bool istyping = false;
    public bool endExplain = false;
    
    public Text[] itemNum;
    


    private string ing_text;
    private string main1_text1 = "오~ 처음 보는 친구구만, 어디서 온 친구 인지 모르겠지만 ,\n" +
        "이 마을에 온 목적은 이마에 써져 있구만 그래 ~ 하하하 ";
    private string main1_text2 = "번지수를 잘 찾아 왔구만. 뭐 패기도 좋고 열정도 좋지만 무엇보다 실력이 있어야지 안그래?\n" +
        "아직 애송이 냄새가 많이 나서 말이지 우선 퀵슬롯 사용법부터 알려줘야 겠구만 그래...";
    private string main1_text3 = "우선은 K키를 눌러 스킬창을 열고, 구르기 스킬을 퀵슬롯에 집어넣어,\n" +
        "구르기 스킬을 한번 사용해 보게나!!";
    public Item[] main1_item;



    private string main2_text1 = "이번에는 조금 더 어려운 미션을 줘볼까?\n" +
       "아무리 신체가 강해도, 장비를 낀 상대를 이기는 힘든 것이야..!";
    private string main2_text2 = "그 구닥다리 무기는 어디서 주워 온 건지 모르지만 별로 도움이 안되어 보이는군.. \n";
    private string main2_text3 = "소피아 상점에가 3레벨 무기를 구매해서 장착해 보게나 ! 훨씬 강해 질 것일세..\n"+
        "모.. 어떻게 레벨업 하는지도 말해줘야 하는건 아니지..?? 하하하";
    public Item[] main2_item;


    private string main3_text1 = "허허.. 금방 포기할줄 알았는데 조금은 신뢰가 생기는 구만...\n";
    private string main3_text2 = "조금 더 힘든 미션을 줘야 겠어 이 미션을 성공하면 자네를 애송이 취급하지 않겠네 \n" +
       "이번 미션을 클리어 한다면 내가 아끼는 말을 하나 주지!";
    private string main3_text3 = "전장에 가서 거북슬라임을 잡아보게나.. 이번에는 정말 만만하지 않을 걸세... 허허 \n" +
        "건투를 빈다네!!";
    public Item[] main3_item;

    private string main4_text1 = "자네를 더 진지하게 대할 필요가 있겠구만.. 우리 나힐 초원은 골렘의 공격으로\n" +
        "많은 것을 잃었고, 언제 습격당할지 몰라 모두 불안에 떨고 있지...";
    private string main4_text2 = "그렇게 현상금을 걸게 되었고 많은 사람들이 왔지만 모두 실패 하고 말았지 \n" +
       "자네가 왔을 때 항상 그랬던 것처럼 중간에 포기할 것이라 생각했는데 이제는 믿음이 가는구만..";
    private string main4_text3 = "하지만 아직 그 골렘을 죽이기에 자네는 부족하네,\n" +
        "우선 8레벨을 찍어 최종스킬을 배우게나 !!";
    public Item[] main4_item;

    private string main5_text1 = "어느덧 이 모험의 끝이 보이는 구만.. 지금까지 따라와줘서 감사하구만..\n" +
        "이제 마지막 한 단계 남았네...";
    private string main5_text2 = "부디 살아 돌아오길 바라네, 나는 당신을 믿는다네! \n";
    private string main5_text3 = "골렘을 죽이고 , 이 마을의 평화를 되찾아 주게나!! \n";
    public Item[] main5_item;


    public int mainnum = 1;
    public int Textnum = 1;
    void Start()
    {
        questTyping = this;
        Quest = transform.GetChild(0).gameObject;
        tx = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        main1_item = Resources.LoadAll<Item>("Quest/Main1");
        main2_item = Resources.LoadAll<Item>("Quest/Main2");
        main3_item = Resources.LoadAll<Item>("Quest/Main3");
        main4_item = Resources.LoadAll<Item>("Quest/Main4");
        main5_item = Resources.LoadAll<Item>("Quest/Main5");

    }

    public void QuestStartKey()
    {
        tx.text = "";
        Texting = true;
        Quest.SetActive(true);
        
        
    }


    void Update()
    {
        if (Quest.activeSelf && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            
            if (Texting)
            {

                StopAllCoroutines();
                tx.text = ing_text;
                Texting = false;
                istyping = false;

            }
            else if (!Texting)
            {
                Texting = true;
                Textnum++;
            }
        }
        if (Texting&&!istyping)
        {
            if (mainnum == 1)
            {
                Main1();
            }
            else if (mainnum == 2)
            {
                Main2();
            }
            else if (mainnum == 3)
            {
                Main3();
            }
            else if (mainnum == 4)
            {
                Main4();
            }
            else if (mainnum == 5)
            {
                Main5();
            }
        }
    }

    private void EndExplain()
    {
        if (endExplain)
        {
            endExplain = false;
            Textnum = 1;

            Quest.SetActive(false);
        }
    }

    public void Main1()
    {
        if (Textnum == 1) {
          StartCoroutine(typing(main1_text1));
         }
        else if (Textnum == 2) {
            StartCoroutine(typing(main1_text2));
        }
        else if(Textnum == 3)
        {
            StartCoroutine(typing(main1_text3));
        }
        else if(Textnum==4)
        {
            
            QuestExplain.questExplain.Main1(main1_item,false);
            endExplain = true;
            EndExplain();
        }

    }

    public void Main2()
    {
        if (Textnum == 1)
        {
            StartCoroutine(typing(main2_text1));
        }
        else if (Textnum == 2)
        {
            StartCoroutine(typing(main2_text2));
        }
        else if (Textnum == 3)
        {
            StartCoroutine(typing(main2_text3));
        }
        else if (Textnum == 4)
        {

            QuestExplain.questExplain.Main2(main2_item, false);
            endExplain = true;
            EndExplain();
        }

    }

    public void Main3()
    {
        if (Textnum == 1)
        {
            StartCoroutine(typing(main3_text1));
        }
        else if (Textnum == 2)
        {
            StartCoroutine(typing(main3_text2));
        }
        else if (Textnum == 3)
        {
            StartCoroutine(typing(main3_text3));
        }
        else if (Textnum == 4)
        {
           
            QuestExplain.questExplain.Main3(main3_item, false);
            endExplain = true;
            EndExplain();
        }

    }

    public void Main4()
    {
        if (Textnum == 1)
        {
            StartCoroutine(typing(main4_text1));
        }
        else if (Textnum == 2)
        {
            StartCoroutine(typing(main4_text2));
        }
        else if (Textnum == 3)
        {
            StartCoroutine(typing(main4_text3));
        }
        else if (Textnum == 4)
        {
           
            QuestExplain.questExplain.Main4(main4_item, false);
            endExplain = true;
            EndExplain();
        }

    }

    public void Main5()
    {
        if (Textnum == 1)
        {
            StartCoroutine(typing(main5_text1));
        }
        else if (Textnum == 2)
        {
            StartCoroutine(typing(main5_text2));
        }
        else if (Textnum == 3)
        {
            StartCoroutine(typing(main5_text3));
        }
        else if (Textnum == 4)
        {
        
            QuestExplain.questExplain.Main5(main5_item, false);
            endExplain = true;
            EndExplain();
        }

    }




    IEnumerator typing(string text)
    {
        Texting = true;
        istyping = true;
        ing_text = text;
        yield return new WaitForSeconds(0.1f);
        for(int i =0;i<= ing_text.Length; i++)
        {
            tx.text = ing_text.Substring(0, i);
            yield return new WaitForSeconds(0.02f);
        }
        Texting = false;
        istyping = false;

    }
}
