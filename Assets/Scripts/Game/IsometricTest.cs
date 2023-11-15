using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricTest : MonoBehaviour {
    [SerializeField] private int _width, _height;
    [SerializeField] private float _ceilSize;
    [SerializeField] private Sprite _tileSprite;
    private IsometricGrid<GameObject> _tileGrid;

    private void Awake() {
        _tileGrid = new IsometricGrid<GameObject>(_width, _height, Vector3.zero, _ceilSize);
        MoveDataParser parser = new MoveDataParser();
        AssetLoader.Instance.LoadAssetAsync<TextAsset>("MoveInfo", (op) => {
            parser.Parse(op.Result.ToString());
        });
    }

    private void Start() {
        for (int row = 0; row < _height; ++row) {
            for (int col = 0; col < _width; ++col) {
                SpriteRenderer sr = new GameObject().AddComponent<SpriteRenderer>();
                sr.sprite = _tileSprite;
                _tileGrid.SetElement(row, col, sr.gameObject);
                Vector3 position = _tileGrid.RowcolToPointCenter(row, col);
                sr.transform.position = position;
            }
        }
    }

    private void OnDrawGizmos() {
        if (_tileGrid == null) {
            return;
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
