using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedData {
    public GridControl GridCtrl { get; private set; }
    public CharacterControl CharcaterCtrl { get; private set; }
    public EffectControl EffectCtrl { get; private set; }

    public SharedData(GridControl gridControl, CharacterControl characterControl, EffectControl effectControl) {
        GridCtrl = gridControl;
        CharcaterCtrl = characterControl;
        EffectCtrl = effectControl;
    }
}
