using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView view;
    public float speed = 3.0f;

    private void OnEnable() {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( view.IsMine ) {

            // PLAYER MOVEMENT AND ROTATION
            ////////////////////////////////////////////
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            float y = 0.0f;

            Vector3 input = new Vector3(x: x, y: y, z: z);
            input.Normalize();

            float velocity = speed * Time.deltaTime;

            transform.position += input * velocity;
            
        }
    }
}
