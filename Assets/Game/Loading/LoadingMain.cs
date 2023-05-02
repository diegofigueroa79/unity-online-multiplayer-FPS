using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LoadingMain : MonoBehaviourPunCallbacks
{
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client
        // and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        // First we need to connect to master server
        PhotonNetwork.ConnectUsingSettings();
    }

    // callback once we've connected to master server
    public override void OnConnectedToMaster() {
        // Now we can join the main lobby
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // load up lobby scene
        SceneManager.LoadScene("Lobby");
    }

}
