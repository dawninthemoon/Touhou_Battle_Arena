using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class EffectExecuter : ScriptableObject {
    public virtual void Initialize() { }
    public abstract UniTask Execute(PlayerCharacter caster, EffectConfig effectConfig, SharedData sharedData);
}
