using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour {
    private static readonly int GridSize = 32;
    private CustomGrid<GameObject> _grid;
    private void Awake() {
        _grid = new CustomGrid<GameObject>(4, 3, Vector3.zero, GridSize, GridSize);
    }
}
