using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour {
    [SerializeField] private int _width, _height;
    [SerializeField] private float _ceilSize;
    [SerializeField] private Vector3 _gridOrigin;
    [SerializeField, Tooltip("Temp Option")] private Sprite _tileSprite;
    [SerializeField, Tooltip("Temp Option")] private TileObject _tileObject;

    private IsometricGrid<TileObject> _tileGrid;
    private IsometricGrid<GridObject>[] _objectGridArray;

    private void Awake() {
        InitializeGrid();
    }

    private void InitializeGrid() {
        _tileGrid = new IsometricGrid<TileObject>(_width, _height, _gridOrigin, _ceilSize);
        _objectGridArray = new IsometricGrid<GridObject>[3];
        _objectGridArray[(int)TeamColor.NONE] 
            = new IsometricGrid<GridObject>(_width, _height, _gridOrigin, _ceilSize);
        _objectGridArray[(int)TeamColor.BLUE] 
            = new IsometricGrid<GridObject>(_width, _height, _gridOrigin, _ceilSize);
        _objectGridArray[(int)TeamColor.RED] 
            = new IsometricGrid<GridObject>(_width, _height, _gridOrigin, _ceilSize);
    }

    private void Start() {
        for (int row = 0; row < _height; ++row) {
            for (int col = 0; col < _width; ++col) {
                Vector3 position = _tileGrid.RowcolToPointCenter(row, col);
                var tileObject = Instantiate(_tileObject, position, Quaternion.identity);
                tileObject.SetTileSprite(_tileSprite);
                _tileGrid.SetElement(row, col, tileObject);
            }
        }
    }

    public Vector3 PointToGridPosition(Vector3 position) {
        Rowcol rc = _tileGrid.PointToRowcolWithClamp((Vector2)position);
        return _tileGrid.RowcolToPointCenter(rc);
    }

    public Rowcol PointToRowcol(Vector3 position) {
        return _tileGrid.PointToRowcol(position);
    }

    public Vector3 RowcolToPoint(Rowcol rowcol) {
        return _tileGrid.RowcolToPointCenter(rowcol);
    }
 
    public bool IsValidRowcol(Rowcol rowcol) {
        return _tileGrid.IsValidRowcol(rowcol);
    }

    public void HighlightTile(Rowcol rowcol) {
        TileObject tileObj = _tileGrid.GetElement(rowcol);
        tileObj?.HighlightSelf();
    }

    public void RemoveHighlightTile(Rowcol rowcol) {
        TileObject tileObj = _tileGrid.GetElement(rowcol);
        tileObj?.RemoveHighlight();
    }

    public GridObject GetObject(TeamColor color, Rowcol rowcol) {
        return GetObjectGridByColor(color).GetElement(rowcol);
    }

    public void HighlightObject(TeamColor color, Rowcol rowcol) {
        var obj = GetObjectGridByColor(color).GetElement(rowcol);
        obj?.GetComponent<SpriteRenderer>().material.SetFloat("_ApplyAmount", 1f);
    }

    public void HighlightObjectExcept(TeamColor exceptColor, Rowcol rowcol) {
        for (int colorInt = (int)TeamColor.NONE; colorInt < (int)TeamColor.COUNT; ++colorInt) {
            if ((TeamColor)colorInt == exceptColor)
                continue;
            HighlightObject((TeamColor)colorInt, rowcol);
        }
    }

    public void RemoveHighlightObject(TeamColor color, Rowcol rowcol) {
        var obj = GetObjectGridByColor(color).GetElement(rowcol);
        obj?.GetComponent<SpriteRenderer>().material.SetFloat("_ApplyAmount", 0f);
    }

    public void RemoveHighlightObjectExcept(TeamColor exceptColor, Rowcol rowcol) {
        for (int colorInt = (int)TeamColor.NONE; colorInt < (int)TeamColor.COUNT; ++colorInt) {
            if ((TeamColor)colorInt == exceptColor)
                continue;
            RemoveHighlightObject((TeamColor)colorInt, rowcol);
        }
    }

    public void RemoveAllHighlights(TeamColor color) {
        for (int row = 0; row < _height; ++row) {
            for (int col = 0; col < _width; ++col) {
                _tileGrid.GetElement(row, col)?.RemoveHighlight();
                GetObjectGridByColor(color).GetElement(row, col)?.GetComponent<SpriteRenderer>().material.SetFloat("_ApplyAmount", 0f);
            }
        }
    }

    public void RemoveAllHighlightsExcept(TeamColor exceptColor) {
        for (int colorInt = (int)TeamColor.NONE; colorInt < (int)TeamColor.COUNT; ++colorInt) {
            if ((TeamColor)colorInt == exceptColor)
                continue;
            RemoveAllHighlights((TeamColor)colorInt);
        }
    }

    // 수정 필요: 오브젝트가 둘 이상 있을 때
    public void OnObjectMoved(TeamColor color, GridObject obj, Rowcol from, Rowcol to) {
        GetObjectGridByColor(color).SetElement(to, obj);
        GetObjectGridByColor(color).SetElement(from, null);
    }

    private IsometricGrid<GridObject> GetObjectGridByColor(TeamColor color) {
        int colorInt = (int)color;
        return _objectGridArray[colorInt];
    }

    private void OnDrawGizmos() {
        if (_tileGrid == null) {
            _tileGrid = new IsometricGrid<TileObject>(_width, _height, _gridOrigin, _ceilSize);
        }

        Color prevGizmoColor = Gizmos.color;
        Gizmos.color = Color.green;

        for (int row = 0; row < _height; ++row) {
            for (int col = 0; col < _width; ++col) {
                Vector3 p00 = _tileGrid.RowcolToPoint(row, col);
                Vector3 p01 = _tileGrid.RowcolToPoint(row, col + 1);
                Vector3 p10 = _tileGrid.RowcolToPoint(row + 1, col);
                Vector3 p11 = _tileGrid.RowcolToPoint(row + 1, col + 1);

                Gizmos.DrawLine(p00, p01);
                Gizmos.DrawLine(p01, p11);
                Gizmos.DrawLine(p11, p10);
                Gizmos.DrawLine(p10, p00);                
            }
        }

        Gizmos.color = prevGizmoColor;
    }
}
