using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterUIControl : MonoBehaviour {
    [SerializeField] private Image _myHpBar, _myEnergyBar;
    [SerializeField] private Image _opponentHpBar, _opponentEnergyBar;

    private void Start() {

    }

    public void OnReceiveDamage(PlayerCharacter character) {
        Image hpBar = IsMyCharacter(character) ? _myHpBar : _opponentHpBar;
        hpBar.fillAmount = (float)character.Health / character.MaxHealth;
    }

    public void OnEnergyChanged(PlayerCharacter character) {
        Image energyBar = IsMyCharacter(character) ? _myEnergyBar : _opponentEnergyBar;
        energyBar.fillAmount = (float)character.Energy / PlayerCharacter.MaxEnergy;
    }

    private bool IsMyCharacter(PlayerCharacter character) {
        return character.Color == PlayerMoveReceiver.MyColor;
    }
}
