using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUi : MonoBehaviour
{

    private void OnEnable()
    {
        StartCoroutine(Setting());
    }

    IEnumerator Setting()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
