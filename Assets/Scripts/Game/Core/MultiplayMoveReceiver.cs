using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MSLIMA.Serializer;
using Moves;

public class MultiplayMoveReceiver : PlayerMoveReceiver {
    private PhotonView _pv;
    private static Dictionary<TeamColor, Rowcol> InitialRowcolDictionary;

    private void Awake() {
        _pv = GetComponent<PhotonView>();
        Serializer.RegisterCustomType<MoveConfig>((byte)'A');
        _executer = FindObjectOfType<MoveExecuter>();
        InitialRowcolDictionary = new Dictionary<TeamColor, Rowcol>();

        InitializeTeamColor();
    }

    protected override void InitializeTeamColor() {
        string teamColorID = "TeamColor";
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(teamColorID)) {
            if (_pv.IsMine) {
                MyColor = (TeamColor)PhotonNetwork.LocalPlayer.CustomProperties[teamColorID];
                OpponentColor = ExTeamColor.GetOpponentColor(MyColor);
            }
        }
    }

    public override void ExecuteMoves(MoveConfig[] moves) {
        _pv.RPC("ExecuteMoves", RpcTarget.All, (byte)MyColor, moves);
    }

    [PunRPC]
    private void ExecuteMoves(byte caster, MoveConfig[] moves) {
        TeamColor casterColor = (TeamColor)caster;
        _executer.ExecuteAll(casterColor, moves);
    }

    public bool CheckInitializeCompleted(TeamColor color) {
        return InitialRowcolDictionary.TryGetValue(color, out Rowcol value);
    }

    public Rowcol GetInitialRowcol(TeamColor color) {
        return InitialRowcolDictionary[color];
    }

    public void InitialSelectComplete(TeamColor color, Rowcol initialRowcol) {
        _pv.RPC("InitialSelectComplete", RpcTarget.All, (byte)color, initialRowcol.row, initialRowcol.column);
    }

    [PunRPC]
    private void InitialSelectComplete(byte color, int row, int col) {
        InitialRowcolDictionary.Add((TeamColor)color, new Rowcol(row, col));
    }
}
