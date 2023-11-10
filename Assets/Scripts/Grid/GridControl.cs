using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour {
    [SerializeField] private Vector2 _gridSize;
    private CustomGrid<CharacterTest> _grid;
    private void Awake() {
        _grid = new CustomGrid<CharacterTest>(4, 3, Vector3.zero, _gridSize.x, _gridSize.y);
    }

    public bool IsValidRowcol(Rowcol rowcol) {
        return _grid.IsValidRowcol(rowcol);
    }

    public Vector3 RowcolToPosition(Rowcol rc) {
        return _grid.RowcolToPointCenter(rc);
    }

    public Vector3 RowcolToPosition(int row, int column) {
        return _grid.RowcolToPointCenter(row, column);
    }

    public void InitializeGrid(GameObject prefab) {
        for (int row = 0; row < _grid.Height; ++row) {
            for (int col = 0; col < _grid.Width; ++col) {
                GameObject obj = Instantiate(prefab, _grid.RowcolToPointCenter(row, col), Quaternion.identity);
                Color color = Color.white;
                if ((row % 2 == 0 && col % 2 == 0) || (row % 2 > 0 && col % 2 > 0)) {
                    color = Color.black;
                }
                obj.transform.localScale = _gridSize;
                obj.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}
