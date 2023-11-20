using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour {
    [SerializeField] private int _width, _height;
    [SerializeField] private float _ceilSize;
    [SerializeField, Tooltip("Temp Option")] private Sprite _tileSprite;
    [SerializeField, Tooltip("Temp Option")] private TileObject _tileObject;

    private IsometricGrid<TileObject> _tileGrid;
    private IsometricGrid<GridObject> _objectGrid;

    private void Awake() {
        _tileGrid = new IsometricGrid<TileObject>(_width, _height, Vector3.zero, _ceilSize);
        _objectGrid = new IsometricGrid<GridObject>(_width, _height, Vector3.zero, _ceilSize);
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
}
