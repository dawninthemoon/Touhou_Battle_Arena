using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;

public abstract class PlayerMoveReceiver : MonoBehaviour {
    protected MoveExecuter _executer;
    public static TeamColor MyColor {
        get;
        protected set;
    }
    public static TeamColor OpponentColor {
        get;
        protected set;
    }

    protected abstract void InitializeTeamColor();
    public abstract void ExecuteMoves(MoveConfig[] moves);
}
