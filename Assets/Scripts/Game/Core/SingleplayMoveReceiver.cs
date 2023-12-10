using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;

public class SingleplayMoveReceiver : PlayerMoveReceiver {
    private void Awake() {
        _executer = FindObjectOfType<MoveExecuter>();
        InitializeTeamColor();
    }

    protected override void InitializeTeamColor() {
        MyColor = TeamColor.BLUE;
        OpponentColor = TeamColor.RED;
    }

    public override void ExecuteMoves(MoveConfig[] moves) {
        _executer.ExecuteAll(MyColor, moves);
    }

    public void ExecuteMoves(MoveConfig[] moves, TeamColor color) {
        _executer.ExecuteAll(color, moves);
    }
}
