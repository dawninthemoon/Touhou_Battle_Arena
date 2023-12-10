using System.Collections;
using System.Collections.Generic;
using Moves;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyMoveDecide : MonoBehaviour {
    [SerializeField] private SingleplayMoveReceiver _playerMoveReceiver;
    [SerializeField] private MoveDataContainer _container;
    [SerializeField] private GridControl _gridControl;
    [SerializeField] private CharacterControl _characterControl;
    private MoveConfig[] _requestedMoves;
    private bool _execute;

    private void Awake() {
        _requestedMoves = new MoveConfig[MoveSlot.MaxSlots];
    }

    private async UniTaskVoid Start() {
        await UniTask.WaitUntil(() => _container.IsLoadCompleted && _execute);
        DecideMoveAndExecute();
    }

    public Rowcol GetInitialRowcol() {
        _execute = true;
        Rowcol initialRowcol = Rowcol.Zero;
        do {
            int row = Random.Range(0, _gridControl.Width);
            int column = Random.Range(0, _gridControl.Height);
            initialRowcol = new Rowcol(row, column);
        } while (!_gridControl.CanMoveTo(initialRowcol));
        return initialRowcol;
    }

    public void DecideMoveAndExecute() {
        int phase = 0;
        while (phase < MoveSlot.MaxSlots) {
            string moveID = Move_Movement.MoveID;
            int areaIndex = Random.Range(0, 4);

            MoveBase instance = _container.GetMoveInstance(moveID);
            Rowcol destination = GetCharacterRowcol(phase) + instance.GetExecutionArea()[areaIndex].Single();

            if (!_gridControl.CanMoveTo(destination)) {
                continue;
            }

            _requestedMoves[phase++] = new MoveConfig(instance.Info.moveID, areaIndex, _characterControl.OpponentCharacterRowcol);
        }
        _playerMoveReceiver.ExecuteMoves(_requestedMoves, PlayerMoveReceiver.OpponentColor);
    }

    private Rowcol GetCharacterRowcol(int currentPhase) {
        Rowcol rc = Rowcol.Zero;
        var moveAreas = _container.GetMoveInstance(Move_Movement.MoveID).GetExecutionArea();
        for (int i = 0; i < currentPhase; ++i) {
            if (_requestedMoves[i].moveID.Equals(Move_Movement.MoveID)) {
               rc += moveAreas[_requestedMoves[i].executionAreaIndex].Single();
            }
        }
        return rc + _characterControl.OpponentCharacterRowcol;
    }
}
