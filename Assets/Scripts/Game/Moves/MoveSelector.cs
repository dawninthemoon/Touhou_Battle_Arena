using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;

public class MoveSelector : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;
    
    public void ShowExecutionAreas(List<ExecutionArea> executionAreas) {
        int numOfAreas = executionAreas.Count;
        for (int i = 0; i < numOfAreas; ++i) {
            foreach (Rowcol rc in executionAreas[i].Rowcols) {
                _gridControl.HighlightTileObject(rc);
            }
        }
    }
}
