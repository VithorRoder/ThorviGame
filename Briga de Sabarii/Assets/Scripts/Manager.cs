using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Manager : MonoBehaviour
{
    public GameObject[] playerPrefabs;

    private TrocarPersonagens trocarPersonagens;

    void Start()
    {
        trocarPersonagens = FindObjectOfType<TrocarPersonagens>();
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        int playerIndex = trocarPersonagens.indiceAtual;
        Vector3 spawnPosition = new Vector3(96.76266f, 136.7889f, 0);
        PhotonNetwork.Instantiate(playerPrefabs[playerIndex].name, spawnPosition, Quaternion.identity);
    }
}