using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamColor {
    NONE,
    RED,
    BLUE
}

public static class ExTeamColor {
    public static TeamColor GetRandomColor() {
        TeamColor c = (TeamColor)Random.Range((int)TeamColor.RED, (int)TeamColor.BLUE + 1);
        return c;
    }

    public static TeamColor GetOpponentColor(TeamColor color) {
        TeamColor opponentColor = (color == TeamColor.RED) ? TeamColor.BLUE : TeamColor.RED;
        return opponentColor;
    }
}