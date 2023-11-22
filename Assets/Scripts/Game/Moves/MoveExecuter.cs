using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Moves;
using Cysharp.Threading.Tasks;

public class MoveExecuter : MonoBehaviour {
    [SerializeField] private MoveDataContainer _container;
    [SerializeField] private MoveAreaSelector _selector;
    [SerializeField] private MoveButtonControl _moveButtonControl;
    [SerializeField] private GridControl _gridControl;
    [SerializeField] private CharacterControl _characterControl;
    private MoveConfig[] _requestedMoves;
    private SharedData _sharedData;
    public static readonly int MaxSlots = 3;
    private int _currentSlotTop;

    private void Awake() {
        _requestedMoves = new MoveConfig[MaxSlots];
        _sharedData = new SharedData(_gridControl, _characterControl);
    }

    public async UniTask<int> RequestExecuteAsync(string moveID) {
        if (_currentSlotTop == MaxSlots) {
            return -1;
        }
        MoveBase instance = _container.GetMoveInstance(moveID);
        bool isRelative = instance.Info.isRelativeForCharacter;
        int areaIndex = await _selector.SelectExecutionArea(instance.GetExecutionArea(), isRelative, GetCharacterRowcol());
        
        _requestedMoves[_currentSlotTop++] = new MoveConfig(instance.Info.moveID, areaIndex);
        return _currentSlotTop - 1;
    }

    public void RequestExecuteMovement(string moveID, int areaIndex) {
        if (_currentSlotTop == MaxSlots) {
            return;
        }

        MoveBase instance = _container.GetMoveInstance(moveID);
        _requestedMoves[_currentSlotTop++] = new MoveConfig(instance.Info.moveID, areaIndex);
        _selector.AddCharacterIllusion(GetCharacterRowcol());
    }

    public void ExecuteAll() {
        _currentSlotTop = 0;
        _selector.RemoveAllIllusions();
        _moveButtonControl.RemoveSlotImages();

        //TeamColor player = TeamColor.RED;
        //instance.Execute(player, areaIndex, _sharedData);
    }

    private Rowcol GetCharacterRowcol() {
        Rowcol rc = Rowcol.Zero;
        var moveAreas = _container.GetMoveInstance(Move_Movement.MoveID).GetExecutionArea();
        for (int i = 0; i < _currentSlotTop; ++i) {
            if (_requestedMoves[i].moveID.Equals(Move_Movement.MoveID)) {
               rc += moveAreas[_requestedMoves[i].executionAreaIndex].Single();
            }
        }
        return rc + _characterControl.MyCharacterRowcol;
    }
}
