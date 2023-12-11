using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EffectControl : MonoBehaviour {
    [SerializeField, Tooltip("Temp Option")]
    private Effect_HomingAmulet _temp;

    private void Awake() {
        _temp.Initialize();
    }

    public async UniTask StartExecuteEffect(PlayerCharacter caster, List<EffectTarget> targets, SharedData sharedData) {
        await _temp.Execute(caster, targets, sharedData);
    }
}
