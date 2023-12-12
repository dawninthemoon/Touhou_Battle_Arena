using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;
using Cysharp.Threading.Tasks;

public class MoveExecuter : MonoBehaviour {
    [SerializeField] private MoveDataContainer _container;
    [SerializeField] private GridControl _gridControl;
    [SerializeField] private CharacterControl _characterControl;
    [SerializeField] private EffectControl _effectControl;
    [SerializeField] private MoveButtonControl _moveButtonControl;
    [SerializeField] private TurnControl _turnControl;
    private SharedData _sharedData;
    private Dictionary<TeamColor, MoveConfig[]> _requestedMoveConfigs;
    private TeamColor _preferenceColor;
    
    private void Awake() {
        _sharedData = new SharedData(
            _gridControl,
            _characterControl,
            _effectControl
        );
        _requestedMoveConfigs = new Dictionary<TeamColor, MoveConfig[]>();
        _preferenceColor = TeamColor.BLUE;
    }

    private void Start() {
        _turnControl.TurnEndEvent.AddListener(OnTurnEnd);
    }

    public void ExecuteAll(TeamColor player, MoveConfig[] moves) {
        _requestedMoveConfigs.Add(player, moves);

        if (_requestedMoveConfigs.Count == 2) {
            ExecuteAll().Forget();
        }
    }

    private async UniTaskVoid ExecuteAll() {
        for (int phase = 0; phase < MoveSlot.MaxSlots; ++phase) {
            TeamColor player = CompareSpellSpeeds(phase);
            if (player == TeamColor.NONE) {
                player = _preferenceColor;
            }
            
            await ExecuteMove(player, phase);
            await ExecuteMove(ExTeamColor.GetOpponentColor(player), phase);
        }

        _turnControl.FinishTurn();
    }

    private async UniTask ExecuteMove(TeamColor player, int phase) {
        MoveConfig move = _requestedMoveConfigs[player][phase];

        MoveBase instance = _container.GetMoveInstance(move.moveID);
        if (instance != null) {
            int index = move.executionAreaIndex;
            Rowcol origin = move.origin;
            _characterControl.UseEnergy(instance.Info.cost, player);
            await instance.StartExecute(player, index, origin, _sharedData);
        }
    }

    private TeamColor CompareSpellSpeeds(int phase) {
        MoveConfig blueMove = _requestedMoveConfigs[TeamColor.BLUE][phase];
        MoveConfig redMove = _requestedMoveConfigs[TeamColor.RED][phase];

        MoveBase blueInstance = _container.GetMoveInstance(blueMove.moveID);
        MoveBase redInstance = _container.GetMoveInstance(redMove.moveID);

        if (blueInstance.Info.spellSpeed > redInstance.Info.spellSpeed) {
            return TeamColor.BLUE;
        }
        else if (blueInstance.Info.spellSpeed < redInstance.Info.spellSpeed) {
            return TeamColor.RED;
        }
        return TeamColor.NONE;
    }

    private void OnTurnEnd() {

        _preferenceColor = ExTeamColor.GetOpponentColor(_preferenceColor);
        _requestedMoveConfigs.Clear();
        _moveButtonControl.SetButtonInteraction(true);

        // TODO: 수정
        _characterControl.GainEnergy(20, TeamColor.BLUE);
        _characterControl.GainEnergy(20, TeamColor.RED);
    }
}
