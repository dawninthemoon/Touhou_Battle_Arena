using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameMainForSingle : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;
    [SerializeField] private CharacterControl _characterControl;
    [SerializeField] private GridSelector _gridSelector;
    [SerializeField] private MoveButtonControl _moveButtonControl;
    [SerializeField] private EnemyMoveDecide _enemyMoveDecide;

    private void Awake() {

    }

    private async UniTaskVoid Start() {
        _moveButtonControl.SetButtonInteraction(false);

        if (PlayerMoveReceiver.MyColor == TeamColor.BLUE) {
            await SelectInitialRowcol();
            OpponentSelect();
        }
        else {
            OpponentSelect();
            await SelectInitialRowcol();
        }
        
        _moveButtonControl.SetButtonInteraction(true);
    }

    private async UniTask SelectInitialRowcol() {
        Rowcol initialRowcol = await _gridSelector.SelectGrid();
        _characterControl.PlaceCharacter(PlayerMoveReceiver.MyColor, initialRowcol);
    }

    private Rowcol OpponentSelect() {
        Rowcol initialRowcol = _enemyMoveDecide.GetInitialRowcol();
        _characterControl.PlaceCharacter(PlayerMoveReceiver.OpponentColor, initialRowcol);

        return initialRowcol;
    }
}
