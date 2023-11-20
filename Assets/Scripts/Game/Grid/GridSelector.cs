using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

public class GridSelector : MonoBehaviour {
    [SerializeField] private GameObject _gridPointer;
    [SerializeField] private GridControl _gridControl;

    private void Start() {
        
    }

    private void Update() {
        Vector3 mousePos = ExMouse.GetMouseWorldPosition();
        _gridPointer.transform.position = _gridControl.PointToGridPosition(mousePos);
    }
}
