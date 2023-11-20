using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;
using Cysharp.Threading.Tasks;

public class GridSelector : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;
    private GameObject _gridMarker;
    private bool _isSelecting;

    private void Awake() {
        AssetLoader.Instance.LoadAssetAsync<GameObject>("GridMarker", (op) => {
            _gridMarker = Instantiate(op.Result);
        });
    }

    private void Update() {
        if (!_isSelecting) {
            return;
        }
        _gridMarker.transform.position = GetGridPosition();
    }

    public async UniTask<Rowcol> SelectGrid() {
        await UniTask.WaitUntil(() => _gridMarker);

        SetActive(true);

        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));

        SetActive(false);
        return _gridControl.PointToRowcol(GetGridPosition());
    }

    private Vector3 GetGridPosition() {
        Vector3 mousePos = ExMouse.GetMouseWorldPosition();
        return _gridControl.PointToGridPosition(mousePos);
    }

    private void SetActive(bool active) {
        _isSelecting = active;
        _gridMarker.gameObject.SetActive(active);
    }
}
