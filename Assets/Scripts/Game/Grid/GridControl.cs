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
    private IsometricGrid<GridObject> _objectGrid;

    private void Awake() {
        _tileGrid = new IsometricGrid<TileObject>(_width, _height, _gridOrigin, _ceilSize);
        _objectGrid = new IsometricGrid<GridObject>(_width, _height, _gridOrigin, _ceilSize);
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
