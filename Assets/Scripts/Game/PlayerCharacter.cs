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
    public static readonly int MaxEnergy = 100;
    public int Energy {
        get;
        set;
    }

    private void Awake() {
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    public override void Initialize(TeamColor color, int initialHeatlh, UnityAction onReceiveDamage) {
        base.Initialize(color, initialHeatlh, onReceiveDamage);
        Energy = MaxEnergy;
    }

    public void MoveImmediate(Vector3 pos, Rowcol to) {
        transform.position = pos;
        Curr = to;
    }

    public override void ReceiveDamage(int amount) {
        base.ReceiveDamage(amount);
        _animator.SetTrigger(HitTriggerKey);
    }

    public void SetAnimationTrigger(string triggerName) {
         _animator.SetTrigger(triggerName);
    }

    public void OnCharacterDead() {
        SetAnimationTrigger(DieTriggerKey);
    }
    
    public void SetFlipX(bool flipX) {
        _renderer.flipX = flipX;
    }
}
