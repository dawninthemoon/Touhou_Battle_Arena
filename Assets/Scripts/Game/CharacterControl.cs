using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;
    [SerializeField, Tooltip("Temp Option")] private GameObject _reimu;

    private void Awake() {
        
    }

    private void Start() {
        _reimu = Instantiate(_reimu);
        _reimu.gameObject.SetActive(false);
    }
    
    public void PlaceMyCharacter(Rowcol rowcol) {
        Vector3 position = _gridControl.RowcolToPoint(rowcol);
        position.y += 12f;
        _reimu.transform.position = position;
        _reimu.gameObject.SetActive(true);
    }
}
