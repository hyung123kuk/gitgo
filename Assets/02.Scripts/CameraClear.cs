using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClear : MonoBehaviour
{
    public Camera MainCamera;

    float CameraMaxDistance = 6f; //카메라 최대FOV값
    RaycastHit hit;

    private void Awake()
    {
        StartCoroutine(ScriptEnable());
    }
    IEnumerator ScriptEnable() //스크립트를 껐다켜야 작동됩니다;;
    {
        gameObject.GetComponent<CameraClear>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<CameraClear>().enabled = true;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, MainCamera.transform.position - transform.position, Color.red);
        Debug.DrawRay(transform.position, (MainCamera.transform.position - transform.position).normalized * CameraMaxDistance, Color.blue);

        if (Physics.Raycast(transform.position, (MainCamera.transform.position - transform.position).normalized, out hit, CameraMaxDistance)) //자신과 메인카메라의 사이에 레이캐스팅하기
        {
            if (hit.transform.gameObject.tag != "Player" && hit.transform.gameObject.tag != "Enemy") //여기 태그있으면 충돌X
            {
                MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, MainCamera.transform.localPosition + Vector3.forward, Time.deltaTime * 10);
                MainCamera.transform.position = hit.point;
            }
            else
            {
                MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, new Vector3(0, 0, -CameraMaxDistance), Time.deltaTime * 5f);
                //Debug.DrawRay(transform.position, MainCamera.transform.position, Color.red);
            }
        }
    }
}
