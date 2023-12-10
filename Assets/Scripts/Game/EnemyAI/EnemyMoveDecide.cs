using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveDecide : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;

    public Rowcol GetInitialRowcol() {
        Rowcol initialRowcol = Rowcol.Zero;
        do {
            int row = Random.Range(0, _gridControl.Width);
            int column = Random.Range(0, _gridControl.Height);
            initialRowcol = new Rowcol(row, column);
        } while (!_gridControl.CanMoveTo(initialRowcol));
        return initialRowcol;
    }
}
