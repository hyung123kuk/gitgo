using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    [SerializeField]
    public Bar Hp_bar;
    [SerializeField]
    public Bar Mp_bar;
    [SerializeField]
    public Text level_Text;
    [SerializeField]
    public Text Exp_Text;
    [SerializeField]
    public Image Exp_bar;
    [SerializeField]
    private PlayerStat playerStat;




    private void Start()
    {
        PlayerStat[] playerStats = FindObjectsOfType<PlayerStat>();
        foreach (PlayerStat myPlayerstat in playerStats)
        {
            if (myPlayerstat.photonView.IsMine)
            {
                playerStat = myPlayerstat;
                break;
            }
        }

    }

    public void LevelSet()
    {
        
        level_Text.text = "LV." + playerStat.Level;

    }

    public void ExpSet()
    {    


        Exp_bar_Set();
        Exp_Text.text = playerStat.NowExp + "/" + playerStat.TotalExp + "(" + (int)(playerStat.NowExp/playerStat.TotalExp*100) + "%)";

    }

    public void Exp_bar_Set()
    {
        StopCoroutine("ExpUp");
        StartCoroutine("ExpUp");

    }
    public float smoothness = 0.02f;
    public float durationBarUp = 0.1f;
    IEnumerator ExpUp()
    {
        
        float progress = 0;
        float increment = smoothness / durationBarUp;
        while (progress <= 1)
        {
            Exp_bar.fillAmount = Mathf.Lerp(Exp_bar.fillAmount, playerStat.NowExp / playerStat.TotalExp, progress);
          
            progress += increment;
            yield return new WaitForSeconds(smoothness);
           
        }


    }




    public void bar_set()
    {
        Hp_set();
        Mp_set();
        ExpSet();
    }

    public void Hp_set()
    {
        Hp_bar.BarSet();

    }

    public void Mp_set()
    {
        Mp_bar.BarSet();

    }

    public void expUp()
    {
        playerStat.AddExp(100f);
    }
}
