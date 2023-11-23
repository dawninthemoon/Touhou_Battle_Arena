using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using RieslingUtils;
using MSLIMA.Serializer;
using Moves;
using System.Linq;

public class PlayerMoveReceiver : MonoBehaviour {
    private PhotonView _pv;
    private MoveExecuter _executer;
    public static TeamColor MyColor;
    public static TeamColor OpponentColor;

    private void Awake() {
        _pv = GetComponent<PhotonView>();
        Serializer.RegisterCustomType<Moves.MoveConfig>((byte)'A');

        _executer = FindObjectOfType<MoveExecuter>();
    }

    private void Start() {
        string teamColorID = "TeamColor";
        Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(teamColorID) + ", " +_pv.IsMine);
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(teamColorID)) {
            if (_pv.IsMine) {
                MyColor = (TeamColor)PhotonNetwork.LocalPlayer.CustomProperties[teamColorID];
                OpponentColor = ExTeamColor.GetOpponentColor(MyColor);
                Debug.Log(MyColor + ", " + OpponentColor);
            }
        }
    }

    public void ExecuteMoves(MoveDataContainer container, MoveConfig[] moves) {
        _pv.RPC("ExecuteMoves", RpcTarget.All, (byte)MyColor, moves);
    }

    [PunRPC]
    private void ExecuteMoves(byte caster, MoveConfig[] moves) {
        TeamColor casterColor = (TeamColor)caster;
        _executer.ExecuteAll(casterColor, moves);
    }
}
