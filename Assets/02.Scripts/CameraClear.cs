using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraClear : MonoBehaviourPunCallbacks
{
    public Camera MainCamera;

    float CameraMaxDistance = 6f; //ī�޶� �ִ�FOV��
    RaycastHit hit;

    private void Awake()
    {
        StartCoroutine(ScriptEnable());
    }
    IEnumerator ScriptEnable() //��ũ��Ʈ�� �����Ѿ� �۵��˴ϴ�;;
    {
        gameObject.GetComponent<CameraClear>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<CameraClear>().enabled = true;
    }
    private void Update()
    {
        if (!photonView.IsMine) //���û��°� �ƴϸ� ����
        {
            return;
        }
        Debug.DrawRay(transform.position, MainCamera.transform.position - transform.position, Color.red);
        Debug.DrawRay(transform.position, (MainCamera.transform.position - transform.position).normalized * CameraMaxDistance, Color.blue);

        if (Physics.Raycast(transform.position, (MainCamera.transform.position - transform.position).normalized, out hit, CameraMaxDistance)) //�ڽŰ� ����ī�޶��� ���̿� ����ĳ�����ϱ�
        {
            if (hit.transform.gameObject.tag != "Player" && hit.transform.gameObject.tag != "Enemy") //���� �±������� �浹X
            {
                MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, MainCamera.transform.localPosition + Vector3.forward, Time.deltaTime * 10);
                MainCamera.transform.position = hit.point;
                Debug.Log("�浹1");
            }
            //else
            //{
            //    Debug.Log("�浹2");
            //    MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, new Vector3(0, 0, -CameraMaxDistance), Time.deltaTime * 5f);
            //    //Debug.DrawRay(transform.position, MainCamera.transform.position, Color.red);
            //}
        }
    }
}
