using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShooting : MonoBehaviour
{
    private bool shootInput;
    private PhotonView view;
    private RaycastHit rayhit;
    private GameObject other;
    [SerializeField]
    private Camera cam;

    // Start is called before the first frame update
    void OnEnable()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( view.IsMine ) {

            shootInput = Input.GetMouseButtonDown(0);
            
            if ( shootInput ) {
                if ( Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)), out rayhit ) ) {
                    if ( rayhit.collider != null ) {
                        other = rayhit.collider.gameObject;
                        if ( TryGetComponent<PlayerMain>(out PlayerMain playerMain ) ){
                            playerMain.TakeDamage(damage: 10);
                        }
                    }
                }
            }

        }
    }
}
