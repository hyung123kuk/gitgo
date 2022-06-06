using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieUI : MonoBehaviour
{
    public static DieUI dieUI;


    [SerializeField]
    private AllUI allUI;
    [SerializeField]
    private GameObject DieUi;

    void Awake()
    {
        dieUI = this;
        allUI = FindObjectOfType<AllUI>();
        DieUi = transform.GetChild(0).gameObject;
    }


    public void ResurrectionButton()
    {
        DieOff();
        if(Resurrection.resurrection.WherePos == 1) //마을앞부활
        {
            Resurrection.resurrection.StartCoroutine("TownResurrection");
        }
        else if(Resurrection.resurrection.WherePos == 0) //던전앞부활
        {
            Resurrection.resurrection.StartCoroutine("DunjeonResurrection");
        }
        
    }

    public void DieOn()
    {
        Cursor.lockState = CursorLockMode.Confined;
        allUI.MouseCursor.transform_cursor.gameObject.SetActive(true);
        DieUi.SetActive(true);
    }

    public void DieOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        allUI.MouseCursor.transform_cursor.gameObject.SetActive(false);
        allUI.MouseCursor.Init_Cursor();
        //AllUI.allUI.CheckCursorLock();
        DieUi.SetActive(false);
    }
}
