using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;
    [SerializeField, Tooltip("Temp Option")] private CharacterTest _reimuPrefab;
    private CharacterTest _myCharacter;

    private void Awake() {
        
    }

    private void Start() {
        _myCharacter = Instantiate(_reimuPrefab);
        _myCharacter.gameObject.SetActive(false);
    }
    
    public void PlaceMyCharacter(Rowcol rowcol) {
        Vector3 position = _gridControl.RowcolToPoint(rowcol);
        position.y += 12f;
        _myCharacter.MoveImmediate(position, rowcol);
        _myCharacter.gameObject.SetActive(true);
    }
}
