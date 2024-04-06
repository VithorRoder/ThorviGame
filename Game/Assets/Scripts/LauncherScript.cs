using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{

  public GameObject ConectedScreenS;
  public GameObject ConectedScreenF;

  void Awake()
  {

  }

  public void OnClick_ConnectBt()
  {
    PhotonNetwork.ConnectUsingSettings();
  }


  public override void OnConnectedToMaster()
  {
    PhotonNetwork.JoinLobby(TypedLobby.Default);
  }

  public override void OnDisconnected(DisconnectCause cause)
  {
    ConectedScreenF.SetActive(true);
  }

  public override void OnJoinedLobby()
  {

    if (ConectedScreenF.activeSelf)
    {
      ConectedScreenF.SetActive(false);
    }

    ConectedScreenS.SetActive(true);
  }


}
