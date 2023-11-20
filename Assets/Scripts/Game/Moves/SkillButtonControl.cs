using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;
using Cysharp.Threading.Tasks;

public class SkillButtonControl : MonoBehaviour {
    [SerializeField] private MoveExecuter _executer;
    [SerializeField] private MoveSelector _selector;
    [SerializeField] private SkillButton[] _skillButtons;

    private void Start() {
        _skillButtons[0].AddListener(() => OnButtonClicked("Move_ReimuBasicAttack").Forget());
    }

    public void Initialize() {
        // TODO: 캐릭터에 따른 스킬 초기화

        //_skillButtons[0].AddListener(() => OnButtonClicked("Move_ReimuBasicAttack"));
    }
    
    private async UniTaskVoid OnButtonClicked(string moveID) {
        MoveBase instance = _executer.GetMoveInstance(moveID);
        bool isRelative = instance.Info.isRelativeForCharacter;

        int areaIndex = await _selector.SelectExecutionArea(instance.GetExecutionArea(), isRelative);
    }
}
