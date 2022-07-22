using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LogManager : MonoBehaviourPun
{
    [SerializeField]
    private Text[] logText;
    [SerializeField]
    public static LogManager logManager;


    //½¦ÀÌÅ· °ü·Ã
    float allX = 0;
    float allY = 0;
    float allZ = 0;
    public float shakeTime = 0f;
    Transform tr;
    void Start()
    {
        logManager = this;
        logText = GetComponentsInChildren<Text>();

        PlayerST[] playerSts = FindObjectsOfType<PlayerST>();
        foreach (PlayerST myPlayerst in playerSts)
        {
            if (myPlayerst.photonView.IsMine)
            {
                tr = myPlayerst.gameObject.transform;
                break;
            }
        }
        
    }

    private void LogUp()
    {
        int index = 0;
        for(int i=0; i<logText.Length; i++)
        {
           
            if (logText[i+1].text == "")
                break;
            index++;
        }
        for(int i = index+1; i ==0 ; i--)
        {

            logText[i + 1].text = logText[i].text;
            logText[i + 1].color = logText[i].color;
            
        }
        

    }
    IEnumerator LogDisappear()
    {
        while(logText[0].text!="")
        {
            yield return new WaitForSeconds(0.01f);

            for(int i =0;i<logText.Length; i++)
            {
                SetColor(0.01f, logText[i]);
            }
                
        }

    }

    public void SetColor(float _alpha,Text logText)
    {
        Color color = logText.color;
        color.a -= _alpha;
        if(color.a <= 0)
        {
            logText.text = "";
        }
        logText.color = color;
    }

    public void Log(string TextLog,bool Error =false )
    {
        LogUp();
        logText[0].text = TextLog;
        if (!Error) { logText[0].color = Color.white; }
       
        else if (Error) { logText[0].color = Color.red;
            ErrorShake();
        }
        StopAllCoroutines();
        StartCoroutine(LogDisappear());
    }


    private void ErrorShake()
    {
        shakeTime = Time.time;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        yield return new WaitForSeconds(0.05f);
        float progress = 0;
        float increment = 0.03f;
        while (progress <= 0.15f)
        {

            float x = Random.Range(-0.3f, 0.3f);

            float z = Random.Range(-0.3f, 0.3f);
            allX += x;
            allZ += z;
            tr.position += new Vector3(x, 0, z);
            progress += increment;
            yield return new WaitForSeconds(increment);


        }
        transform.position -= new Vector3(allX, allY, allZ);
        allX = 0; allY = 0; allZ = 0;
    }


}
