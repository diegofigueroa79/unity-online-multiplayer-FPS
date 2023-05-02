using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

public class LobbyMain : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private UIDocument lobbyUIDocument;
    private VisualElement root;
    private Button createButton;
    private Button joinButton;
    private TextField usernameInput;
    private TextField createInput;
    private TextField joinInput;

    private void Start() {
        root = lobbyUIDocument.rootVisualElement;
        createButton = root.Q<Button>("CreateButton");
        joinButton = root.Q<Button>("JoinButton");
        usernameInput = root.Q<TextField>("UsernameTextField");
        createInput = root.Q<TextField>("CreateTextField");
        joinInput = root.Q<TextField>("JoinTextField");

        createButton.clicked += () => CreateRoom();
        joinButton.clicked += () => JoinRoom();
    }

    private void CreateRoom() {
        PhotonNetwork.NickName = usernameInput.text;
        PhotonNetwork.CreateRoom(roomName: createInput.text);
    }

    private void JoinRoom() {
        PhotonNetwork.NickName = usernameInput.text;
        PhotonNetwork.JoinRoom(roomName: joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Room");
    }
}
