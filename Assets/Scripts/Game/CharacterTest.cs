using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterTest : GridObject {
    private TeamColor _teamColor;
    public Rowcol Curr {
        get;
        private set;
    }

    public void MoveImmediate(Vector3 pos, Rowcol to) {
        transform.position = pos;
        Curr = to;
    }
}
