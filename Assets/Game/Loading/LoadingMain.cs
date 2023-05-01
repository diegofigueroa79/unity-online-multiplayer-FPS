using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LoadingMain : MonoBehaviourPunCallbacks
{
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
