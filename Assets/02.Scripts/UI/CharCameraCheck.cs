using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCameraCheck : MonoBehaviour
{
    private void OnEnable()
    {
        if (!transform.GetChild(0).gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(true);
    }
}
