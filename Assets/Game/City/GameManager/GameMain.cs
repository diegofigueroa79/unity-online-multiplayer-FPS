using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;

public class GameMain : MonoBehaviour
{
    private int player_id;
    [SerializeField]
    private Transform initialSpawnPoints;
    private Transform spawnPoint;
    [SerializeField]
    private GameObject _player;

    private void Awake() {

        // SPAWN LOCAL PLAYER
        ////////////////////////////////////////////
        // get transform of spawn point by player id
        player_id = PhotonNetwork.LocalPlayer.ActorNumber;
        spawnPoint = initialSpawnPoints.GetChild(player_id);
        // instantiate player in a preassigned spawn point
        _player = PhotonNetwork.Instantiate("player", spawnPoint.position, Quaternion.identity);
        ////////////////////////////////////////////
    }
}
