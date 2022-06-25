using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private SphereCollider NPCRange;
    [SerializeField]
    public static bool isNPCRange=false;
    [SerializeField]
    private AllUI allUI;
    [SerializeField]
    private PlayerST playerST;
    [SerializeField]
    private Text NPCRangeText;

    public Camera Camera; // 카메라 지정
    public RaycastHit hit; //힛 한곳 취득해 넣어둘곳

  

    public void SetNPC()
    {
        NPCRange = GetComponent<SphereCollider>();
        allUI = FindObjectOfType<AllUI>();
        playerST = FindObjectOfType<PlayerST>();
        Camera = playerST.GetComponentInChildren<Camera>();

    }
   
    private void OnTriggerStay(Collider other)
    {
        Cursor.lockState = CursorLockMode.Confined;
        allUI.MouseCursor.transform_cursor.gameObject.SetActive(true);
        
        if (other.tag == "Player")
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                isNPCRange = true;

                NPCtext();
            }
                
        }
        if(other.tag == "Arrow")
        {
            Destroy(other.gameObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Player")
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {

                isNPCRange = false;
               NPCRangeText.enabled = false;
                Cursor.lockState = CursorLockMode.Locked;
              allUI.MouseCursor.transform_cursor.gameObject.SetActive(false);
            }
        }
    }


    void NPCtext()
    {
        NPCRangeText.enabled = true;

        if (NPCRange.gameObject.tag == "SHOPIA")
        {
            NPCRangeText.text = "소피아를 클릭하시면 아이템 상점을 들어갑니다.";
        }
        else if (NPCRange.gameObject.tag == "PETER")
        {
            NPCRangeText.text = "피터를 클릭하시면 퀘스트를 받을수 있습니다..";
        }
    }
 
    
}
