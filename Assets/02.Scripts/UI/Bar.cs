using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private Image bar;
    [SerializeField]
    private Image back;
    [SerializeField]
    private Color backColor;
    [SerializeField]
   public Text text;
    public enum bars { hpbar, mpbar }
    public bars bartype;
    [SerializeField]
    private PlayerStat playerStat;
    public float duration = 0.1f; //  ¹ÝÂ¦ÀÌ´Â ¼Óµµ
    public float smoothness = 0.02f;
    public float duration2 = 0.2f; // ÀÌÀü²¨·Î ¹Ù²î´Â ¼Óµµ
    public float durationBar;
    public float durationBarUp = 0.1f;


    private void OnEnable()
    {
      
    }

    void Start()
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
        backColor = back.color;
        durationBar = duration + duration2;
    }




    float prevBar;
    public void BarSet()
    {
        prevBar = bar.fillAmount;
        if (bartype == bars.hpbar)
        {
            if (bar.fillAmount > playerStat._Hp / playerStat._MAXHP)
            {

                StopCoroutine("LerpColor");
                StopCoroutine("LerpBarDown");
                StartCoroutine("LerpColor");
                StartCoroutine("LerpBarDown");
                bar.fillAmount = playerStat._Hp / playerStat._MAXHP;

            }
            else
            {
                StopCoroutine("LerpBarUp");
                StartCoroutine("LerpBarUp");
            }

            text.text = (int)playerStat._Hp + "/" + (int)playerStat._MAXHP;
        }

        else if(bartype == bars.mpbar)
        {
            if (bar.fillAmount > playerStat._Mp / playerStat._MAXMP)
            {

                StopCoroutine("LerpColor");
                StopCoroutine("LerpBarDown");
                StartCoroutine("LerpColor");
                StartCoroutine("LerpBarDown");
                bar.fillAmount =playerStat._Mp / playerStat._MAXMP;

            }
            else
            {
                StopCoroutine("LerpBarUp");
                StartCoroutine("LerpBarUp");
            }
            text.text = (int)playerStat._Mp + "/" + (int)playerStat._MAXMP;
        }
     

    }

    IEnumerator LerpColor()
    {

        float progress = 0;
        float increment = smoothness / duration;
        while (progress <= 1)
        {
            back.color = Color.Lerp(backColor, Color.white, progress);

            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }

        progress = 0;
        increment = smoothness / duration2;
        while (progress <= 1)
        {
            back.color = Color.Lerp(Color.white, backColor, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }



    }


    IEnumerator LerpBarDown()
    {
        float progress = 0;
        float increment = smoothness / durationBar;
        while (progress <= 1)
        {
            if (bartype == bars.hpbar)
                back.fillAmount = Mathf.Lerp(prevBar, playerStat._Hp / playerStat._MAXHP, progress);
            else if (bartype == bars.mpbar)
                back.fillAmount = Mathf.Lerp(prevBar, playerStat._Mp / playerStat._MAXMP, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
        if (bar.fillAmount > 0.98f)
        {
            bar.fillAmount = 0.98f;
            
        }

    }

    IEnumerator LerpBarUp()
    {
        float progress = 0;
        float increment = smoothness / durationBarUp;
        while (progress <= 1)
        {
            if (bartype == bars.hpbar)
                bar.fillAmount = Mathf.Lerp(prevBar, playerStat._Hp / playerStat._MAXHP, progress);
            else if (bartype == bars.mpbar)
                bar.fillAmount = Mathf.Lerp(prevBar, playerStat._Mp / playerStat._MAXMP, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
            if (bar.fillAmount > 0.98f)
            {
                bar.fillAmount = 0.98f;
                StopCoroutine("LerpBarUp");
            }
        }
        

    }


}
