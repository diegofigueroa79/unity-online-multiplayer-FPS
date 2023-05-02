using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Realtime;

public class RoomMain : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private UIDocument roomUIDocument;
    private VisualElement root;
    private VisualElement playerContainer;
    private List<Label> playerLabelList;
    private Label roomNameLabel;
    private Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        // initialize ui elements
        root = roomUIDocument.rootVisualElement;
        roomNameLabel = root.Q<Label>("RoomNameLabel");
        startButton = root.Q<Button>("StartButton");
        playerContainer = root.Q<VisualElement>("PlayerListContainer");

        // add startgame function to startbutton
        startButton.clicked += () => StartGame();

        // on start of game, set room name label to current room name
        roomNameLabel.text = $"Room: {PhotonNetwork.CurrentRoom.Name}";
        UpdatePlayerList();
    }

    void UpdatePlayerList(Player newPlayer = null) {
        
        playerLabelList = playerContainer.Query<Label>().ToList();

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (newPlayer != null) {
            playerLabelList[playerCount - 1].text = newPlayer.NickName;
            return;
        }

        for (int i = 0; i < playerCount; i++)
        {
            playerLabelList[i].text = PhotonNetwork.PlayerList[i].NickName;    
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList(newPlayer);
    }

    void StartGame() {
        if ( PhotonNetwork.IsMasterClient ) {
            PhotonNetwork.LoadLevel("City");
        }
    }

}
