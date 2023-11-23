using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;
    [SerializeField, Tooltip("Temp Option")] private CharacterTest _reimuPrefab;
    [SerializeField, Tooltip("Temp Option")] private CharacterTest _marisaPrefab;
    private CharacterTest _myCharacter;
    private CharacterTest _opponentCharacter;
    public Rowcol MyCharacterRowcol { get { return _myCharacter.Curr; } }

    private void Awake() {
        _myCharacter = Instantiate(_reimuPrefab);
        _myCharacter.gameObject.SetActive(false);

        _opponentCharacter = Instantiate(_reimuPrefab);
        _opponentCharacter.GetComponent<SpriteRenderer>().color = Color.red;
        _opponentCharacter.gameObject.SetActive(false);
    }

    public void PlaceCharacter(TeamColor player, Rowcol rowcol) {
        Vector3 position = _gridControl.RowcolToPoint(rowcol);
        position.y += 12f;

        var character = GetCharacterByColor(player);
        
        _gridControl.OnObjectMoved(character.gameObject, character.Curr, rowcol);
        character.MoveImmediate(position, rowcol);
        character.gameObject.SetActive(true);
    }

    public void MoveCharacter(TeamColor player, Rowcol rowcol) {
        var character = GetCharacterByColor(player);

        Rowcol target = character.Curr + rowcol;
        Vector3 position = _gridControl.RowcolToPoint(target);
        position.y += 12f;

        
        _gridControl.OnObjectMoved(character.gameObject, character.Curr, target);
        character.MoveImmediate(position, target);
        character.gameObject.SetActive(true);
    }

    // 후에 네트워크 있으면 수정 필요
    public CharacterTest GetCharacterByColor(TeamColor color) {
        return (color == PlayerMoveReceiver.MyColor) ? _myCharacter : _opponentCharacter;
    }
}
