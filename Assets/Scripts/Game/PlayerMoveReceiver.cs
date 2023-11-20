using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using RieslingUtils;

public class PlayerMoveReceiver : MonoBehaviour {
    private PhotonView _pv;
    private TeamColor _playerColor;
    private void Awake() {
        _pv = GetComponent<PhotonView>();
    }

    private void Start() {
        string teamColorID = "TeamColor";
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(teamColorID)) {
            _playerColor = ExEnum.Parse<TeamColor>((string)PhotonNetwork.LocalPlayer.CustomProperties[teamColorID]);
            if (!_pv.IsMine) {
                _playerColor = ExTeamColor.GetOpponentColor(_playerColor);
            }
        }
    }
}
