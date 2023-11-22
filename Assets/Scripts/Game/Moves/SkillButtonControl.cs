using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Moves;
using Cysharp.Threading.Tasks;
using System.Linq;

public class SkillButtonControl : MonoBehaviour, ILoadable {
    [SerializeField] private MoveDataContainer _moveDataContainer;
    [SerializeField] private MoveExecuter _executer;
    [SerializeField] private MoveSelector _selector;
    [SerializeField] private SkillButton[] _skillButtons;
    private static readonly string SkillIconLabel = "SkillIcons";
    private Dictionary<string, Sprite> _skillIconDictionary;

    public bool IsLoadCompleted {
        get;
        private set;
    }

    private void Awake() {
        AssetLoader.Instance.LoadAssetsAsync<Sprite>(SkillIconLabel, (op) => {
            _skillIconDictionary = op.Result.ToDictionary(x => x.name.Replace("SkillIcon_", ""), s => s);
            IsLoadCompleted = true;
        });
    }

    private async UniTaskVoid Start() {
        await UniTask.WaitUntil(() => _moveDataContainer.IsLoadCompleted && this.IsLoadCompleted);

        Initialize(_moveDataContainer.GetMoveInstancesByCharacter("Reimu"));
    }

    public void Initialize(MoveBase[] skillInstances) {
        foreach (MoveBase instance in skillInstances) {
            int buttonIndex = instance.Info.buttonIndex;
            _skillButtons[buttonIndex].AddListener(() => OnButtonClicked(instance.Info.moveID).Forget());
            _skillButtons[buttonIndex].GetComponent<Image>().sprite = _skillIconDictionary[instance.Info.moveID];
        }
    }
    
    private async UniTaskVoid OnButtonClicked(string moveID) {
        MoveBase instance = _moveDataContainer.GetMoveInstance(moveID);
        bool isRelative = instance.Info.isRelativeForCharacter;

        int areaIndex = await _selector.SelectExecutionArea(instance.GetExecutionArea(), isRelative);
    }
}
