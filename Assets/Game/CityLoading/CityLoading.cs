using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CityLoading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.LoadLevel("City");
    }
}
