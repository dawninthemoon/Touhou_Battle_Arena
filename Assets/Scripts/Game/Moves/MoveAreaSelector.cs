using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;
using Cysharp.Threading.Tasks;
using RieslingUtils;

public class MoveAreaSelector : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;
    [SerializeField, Tooltip("Temp Options")] private GameObject _characterIllusionPrefab;
    private ObjectPool<GameObject> _gridMarkerPool;
    List<ExecutionArea> _executionAreas;
    private int _selectedAreaIndex;
    private bool _isRelativeForCharacter;
    private List<GameObject> _gridMarkers;
    private Rowcol _prevMouseRowcol;
    private Rowcol _casterPosition;

    // TODO: Remove this
    private List<GameObject> _illusionsList = new List<GameObject>();

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

            Rowcol target = _isRelativeForCharacter ? curr - _casterPosition : curr;
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

        int areaSize = (_executionAreas != null) ? _executionAreas[_selectedAreaIndex].Size : 0;
        int diff = areaSize - _gridMarkers.Count;
        for (int i = 0; i < Mathf.Abs(diff); ++i) {
            if (diff > 0) {
                _gridMarkers.Add(_gridMarkerPool.GetObject());
            }
            else if (diff < 0) {
                _gridMarkerPool.ReturnObject(_gridMarkers[i]);
                _gridMarkers.RemoveAt(i--);
                ++diff;
            }
        }

        if (_executionAreas != null) {
            Rowcol curr = _gridControl.PointToRowcol(ExMouse.GetMouseWorldPosition());
            foreach (Rowcol rc in _executionAreas[_selectedAreaIndex].Rowcols) {
                Rowcol target = _isRelativeForCharacter ? _casterPosition + rc : rc + curr;
                _gridMarkers[t].transform.position = _gridControl.RowcolToPoint(target);
                _gridMarkers[t].SetActive(_gridControl.IsValidRowcol(target));
                ++t;
            }
        }
    }

    public void AddCharacterIllusion(Rowcol rowcol) {
        Vector3 position = _gridControl.RowcolToPoint(rowcol);
        position.y += 12f;
        GameObject illusion = Instantiate(_characterIllusionPrefab, position, Quaternion.identity);
        _illusionsList.Add(illusion);
    }

    public void RemoveAllIllusions() {
        for (int i = 0; i < _illusionsList.Count; ++i) {
            var obj = _illusionsList[i];
            _illusionsList.RemoveAt(i--);
            DestroyImmediate(obj);
        }
    }

    public async UniTask<(int, Rowcol)> SelectExecutionArea(List<ExecutionArea> executionAreas, bool isRelativeForCharacter, Rowcol casterPos) {
        _selectedAreaIndex = 0;
        _executionAreas = executionAreas;
        _isRelativeForCharacter = isRelativeForCharacter;
        _prevMouseRowcol = _gridControl.PointToRowcol(ExMouse.GetMouseWorldPosition());
        _casterPosition = casterPos;

        UpdateGridMarkers();
        ShowExecutionAreas(executionAreas, isRelativeForCharacter);

        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));

        Rowcol curr = _gridControl.PointToRowcol(ExMouse.GetMouseWorldPosition());
        Rowcol origin = _isRelativeForCharacter ? _casterPosition : curr;
        _executionAreas = null;
        UpdateGridMarkers();
        _gridControl.RemoveAllHighlights();

        return (_selectedAreaIndex, origin);
    }
    
    private void ShowExecutionAreas(List<ExecutionArea> executionAreas, bool isRelativeForCharacter) {
        int numOfAreas = executionAreas.Count;
        Rowcol curr = _gridControl.PointToRowcol(ExMouse.GetMouseWorldPosition());
        for (int i = 0; i < numOfAreas; ++i) {
            foreach (Rowcol rc in executionAreas[i].Rowcols) {
                Rowcol target = isRelativeForCharacter ? _casterPosition + rc : rc + curr;

                _gridControl.HighlightTile(target);
                _gridControl.HighlightObject(target);
            }
        }
    }

    private void HideExecutionAreas(List<ExecutionArea> executionAreas, bool isRelativeForCharacter) {
        int numOfAreas = executionAreas.Count;
        for (int i = 0; i < numOfAreas; ++i) {
            foreach (Rowcol rc in executionAreas[i].Rowcols) {
                Rowcol target = isRelativeForCharacter ? _casterPosition + rc : rc + _prevMouseRowcol;
                _gridControl.RemoveHighlightTile(target);
                _gridControl.RemoveHighlightObject(target);
            }
        }
    }
}
