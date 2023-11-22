using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public async UniTask<int> RequestExecuteAsync(string moveID) {
        if (_currentSlotTop == MaxSlots) {
            return -1;
        }
        MoveBase instance = _container.GetMoveInstance(moveID);
        bool isRelative = instance.Info.isRelativeForCharacter;
        int areaIndex = await _selector.SelectExecutionArea(instance.GetExecutionArea(), isRelative);
        
        _requestedMoves[_currentSlotTop++] = new MoveConfig(instance.Info.moveID, areaIndex);
        return _currentSlotTop - 1;
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
