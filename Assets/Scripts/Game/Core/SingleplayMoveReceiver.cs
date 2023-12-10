using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;

public class SingleplayMoveReceiver : PlayerMoveReceiver {
    private void Awake() {
        InitializeTeamColor();
    }

    protected override void InitializeTeamColor() {
        MyColor = TeamColor.BLUE;
        OpponentColor = TeamColor.RED;
    }

    public override void ExecuteMoves(MoveDataContainer container, MoveConfig[] moves) {

    }
}
