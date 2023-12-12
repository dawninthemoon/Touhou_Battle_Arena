using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnControl : MonoBehaviour {
    private int _currentTurn;
    public UnityEvent TurnEndEvent {
        get;
        private set;
    }

    private void Awake() {
        InitializeData();
    }

    private void InitializeData() {
        TurnEndEvent = new UnityEvent();
        _currentTurn = 1;
    }

    public void FinishTurn() {
        TurnEndEvent.Invoke();
    }
}
