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
    private List<GameObject> _gridMarkers;
    private Rowcol _prevMouseRowcol;

    private void Awake() {
        _gridMarkers = new List<GameObject>();

        AssetLoader.Instance.LoadAssetAsync<GameObject>("GridMarker", (op) => {
            _gridMarkerPool = new ObjectPool<GameObject>(
                10,
                () => Instantiate(op.Result),
                (x) => x.gameObject.SetActive(true),
                (x) => x.gameObject.SetActive(false)
            );
        });
    }

    private void Update() {
        if (_executionAreas != null) {
            AreaSelectProgress();
        }
    }

    private void AreaSelectProgress() {
        Rowcol curr = _gridControl.PointToRowcol(ExMouse.GetMouseWorldPosition());
        if (!curr.Equals(_prevMouseRowcol)) {
            HideExecutionAreas(_executionAreas, _isRelativeForCharacter);

            Rowcol target = _isRelativeForCharacter ? curr - _characterCtrl.MyCharacterRowcol : curr;
            if (_isRelativeForCharacter && !_executionAreas[_selectedAreaIndex].Contains(target)) {
                int numOfAreas = _executionAreas.Count;
                for (int i = 0; i < numOfAreas; ++i) {
                    if ((i != _selectedAreaIndex) && _executionAreas[i].Contains(target)) {
                        _selectedAreaIndex = i;
                        break;
                    }
                }
            }

            ShowExecutionAreas(_executionAreas, _isRelativeForCharacter);
            UpdateGridMarkers();

            _prevMouseRowcol = curr;
        }
    }

    private void UpdateGridMarkers() {
        int t = 0;

        int diff = _executionAreas[_selectedAreaIndex].Size - _gridMarkers.Count;

        for (int i = 0; i < Mathf.Abs(diff); ++i) {
            if (diff > 0) {
                _gridMarkers.Add(_gridMarkerPool.GetObject());
            }
            else if (diff < 0) {
                _gridMarkerPool.ReturnObject(_gridMarkers[i]);
                _gridMarkers.RemoveAt(i--);
            }
        }

        Rowcol curr = _gridControl.PointToRowcol(ExMouse.GetMouseWorldPosition());
        foreach (Rowcol rc in _executionAreas[_selectedAreaIndex].Rowcols) {
            Rowcol target = _isRelativeForCharacter ? _characterCtrl.MyCharacterRowcol + rc : rc + curr;
            _gridMarkers[t].transform.position = _gridControl.RowcolToPoint(target);
            _gridMarkers[t].SetActive(_gridControl.IsValidRowcol(target));
            ++t;
        }
    }

    public async UniTask<int> SelectExecutionArea(List<ExecutionArea> executionAreas, bool isRelativeForCharacter) {
        _selectedAreaIndex = 0;
        _executionAreas = executionAreas;
        _isRelativeForCharacter = isRelativeForCharacter;
        _prevMouseRowcol = _gridControl.PointToRowcol(ExMouse.GetMouseWorldPosition());

        UpdateGridMarkers();
        ShowExecutionAreas(executionAreas, isRelativeForCharacter);

        await UniTask.Yield();

        return _selectedAreaIndex;
    }
    
    private void ShowExecutionAreas(List<ExecutionArea> executionAreas, bool isRelativeForCharacter) {
        int numOfAreas = executionAreas.Count;
        Rowcol curr = _gridControl.PointToRowcol(ExMouse.GetMouseWorldPosition());
        for (int i = 0; i < numOfAreas; ++i) {
            foreach (Rowcol rc in executionAreas[i].Rowcols) {
                Rowcol target = isRelativeForCharacter ? _characterCtrl.MyCharacterRowcol + rc : rc + curr;

                _gridControl.HighlightTileObject(target);
            }
        }
    }

    private void HideExecutionAreas(List<ExecutionArea> executionAreas, bool isRelativeForCharacter) {
        int numOfAreas = executionAreas.Count;
        Rowcol curr = _gridControl.PointToRowcol(ExMouse.GetMouseWorldPosition());
        for (int i = 0; i < numOfAreas; ++i) {
            foreach (Rowcol rc in executionAreas[i].Rowcols) {
                Rowcol target = isRelativeForCharacter ? _characterCtrl.MyCharacterRowcol + rc : rc + _prevMouseRowcol;
                _gridControl.RemoveHighlightTileObject(target);
            }
        }
    }
}
