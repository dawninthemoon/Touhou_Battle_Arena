using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterUIControl : MonoBehaviour {
    [SerializeField] private Image _myHpBar;
    [SerializeField] private Image _opponentHpBar;

    private void Start() {

    }

    public void OnReceiveDamage(CharacterTest character) {
        Image hpBar = IsMyCharacter(character) ? _myHpBar : _opponentHpBar;
        hpBar.fillAmount = (float)character.Health / character.MaxHealth;
    }

    private bool IsMyCharacter(CharacterTest character) {
        return character.Color == PlayerMoveReceiver.MyColor;
    }
}