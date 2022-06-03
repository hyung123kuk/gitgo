using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dungeonOpen : MonoBehaviour
{
    public static dungeonOpen d_Open;

    [SerializeField]
    GameObject door_left;
    [SerializeField]
    GameObject door_right;
    [SerializeField]
    GameObject Cube12;

    public void DunGeonOpen()
    {
        door_left.SetActive(false);
        door_right.SetActive(false);
        Cube12.SetActive(false);
    }

    

}
