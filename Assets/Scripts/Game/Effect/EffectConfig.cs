using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EffectTarget {
    public PlayerCharacter obj;
    public Vector3 pos;
    public EffectTarget(PlayerCharacter obj, Vector3 pos) {
        this.obj = obj;
        this.pos = pos;
    }
}

public class EffectConfig {
    public List<EffectTarget> Targets {
        get;
        private set;
    }
    public int AreaIndex {
        get;
        set;
    }

    public EffectConfig() {
        Targets = new List<EffectTarget>();
    }

    public void Add(EffectTarget target) {
        Targets.Add(target);
    }

    public void Reset() {
        AreaIndex = 0;
        Targets.Clear();
    }
}