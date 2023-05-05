using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView view;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float speed = 3.0f;
    [SerializeField]
    private float sensitivity = 5.0f;


    private void OnEnable() {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ( view.IsMine ) {

            // PLAYER MOVEMENT WITH KEYBOARD
            ////////////////////////////////////////////
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            float y = 0.0f;

            Vector3 input = new Vector3(x: x, y: y, z: z);
            input.Normalize();

            float velocity = speed * Time.deltaTime;

            transform.position += input * velocity;
            
            // PLAYER ROTATION WITH MOUSE
            ////////////////////////////////////////////
            //float mouse_x = sensitivity * Input.GetAxis("Mouse X");
            //float mouse_y = sensitivity * Input.GetAxis("Mouse Y");

        }
    }
}
