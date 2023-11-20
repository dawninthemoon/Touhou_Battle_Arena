using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;
using Cysharp.Threading.Tasks;
using RieslingUtils;
using ExitGames.Client.Photon.StructWrapping;

public class MoveSelector : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;
    [SerializeField, Tooltip("Temp Option")] private CharacterControl _characterCtrl;
    private ObjectPool<GameObject> _gridMarkerPool;
    List<ExecutionArea> _executionAreas;
    private int _selectedAreaIndex;
    private bool _isRelativeForCharacter;
    private GameObject[] _markers;

    private void Awake() {
        AssetLoader.Instance.LoadAssetAsync<GameObject>("GridMarker", (op) => {
            _gridMarkerPool = new ObjectPool<GameObject>(
                10,
                () => Instantiate(op.Result),
                (x) => x.gameObject.SetActive(true),
                (x) => x.gameObject.SetActive(false)
            );
            _markers = new GameObject[3] {
                _gridMarkerPool.GetObject(),
                _gridMarkerPool.GetObject(),
                _gridMarkerPool.GetObject()
            };
        });
    }

    private void Update() {
        if (_executionAreas != null) {
            AreaSelectProgress();
        }
    }

    private void AreaSelectProgress() {
        int t = 0;
        Rowcol target;
        foreach (Rowcol rc in _executionAreas[_selectedAreaIndex].Rowcols) {
            target = _isRelativeForCharacter ? _characterCtrl.MyCharacterRowcol + rc : rc;
            _markers[t].transform.position = _gridControl.RowcolToPoint(target);
            ++t;
        }

        Rowcol curr = _gridControl.PointToRowcol(ExMouse.GetMouseWorldPosition());
        target = _isRelativeForCharacter ? curr - _characterCtrl.MyCharacterRowcol : curr;
        if (!_executionAreas[_selectedAreaIndex].Contains(target)) {
            int numOfAreas = _executionAreas.Count;
            for (int i = 0; i < numOfAreas; ++i) {
                if ((i != _selectedAreaIndex) && _executionAreas[i].Contains(target)) {
                    _selectedAreaIndex = i;
                    break;
                }
            }
        }
}

    public async UniTask<int> SelectExecutionArea(List<ExecutionArea> executionAreas, bool isRelativeForCharacter) {
        _selectedAreaIndex = 0;
        _executionAreas = executionAreas;
        _isRelativeForCharacter = isRelativeForCharacter;
        
        ShowExecutionAreas(executionAreas, isRelativeForCharacter);

        await UniTask.Yield();

        return _selectedAreaIndex;
    }
    
    private void ShowExecutionAreas(List<ExecutionArea> executionAreas, bool isRelativeForCharacter) {
        int numOfAreas = executionAreas.Count;
        for (int i = 0; i < numOfAreas; ++i) {
            foreach (Rowcol rc in executionAreas[i].Rowcols) {
                Rowcol target = isRelativeForCharacter ? _characterCtrl.MyCharacterRowcol + rc : rc;
                _gridControl.HighlightTileObject(target);
            }
        }
    }
}
