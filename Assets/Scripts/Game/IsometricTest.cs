using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricTest : MonoBehaviour {
    [SerializeField] private int _width, _height;
    [SerializeField] private float _ceilSize;
    private IsometricGrid<GameObject> _grid;

    private void Awake() {
        _grid = new IsometricGrid<GameObject>(_width, _height, Vector3.zero, _ceilSize);
        MoveDataParser parser = new MoveDataParser();
        AssetLoader.Instance.LoadAssetAsync<TextAsset>("MoveInfo", (op) => {
            parser.Parse(op.Result.ToString());
        });
    }

    private void Start() {
    }

    private void OnDrawGizmos() {
        if (_grid == null) {
            return;
        }

        Color prevGizmoColor = Gizmos.color;
        Gizmos.color = Color.green;

        for (int row = 0; row < _height; ++row) {
            for (int col = 0; col < _width; ++col) {
                Vector3 p00 = _grid.RowcolToPoint(row, col);
                Vector3 p01 = _grid.RowcolToPoint(row, col + 1);
                Vector3 p10 = _grid.RowcolToPoint(row + 1, col);
                Vector3 p11 = _grid.RowcolToPoint(row + 1, col + 1);

                Gizmos.DrawLine(p00, p01);
                Gizmos.DrawLine(p01, p11);
                Gizmos.DrawLine(p11, p10);
                Gizmos.DrawLine(p10, p00);                
            }
        }

        Gizmos.color = prevGizmoColor;
    }
}
