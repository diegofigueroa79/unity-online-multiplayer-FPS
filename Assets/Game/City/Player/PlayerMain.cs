using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void OnEnable() {
        // when player is instantiated, enable playermovement script
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = true;       
    }
}
