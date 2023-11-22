using System.Collections;
using System.Collections.Generic;
using Moves;
using UnityEngine;
using UnityEngine.UI;

public class MovementButtonControl : MonoBehaviour {
    [SerializeField] private MoveExecuter _executer;
    [SerializeField] private Button _upArrow, _downArrow, _rightArrow, _leftArrow;
    private static readonly string MovementMoveID = "Move_Movement";

    private void Awake() {
        _upArrow.onClick.AddListener(() => MovementSelected(0));
        _rightArrow.onClick.AddListener(() => MovementSelected(1));
        _downArrow.onClick.AddListener(() => MovementSelected(2));
        _leftArrow.onClick.AddListener(() => MovementSelected(3));
    }

    private void MovementSelected(int areaIndex) {
        _executer.RequestExecute(MovementMoveID, areaIndex);
    }
}
