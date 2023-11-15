using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using MSLIMA.Serializer;

public class GameTest : MonoBehaviour {
    private PhotonView _photonView;
    private int _playerID;
    private Moves.MoveInfo[] _moves;

    private void Awake() {
        _photonView = GetComponent<PhotonView>();
        Serializer.RegisterCustomType<Moves.MoveInfo>((byte)'A');

        _playerID = 0;
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("ID")) {
            _playerID = (int)PhotonNetwork.LocalPlayer.CustomProperties["ID"];
            if (!_photonView.IsMine) {
                _playerID = 1 - _playerID;
            }
        }

        if (_photonView.IsMine) {
            MoveDataParser parser = new MoveDataParser();
            AssetLoader.Instance.LoadAssetAsync<TextAsset>("MoveInfo", (op) => {
                _moves = parser.Parse(op.Result.ToString());
            });
        }
    }
    
    private void Update() {
        if (_photonView.IsMine && Input.GetMouseButtonDown(0)) {
            _photonView.RPC("Func", RpcTarget.All, _moves[0]);
        }
    }

    [PunRPC]
    private void Func(Moves.MoveInfo move) {
        Debug.Log(move.ToString());
    }
}