using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;
using Cysharp.Threading.Tasks;

public class MoveExecuter : MonoBehaviour {
    private struct RequestedMoveConfig {
        public TeamColor player;
        public MoveConfig[] moves;
        public RequestedMoveConfig(TeamColor p, MoveConfig[] m) {
            player = p;
            moves = m;
        }
    }

    [SerializeField] private MoveDataContainer _container;
    [SerializeField] private GridControl _gridControl;
    [SerializeField] private CharacterControl _characterControl;
    private SharedData _sharedData;
    private int _requestesOnQueue;
    private RequestedMoveConfig[] _requestedMoveConfigs;
    
    private void Awake() {
        _sharedData = new SharedData(_gridControl, _characterControl);
        _requestedMoveConfigs = new RequestedMoveConfig[2];
    }

    public void ExecuteAll(TeamColor player, MoveConfig[] moves) {
        _requestedMoveConfigs[_requestesOnQueue++] = new RequestedMoveConfig(player, moves);

        if (_requestesOnQueue == 2) {
            ExecuteAll().Forget();
        }
    }

    private async UniTaskVoid ExecuteAll() {
        _requestesOnQueue = 0;

        for (int phase = 0; phase < MoveSlot.MaxSlots; ++phase) {
            for (int playerIdx = 0; playerIdx < 2; ++playerIdx) {
                TeamColor player = _requestedMoveConfigs[playerIdx].player;
                MoveConfig[] moves = _requestedMoveConfigs[playerIdx].moves;

                MoveBase instance = _container.GetMoveInstance(moves[phase].moveID);
                if (instance != null) {
                    int index = moves[phase].executionAreaIndex;
                    Rowcol origin = moves[phase].origin;
                    await instance.Execute(player, index, origin, _sharedData);
                }
            }
        }
    }
}
