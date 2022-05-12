using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shopia : NPC
{
    [SerializeField]
    private itemStore itemStore;
    



    void Start()
    {
        itemStore = FindObjectOfType<itemStore>();
        

        SetNPC();
    }

    // Update is called once per frame
    public void Update()
    {

        if (isNPCRange&& !inventory.iDown)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, maxDistance: 5f))
                {
                    if (hit.transform.gameObject.tag == "SHOPIA")
                    {
                        itemStore.storeOn();



                    }
                }
            }
        }
    }
}
  
        



