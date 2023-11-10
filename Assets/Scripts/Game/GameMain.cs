using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameMain : MonoBehaviour {
    private void Awake() {
        PhotonNetwork.Instantiate("GameTest", Vector3.zero, Quaternion.identity);
    }
}
