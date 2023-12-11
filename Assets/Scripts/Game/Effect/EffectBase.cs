using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public struct EffectTarget {
    public PlayerCharacter obj;
    public Vector3 pos;
    public EffectTarget(PlayerCharacter obj, Vector3 pos) {
        this.obj = obj;
        this.pos = pos;
    }
}

public abstract class EffectBase : ScriptableObject {
    public abstract UniTask Execute(PlayerCharacter caster, List<EffectTarget> targets, SharedData sharedData);
}
