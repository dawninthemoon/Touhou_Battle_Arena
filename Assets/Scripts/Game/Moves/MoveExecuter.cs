using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;
using Cysharp.Threading.Tasks;

public class MoveExecuter : MonoBehaviour {
    [SerializeField] private MoveDataContainer _container;
    [SerializeField] private MoveAreaSelector _selector;
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

    public void RequestExecute(string moveID) {
        if (_currentSlotTop == MaxSlots) {
            return;
        }
        MoveBase instance = _container.GetMoveInstance(moveID);
        RequestExecuteAsync(instance).Forget();
    }

    private async UniTaskVoid RequestExecuteAsync(MoveBase instance) {
        bool isRelative = instance.Info.isRelativeForCharacter;
        int areaIndex = await _selector.SelectExecutionArea(instance.GetExecutionArea(), isRelative);
        _requestedMoves[_currentSlotTop++] = new MoveConfig(instance.Info.moveID, areaIndex);
    }

    public void RequestExecute(string moveID, int areaIndex) {
        if (_currentSlotTop == MaxSlots) {
            return;
        }
        MoveBase instance = _container.GetMoveInstance(moveID);
        _requestedMoves[_currentSlotTop++] = new MoveConfig(instance.Info.moveID, areaIndex);
    }

    public void ExecuteAll() {
        //TeamColor player = TeamColor.RED;
        //instance.Execute(player, areaIndex, _sharedData);
    }
}
