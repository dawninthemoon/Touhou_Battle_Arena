using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectBase : ScriptableObject {
    public abstract void Execute(GridObject[] targets, SharedData sharedData);
}
