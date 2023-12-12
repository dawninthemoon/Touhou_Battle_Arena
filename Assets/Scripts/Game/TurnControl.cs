using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnControl : MonoBehaviour {
    [SerializeField] private MoveButtonControl _moveButtonControl;
    private int _currentTurn;
    public UnityEvent TurnEndEvent {
        get;
        private set;
    }

    private void Awake() {
        _currentTurn = 1;
        TurnEndEvent = new UnityEvent();
        TurnEndEvent.AddListener(OnTurnEnd);
    }

    private void OnTurnEnd() {
        ++_currentTurn;
        _moveButtonControl.SetButtonInteraction(true);
    }
}
