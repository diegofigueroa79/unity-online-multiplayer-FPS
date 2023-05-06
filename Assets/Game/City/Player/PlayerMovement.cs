using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView view;
    [SerializeField]
    private Animator animatorController;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float speed = 3.0f;
    [SerializeField]
    private float sensitivity = 1000.0f;
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
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
            // SET WALKING ANIMATION
            if ( x != 0f || z != 0f ) {
                animatorController.SetBool("IsWalking", true);
            } else {
                animatorController.SetBool("IsWalking", false);
            }

            Vector3 move = transform.right * x + transform.forward * z;

            velocity = speed * Time.deltaTime;

            transform.position += move * velocity;
            
            // PLAYER ROTATION WITH MOUSE
            ////////////////////////////////////////////
            mouse_x = sensitivity * Input.GetAxis("Mouse X") * Time.deltaTime;
            mouse_y = sensitivity * Input.GetAxis("Mouse Y") * Time.deltaTime;

            // rotate camera to look up and down
            xRotation -= mouse_y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // rotate transform to look left and right
            transform.Rotate(Vector3.up * mouse_x);
        }
    }
}
