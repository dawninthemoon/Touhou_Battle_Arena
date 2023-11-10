using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTest : MonoBehaviour {
    [SerializeField] private GameObject _tilePrefab;
    private GridControl _gridControl;
    private void Awake() {
        _gridControl = GetComponent<GridControl>();
    }

    private void Start() {
        _gridControl.InitializeGrid(_tilePrefab);
    }
}
