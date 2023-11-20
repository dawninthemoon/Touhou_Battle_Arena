using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;
using Cysharp.Threading.Tasks;

public class GridSelector : MonoBehaviour {
    [SerializeField] private GameObject _gridPointer;
    [SerializeField] private GridControl _gridControl;
    private bool _isSelecting;

    private void Start() {
        
    }

    private void Update() {
        if (!_isSelecting) {
            return;
        }
        _gridPointer.transform.position = GetGridPosition();
    }

    public async UniTask<Rowcol> SelectGrid() {
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
        _gridPointer.gameObject.SetActive(active);
    }
}
