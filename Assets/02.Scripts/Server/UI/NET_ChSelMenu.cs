using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NET_ChSelMenu : MonoBehaviour
{
    [SerializeField]
    private Camera uiCamera;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private RectTransform rectParent;
    [SerializeField]
    private RectTransform rectInfo;
    [SerializeField]
    NET_UIPlayer net_UIPlayer;
    [SerializeField]
    NET_PartyPlayer net_partyPlayer;

    public Vector3 offset = Vector3.zero;
    public Transform targetTr;

    

    public void Start()
    {
        canvas = GameObject.Find("EnemyHp_Bar_UI").GetComponent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectInfo = this.gameObject.GetComponent<RectTransform>();

        NET_UIPlayer[] net_uiplayes = GameObject.FindObjectsOfType<NET_UIPlayer>();

        foreach (NET_UIPlayer myUIPlaye in net_uiplayes) {
            if (myUIPlaye.GetComponent<PhotonView>().IsMine)
            {
                net_UIPlayer = myUIPlaye;
                break;
            }
        }

        NET_PartyPlayer[] net_partyyplayes = GameObject.FindObjectsOfType<NET_PartyPlayer>();

        foreach (NET_PartyPlayer myPartyPlaye in net_partyyplayes)
        {
            if (myPartyPlaye.GetComponent<PhotonView>().IsMine)
            {
                net_partyPlayer = myPartyPlaye;
                break;
            }
        }

        MenuOn();
    }


    public void MenuOn()
    {
        if (targetTr == null)
        {
            Destroy(gameObject);
            return;
        }

        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        rectInfo.localPosition = localPos;

    }

    public void TradeButton()
    {
        net_UIPlayer.Trade();
        net_UIPlayer.DestroyMenu();
    }

    public void InfoButton()
    {
        net_UIPlayer.Info();
        net_UIPlayer.DestroyMenu();
    }

    public void PartyInvite()
    {
        net_partyPlayer.PartyInvite();
        net_UIPlayer.DestroyMenu();
    }
}
