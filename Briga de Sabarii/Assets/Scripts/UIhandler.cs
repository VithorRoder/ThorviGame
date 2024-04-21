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
    public GameObject AcessandoSala;
    public GameObject CriandoSala;
    public GameObject FalhaEncontrarSala;

    public void OnClick_JoinRoom()
    {
        FalhaEncontrarSala.SetActive(false);
        CriandoSala.SetActive(false);
        AcessandoSala.SetActive(true);
        PhotonNetwork.JoinRoom(JoinRoomTF.text, null);

        StartCoroutine(WaitForJoin());
    }

    private IEnumerator WaitForJoin()
    {
        yield return new WaitForSeconds(10f);

        if (!PhotonNetwork.InRoom)
        {
            AcessandoSala.SetActive(false);
            CriandoSala.SetActive(false);
            FalhaEncontrarSala.SetActive(true);
        }
    }

    public void OnClick_CreateRoom()
    {
        FalhaEncontrarSala.SetActive(false);
        AcessandoSala.SetActive(false);
        CriandoSala.SetActive(true);
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