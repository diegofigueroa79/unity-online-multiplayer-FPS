using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public int health = 100;

    private void OnEnable() {
        // when player is instantiated, enable playermovement script
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = true;       
    }

    private void TakeDamage(int damage) {
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
}
