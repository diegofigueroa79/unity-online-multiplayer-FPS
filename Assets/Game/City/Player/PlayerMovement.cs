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
    private float sensitivity = 50.0f;
    private float xRotation;
    private float x;
    private float y;
    private float z;
    private float mouse_x;
    private float mouse_y;
    private float velocity;


    private void OnEnable() {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( view.IsMine ) {

            // PLAYER MOVEMENT WITH KEYBOARD
            ////////////////////////////////////////////
            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
            y = 0.0f;

            Vector3 input = new Vector3(x: x, y: y, z: z);
            input.Normalize();

            velocity = speed * Time.deltaTime;

            transform.position += input * velocity;
            
            // PLAYER ROTATION WITH MOUSE
            ////////////////////////////////////////////
            mouse_x = sensitivity * Input.GetAxisRaw("Mouse X");
            mouse_y = sensitivity * Input.GetAxisRaw("Mouse Y");

            // rotate camera to look up and down
            xRotation -= mouse_y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // rotate transform to look left and right
            transform.Rotate(Vector3.up * mouse_x);
        }
    }
}
