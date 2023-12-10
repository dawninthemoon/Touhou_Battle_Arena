using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;
    [SerializeField, Tooltip("Temp Option")] private PlayerCharacter _reimuPrefab;
    [SerializeField] private CharacterUIControl _characterUIControl;
    [SerializeField] private MoveButtonControl _moveButtonControl;
    private PlayerCharacter _myCharacter;
    private PlayerCharacter _opponentCharacter;
    public Rowcol MyCharacterRowcol { get { return _myCharacter.Curr; } }
    public Rowcol OpponentCharacterRowcol { get { return _opponentCharacter.Curr; } }
    public int MyCharacterEnergy { get { return _myCharacter.Energy; } }

    private void Awake() {
        _myCharacter = Instantiate(_reimuPrefab);
        _myCharacter.gameObject.SetActive(false);

        _opponentCharacter = Instantiate(_reimuPrefab);
        _opponentCharacter.GetComponent<SpriteRenderer>().color = Color.red;
        _opponentCharacter.gameObject.SetActive(false);
    }

    private void Start() {
        _myCharacter.Initialize(
            PlayerMoveReceiver.MyColor,
            100,
            () => {
                OnReceiveDamage(_myCharacter);
            }
        );
        _opponentCharacter.Initialize(
            PlayerMoveReceiver.OpponentColor,
            100,
            () => {
                OnReceiveDamage(_opponentCharacter);
            }
        );
    }

    private void Update() {
        if (_myCharacter.gameObject.activeSelf && _opponentCharacter.gameObject.activeSelf) {
            int myCharacterColumn = _myCharacter.Curr.column;
            int opponentCharacterColumn = _opponentCharacter.Curr.column;

            if (myCharacterColumn != opponentCharacterColumn) {
                UpdateCharactersFlip(myCharacterColumn, opponentCharacterColumn);
            }
        }
    }

    public void GainEnergy(int amount) {
        _myCharacter.Energy += amount;
        _characterUIControl.OnEnergyChanged(_myCharacter);
    }

    public void GainEnergy(int amount, TeamColor color) {
        PlayerCharacter character = GetCharacterByColor(color);
        character.Energy += amount;
        _characterUIControl.OnEnergyChanged(character);
    }

    public void UseEnergy(int cost) {
        _myCharacter.Energy -= cost;
        _characterUIControl.OnEnergyChanged(_myCharacter);
    }

    public void UseEnergy(int cost, TeamColor color) {
        PlayerCharacter character = GetCharacterByColor(color);
        character.Energy -= cost;
        _characterUIControl.OnEnergyChanged(character);
    }

    public void PlaceCharacter(TeamColor player, Rowcol rowcol) {
        Vector3 position = _gridControl.RowcolToPoint(rowcol);
        position.y += 12f;

        var character = GetCharacterByColor(player);
        
        _gridControl.OnObjectMoved(player, character, character.Curr, rowcol);
        character.MoveImmediate(position, rowcol);
        character.gameObject.SetActive(true);

        if (_myCharacter.gameObject.activeSelf && _opponentCharacter.gameObject.activeSelf) {
            UpdateCharactersFlip(_myCharacter.Curr.column, _myCharacter.Curr.column);
        }
    }

    public void MoveCharacter(TeamColor player, Rowcol rowcol) {
        var character = GetCharacterByColor(player);

        Rowcol target = character.Curr + rowcol;
        Vector3 position = _gridControl.RowcolToPoint(target);
        position.y += 12f;
        
        _gridControl.OnObjectMoved(player, character, character.Curr, target);
        character.MoveImmediate(position, target);
        character.gameObject.SetActive(true);
    }

    public PlayerCharacter GetCharacterByColor(TeamColor color) {
        return (color == PlayerMoveReceiver.MyColor) ? _myCharacter : _opponentCharacter;
    }

    private void OnReceiveDamage(PlayerCharacter character) {
        _characterUIControl.OnReceiveDamage(character);
        if (character.Health == 0) {
            character.OnCharacterDead();
            _moveButtonControl.SetButtonInteraction(false);
        }
    }

    private void UpdateCharactersFlip(int myCharacterColumn, int opponentCharacterColumn) {
        bool flipX = (myCharacterColumn - opponentCharacterColumn) > 0;
        _myCharacter.SetFlipX(flipX);
        _opponentCharacter.SetFlipX(!flipX);
    }
}
