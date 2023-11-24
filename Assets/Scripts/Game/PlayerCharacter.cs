using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : GridObject {
    private Animator _animator;
    public Rowcol Curr {
        get;
        private set;
    }
    private static readonly string HitTriggerKey = "hit";
    private static readonly string DieTriggerKey = "die";
    private static readonly string AttackTriggerKey = "attack";


    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void MoveImmediate(Vector3 pos, Rowcol to) {
        transform.position = pos;
        Curr = to;
    }

    public override void ReceiveDamage(int amount) {
        base.ReceiveDamage(amount);
        _animator.SetTrigger(HitTriggerKey);
    }

    public void OnCharacterDead() {
        _animator.SetTrigger(DieTriggerKey);
    }

    public void OnCharacterAttack() {
        _animator.SetTrigger(AttackTriggerKey);
    }
}
