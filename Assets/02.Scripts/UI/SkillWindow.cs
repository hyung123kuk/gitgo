using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillWindow : MonoBehaviour , IPointerClickHandler
{
    [SerializeField]
    private GameObject skillWindow;
    [SerializeField]
    private AllUI allUI;
    [SerializeField]
    

    public static bool kDown =false;

    private void Start()
    {
        allUI = FindObjectOfType<AllUI>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) //��ųâ �ѱ�/ ����
        {
            kDown = !kDown;
            if (!kDown) //��
            {
                SkillWindowOff();

            }
            else //Ŵ
            {
                SkillWindowOn();
                allUI.SkillWindowTop();
            }
        }
    }

    public void SkillWindowOn()
    {
        skillWindow.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        allUI.MouseCursor.transform_cursor.gameObject.SetActive(true);
        kDown = true;
        
    }

    public void SkillWindowOff()
    {

        skillWindow.SetActive(false);
        allUI.MouseCursor.transform_cursor.gameObject.SetActive(false);
        allUI.MouseCursor.Init_Cursor();
        kDown = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        allUI.SkillWindowTop();
    }


}
