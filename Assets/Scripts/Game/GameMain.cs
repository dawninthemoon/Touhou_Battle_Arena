using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MSLIMA.Serializer;

public class GameMain : MonoBehaviour {
    private PlayerMoveReceiver _playerMoveReceiver;

    private void Awake() {
        Serializer.RegisterCustomType<Moves.MoveConfig>((byte)'A');
        var reciver = PhotonNetwork.Instantiate("PlayerMoveReciver", Vector3.zero, Quaternion.identity);
        _playerMoveReceiver = reciver.GetComponent<PlayerMoveReceiver>();
    }
}
