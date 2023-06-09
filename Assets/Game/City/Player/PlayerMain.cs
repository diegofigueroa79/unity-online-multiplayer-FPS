using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerMain : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private AudioListener audioListener;
    private Transform respawnPoints;
    private Transform respawnPoint;
    public int health = 100;
    public int killCount = 0;

    private PhotonView view;
    private UIDocument uidocumentMetrics;
    private GameObject metricsGameObj;
    private VisualElement root;
    private Label timerLabel;
    private Label killCountLabel;
    private ProgressBar healthBar;
    private int timeCount = 5 * 60; // five minutes

    private GameMain gameMain;

    private void OnEnable() {
        gameMain = GameObject.Find("GameManager").GetComponent<GameMain>();
        view = GetComponent<PhotonView>();

        // when player is instantiated, enable playermovement script
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        playerShooting = GetComponent<PlayerShooting>();
        playerShooting.enabled = true;

        if ( view.IsMine ) {
            // Set localplayer customproperties kills to zero
            Hashtable hash = new Hashtable();
            hash.Add("kills", killCount);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            cam.enabled = true;
            audioListener.enabled = true;
        }

        respawnPoints = GameObject.Find("RespawnPoints").transform;

        // start timer coroutine
        metricsGameObj = GameObject.Find("UIDocumentMetrics");
        uidocumentMetrics = metricsGameObj.GetComponent<UIDocument>();
        root = uidocumentMetrics.rootVisualElement;
        timerLabel = root.Q<Label>("TimerLabel");
        timerLabel.text = "Timer: 05:00";
        killCountLabel = root.Q<Label>("KillCountLabel");
        // grab health progress bar
        healthBar = root.Q<ProgressBar>("HealthBar");
        healthBar.value = health;
        var actualBar = root.Q(className: "unity-progress-bar__progress");
        actualBar.style.backgroundColor = Color.green;
        // only the master client will start the timer
        if ( PhotonNetwork.LocalPlayer.IsMasterClient ) {
            StartCoroutine(TimerCoroutine(timeCount));
        }
    }

    public void TakeDamage(int damage, int photonID, int photonViewID, int attackerActorNum) {
        if ( PhotonNetwork.LocalPlayer.ActorNumber == photonID ) {
            PhotonView receiverView = PhotonView.Find(photonViewID);
            PlayerMain receiverPlayerMain = receiverView.gameObject.GetComponent<PlayerMain>();
            receiverPlayerMain.health -= damage;
            receiverPlayerMain.healthBar.value = receiverPlayerMain.health;
            // if health drops below zero, call death function
            if ( receiverPlayerMain.health <= 0 ) {
                receiverPlayerMain.PlayerDeath();
                receiverView.RPC("UpdateKillCount", RpcTarget.Others, attackerActorNum);
            }
        }
    }

    private void PlayerDeath() {
        // disable movement and shooting
        playerMovement.enabled = false;
        playerShooting.enabled = false;
        PlayerRespawn();
    }

    private void PlayerRespawn() {
        // respawn to new spawn point
        int randID = Random.Range(0, respawnPoints.childCount);
        respawnPoint = respawnPoints.GetChild(randID);
        transform.position = respawnPoint.transform.position;
        // reset metrics
        health = 100;
        healthBar.value = health;
        // enable movement and shooting
        playerMovement.enabled = true;
        playerShooting.enabled = true;
    }

    public IEnumerator TimerCoroutine(int seconds) {
        while ( seconds > 0 ) {
            yield return new WaitForSeconds(1);
            --seconds;
            //timerLabel.text = calculatetime(seconds);
            view.RPC("UpdateTime", RpcTarget.All, seconds);
        }

        // game has finished
        view.RPC("EndGameRPC", RpcTarget.All);
    }

    private string CalculateTime(int seconds) {
        int mins = seconds / 60;
        int sec = seconds % 60;
        string str_seconds = (sec < 10 ) ? $"0{sec}" : $"{sec}";
        return $"Timer: 0{mins}:{str_seconds}";
    }

    [PunRPC]
    void UpdateTime(int seconds) {
        timerLabel.text = CalculateTime(seconds);
    }

    [PunRPC]
    void UpdateKillCount(int actorNum) {
        if ( PhotonNetwork.LocalPlayer.ActorNumber == actorNum ) {
            ++killCount;
            killCountLabel.text = $"Kill Count: {killCount}";
            Hashtable hash = new Hashtable();
            hash.Add("kills", killCount);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    [PunRPC]
    void EndGameRPC() {
        if ( view.IsMine ) {
            gameMain.EndGame();
        }
    }
}
