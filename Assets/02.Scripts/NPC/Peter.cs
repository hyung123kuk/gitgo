using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peter : NPC
{
    [SerializeField]
    private QuestStore questStore;




    void Start()
    {
        questStore = FindObjectOfType<QuestStore>();


        SetNPC();
    }

    // Update is called once per frame
    public void Update()
    {

        if (isNPCRange && !inventory.iDown)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, maxDistance: 5f))
                {
                    if (hit.transform.gameObject.tag == "PETER")
                    {
                        questStore.storeOn();



                    }
                }
            }
        }
    }
}
