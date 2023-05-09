using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

public class PlayerMain : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    [SerializeField]
    private Transform respawnPoints;
    private Transform respawnPoint;
    public int health = 100;
    public int killCount = 0;

    private PhotonView view;
    private UIDocument uidocumentMetrics;
    private GameObject metricsGameObj;
    private VisualElement root;
    private Label timerLabel;
    private ProgressBar healthBar;
    private int timeCount = 5 * 60; // five minutes

    private void OnEnable() {
        // when player is instantiated, enable playermovement script
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        playerShooting = GetComponent<PlayerShooting>();

        // start timer coroutine
        view = GetComponent<PhotonView>();
        metricsGameObj = GameObject.Find("UIDocumentMetrics");
        uidocumentMetrics = metricsGameObj.GetComponent<UIDocument>();
        root = uidocumentMetrics.rootVisualElement;
        timerLabel = root.Q<Label>("TimerLabel");
        timerLabel.text = "Timer: 05:00";
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

    public void TakeDamage(int damage) {
        health -= damage;
        healthBar.value = health;
        // if health drops below zero, call death function
        if ( health <= 0 ) {
            PlayerDeath();
        }
    }

    private void PlayerDeath() {
        // disable movement and shooting
        playerMovement.enabled = false;
        playerShooting.enabled = false;
        
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
    }

    private string CalculateTime(int seconds) {
        int mins = seconds / 60;
        int sec = seconds % 60;
        string str_seconds = (seconds < 10 ) ? $"0{sec}" : $"{sec}";
        return $"Timer: 0{mins}:{str_seconds}";
    }

    [PunRPC]
    void UpdateTime(int seconds) {
        timerLabel.text = CalculateTime(seconds);
    }
}
