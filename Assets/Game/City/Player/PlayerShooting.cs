using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

public class PlayerShooting : MonoBehaviour
{
    private bool shootInput;
    private PhotonView view;
    private RaycastHit rayhit;
    private GameObject other;
    [SerializeField]
    private Camera cam;
    private UIDocument uidocumentMetrics;
    private GameObject metricsGameObj;
    private VisualElement root;
    private Label killCountLabel;
    private PlayerMain playerMain;
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private LayerMask playerLayer;
    private AudioSource playerAudioSource;
    [SerializeField]
    private AudioClip gunshotClip;

    // Start is called before the first frame update
    void OnEnable()
    {
        view = GetComponent<PhotonView>();
        playerAudioSource = GetComponent<AudioSource>();
        metricsGameObj = GameObject.Find("UIDocumentMetrics");
        uidocumentMetrics = metricsGameObj.GetComponent<UIDocument>();
        root = uidocumentMetrics.rootVisualElement;
        killCountLabel = root.Q<Label>("KillCountLabel");
        playerMain = GetComponent<PlayerMain>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( view.IsMine ) {

            shootInput = Input.GetMouseButtonDown(0);
            
            if ( shootInput ) {
                muzzleFlash.Play();
                playerAudioSource.PlayOneShot(gunshotClip, 0.2f);

                if ( Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)), out rayhit, Mathf.Infinity, playerLayer ) ) {
                    if ( rayhit.collider != null ) {
                        other = rayhit.collider.gameObject;
                        if ( other.TryGetComponent<PhotonView>(out PhotonView otherView ) ) {
                            int photonID = otherView.Owner.ActorNumber;
                            view.RPC("CallTakeDamage", RpcTarget.Others, 10, photonID);
                        }
                    }
                }
            }

        }
    }

    [PunRPC]
    void CallTakeDamage(int damage, int photonID) {
        playerMain.TakeDamage(damage, photonID);
    }
}
