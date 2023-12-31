using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamColor {
    NONE,
    BLUE,
    RED,
    COUNT
}

public static class ExTeamColor {
    public static TeamColor GetRandomColor() {
        TeamColor c = (TeamColor)Random.Range((int)TeamColor.BLUE, (int)TeamColor.RED + 1);
        return c;
    }

    public static TeamColor GetOpponentColor(TeamColor color) {
        if (color == TeamColor.NONE)
            return TeamColor.NONE;
            
        TeamColor opponentColor = (color == TeamColor.RED) ? TeamColor.BLUE : TeamColor.RED;
        return opponentColor;
    }

    public static TeamColor GetOpponent(this TeamColor color) {
        if (color == TeamColor.NONE)
            return TeamColor.NONE;
            
        TeamColor opponentColor = (color == TeamColor.RED) ? TeamColor.BLUE : TeamColor.RED;
        return opponentColor;
    }
}