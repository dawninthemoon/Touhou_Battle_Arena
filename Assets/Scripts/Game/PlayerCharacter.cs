using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : GridObject {
    private Animator _animator;
    private SpriteRenderer _renderer;
    public Rowcol Curr {
        get;
        private set;
    }
    private static readonly string HitTriggerKey = "hit";
    private static readonly string DieTriggerKey = "die";
    private static readonly string AttackTriggerKey = "attack";


    private void Awake() {
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void MoveImmediate(Vector3 pos, Rowcol to) {
        transform.position = pos;
        Curr = to;
    }

    public override void ReceiveDamage(int amount) {
        base.ReceiveDamage(amount);
        _animator.SetTrigger(HitTriggerKey);
    }

    public void SetTrigger(string triggerName) {
         _animator.SetTrigger(triggerName);
    }

    public void OnCharacterDead() {
        SetTrigger(DieTriggerKey);
    }

    public void OnCharacterAttack() {
        SetTrigger(AttackTriggerKey);
    }
    
    public void SetFlipX(bool flipX) {
        _renderer.flipX = flipX;
    }
}
