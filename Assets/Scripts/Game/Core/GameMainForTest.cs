using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameMainForTest : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;
    [SerializeField] private CharacterControl _characterControl;
    [SerializeField] private GridSelector _gridSelector;

    private void Awake() {
    }

    private async UniTaskVoid Start() {
        Rowcol initialRowcol = await _gridSelector.SelectGrid();
        _characterControl.PlaceCharacter(TeamColor.BLUE, initialRowcol);

        initialRowcol = await _gridSelector.SelectGrid();
        _characterControl.PlaceCharacter(TeamColor.RED, initialRowcol);
    }
}
