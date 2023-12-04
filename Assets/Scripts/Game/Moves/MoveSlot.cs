using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using System.Linq;

public class MoveSlot : MonoBehaviour {
    [SerializeField] private MoveDataContainer _container;
    [SerializeField] private MoveAreaSelector _selector;
    [SerializeField] private MoveButtonControl _moveButtonControl;
    [SerializeField] private CharacterControl _characterControl;
    [SerializeField] private GridControl _gridControl;
    private PlayerMoveReceiver _playerMoveReceiver;
    private MoveConfig[] _requestedMoves;
    public static readonly int MaxSlots = 3;
    private int _currentSlotTop;
    private int _usedEnergies;

    private void Awake() {
        _requestedMoves = new MoveConfig[MaxSlots];
    }

    private void Start() {
        _playerMoveReceiver = FindAnyObjectByType<PlayerMoveReceiver>();
    }

    public async UniTask<int> RequestExecuteAsync(string moveID) {
        if (_currentSlotTop == MaxSlots) {
            return -1;
        }
        MoveBase instance = _container.GetMoveInstance(moveID);

        if (_characterControl.MyCharacterEnergy >= instance.Info.cost) {
            _characterControl.UseEnergy(instance.Info.cost);
            _usedEnergies += instance.Info.cost;

            bool isRelative = instance.Info.isRelativeForCharacter;
            (int, Rowcol) result = await _selector.SelectExecutionArea(
                instance.GetExecutionArea(),
                isRelative,
                GetCharacterRowcol()
            );

            if (result.Item1 != -1) {
                _requestedMoves[_currentSlotTop++] = new MoveConfig(instance.Info.moveID, result.Item1, result.Item2);
                return _currentSlotTop - 1;
            }
        }
        return -1;
    }

    public void RequestExecuteMovement(string moveID, int areaIndex) {
        if (_currentSlotTop == MaxSlots) {
            return;
        }

        MoveBase instance = _container.GetMoveInstance(moveID);
        Rowcol destination = GetCharacterRowcol() + instance.GetExecutionArea()[areaIndex].Single();

        if (!_gridControl.IsValidRowcol(destination)) {
            return;
        }

        _requestedMoves[_currentSlotTop++] = new MoveConfig(instance.Info.moveID, areaIndex, _characterControl.MyCharacterRowcol);
        _selector.AddCharacterIllusion(GetCharacterRowcol());
    }

    public void RequestExecuteAll() {
        if (_currentSlotTop == MaxSlots - 1) {
            return;
        }
        ResetSlot();

        _moveButtonControl.SetButtonInteraction(false);
        _playerMoveReceiver.ExecuteMoves(_container, _requestedMoves);
    }

    public void ResetSlot() {
        _characterControl.GainEnergy(_usedEnergies);
        _usedEnergies = 0;
        _currentSlotTop = 0;
        _selector.RemoveAllIllusions();
        _moveButtonControl.RemoveSlotImages();
    }

    public void ReSelectSlot() {
        ResetSlot();
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
