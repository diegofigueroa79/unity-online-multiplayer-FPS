using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

public class PlayerMain : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public int health = 100;
    public int killCount = 0;

    private PhotonView view;
    private UIDocument uidocumentMetrics;
    private GameObject metricsGameObj;
    private VisualElement root;
    private Label timerLabel;
    private int timeCount = 5 * 60; // five minutes

    private void OnEnable() {
        // when player is instantiated, enable playermovement script
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = true;

        // start timer coroutine
        view = GetComponent<PhotonView>();
        metricsGameObj = GameObject.Find("UIDocumentMetrics");
        uidocumentMetrics = metricsGameObj.GetComponent<UIDocument>();
        root = uidocumentMetrics.rootVisualElement;
        timerLabel = root.Q<Label>("TimerLabel");
        timerLabel.text = "Timer: 05:00";
        // only the master client will start the timer
        if ( PhotonNetwork.LocalPlayer.IsMasterClient ) {
            StartCoroutine(TimerCoroutine(timeCount));
        }     
    }

    public void TakeDamage(int damage) {
        health -= damage;
        // if health drops below zero, call death function
        if ( health <= 0 ) {
            PlayerDeath();
        }
    }

    private void PlayerDeath() {
        // disable movement
        playerMovement.enabled = false;
        // disable collider
        // start coroutine for fadein fadeout black screen?
        // respawn to new spawn point
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
