using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;
using System.Reflection;
using System.Linq;

public class MoveExecuter : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;
    [SerializeField] private CharacterControl _characterControl;

    private SharedData _sharedData;

    private void Awake() {
        _sharedData = new SharedData(_gridControl, _characterControl);
    }

    public void Execute(MoveBase instance, int areaIndex) {
        TeamColor player = TeamColor.RED;
        instance.Execute(player, areaIndex, _sharedData);
    }
}
