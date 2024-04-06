using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class UIhandler : MonoBehaviourPunCallbacks
{
    public InputField CriarRoomTF;
    public InputField JoinRoomTF;

    public void OnClick_JoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinRoomTF.text, null);
    }

    public void OnClick_CreateRoom()
    {
        PhotonNetwork.CreateRoom(CriarRoomTF.text, new RoomOptions { MaxPlayers = 6 }, null);
    }

    public override void OnJoinedRoom()
    {
        print("Sala Acessada Com Sucesso !");
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Falha ao Tentar Encontrar a Sala." + returnCode + " Mensagem " + message);
    }
}