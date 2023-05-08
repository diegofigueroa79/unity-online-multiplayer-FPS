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

    // Start is called before the first frame update
    void OnEnable()
    {
        view = GetComponent<PhotonView>();
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
                if ( Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)), out rayhit ) ) {
                    if ( rayhit.collider != null ) {
                        other = rayhit.collider.gameObject;
                        if ( TryGetComponent<PlayerMain>(out PlayerMain otherPlayerMain ) ){
                            otherPlayerMain.TakeDamage(damage: 10);
                            if ( otherPlayerMain.health <= 0 ) {
                                killCountLabel.text = $"Kill Count: {playerMain.killCount}";
                            }
                        }
                    }
                }
            }

        }
    }
}
