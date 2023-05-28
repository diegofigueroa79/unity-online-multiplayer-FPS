using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    private UIDocument endgameUIDocument;
    private VisualElement root;
    private VisualElement playersContainer;
    private List<VisualElement> playerContainerList;
    private Button quitBtn;
    private Button playAgainBtn;
    private Label playerLabel;
    private Label killLabel;


    public void EnableEndGameUI() {
        endgameUIDocument.enabled = true;
        root = endgameUIDocument.rootVisualElement;
        playersContainer = root.Q<VisualElement>("PlayersContainer");
        playerContainerList = playersContainer.Query<VisualElement>("PlayerContainer").ToList();

        quitBtn = root.Q<Button>("QuitBtn");
        playAgainBtn = root.Q<Button>("PlayAgainBtn");
        quitBtn.clicked += () => QuitGame();
        playAgainBtn.clicked += () => PlayAgain();
    }

    public void DisableEndGameUI() {
        endgameUIDocument.enabled = false;
    }

    public void UpdateScoreBoard() {
        // Get count of players in room
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        for (int i = 0; i < playerCount; i++) {
            string nickname = PhotonNetwork.PlayerList[i].NickName;
            int kills = (int)PhotonNetwork.PlayerList[i].CustomProperties["kills"];
            VisualElement container = playerContainerList[i];
            List<Label> labels = container.Query<Label>().ToList();
            labels[0].text = nickname; // set label for player name to nickname
            labels[1].text = $"{kills}"; // set label for kills
        }
    }

    private void QuitGame() {
        Application.Quit();
    }

    private void PlayAgain() {
        if ( PhotonNetwork.IsMasterClient ) {
            PhotonNetwork.LoadLevel("CityLoading");
        }
    }

}
