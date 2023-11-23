using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Moves;
using Cysharp.Threading.Tasks;

public class MoveButtonControl : MonoBehaviour, ILoadable {
    [SerializeField] private MoveDataContainer _moveDataContainer;
    [SerializeField] private MoveSlot _moveSlot;
    [SerializeField] private MoveAreaSelector _selector;
    [SerializeField] private SkillButton[] _skillButtons;
    [SerializeField] private Image[] _slotImages;
    [SerializeField] private Button _upArrow, _downArrow, _rightArrow, _leftArrow;
    private Dictionary<string, Sprite> _skillIconDictionary;
    private static readonly string SkillIconLabel = "SkillIcons";
    public bool IsLoadCompleted {
        get;
        private set;
    }

    private void Awake() {
        AssetLoader.Instance.LoadAssetsAsync<Sprite>(SkillIconLabel, (op) => {
            _skillIconDictionary = op.Result.ToDictionary(x => x.name.Replace("SkillIcon_", ""), s => s);
            IsLoadCompleted = true;
        });

        _upArrow.onClick.AddListener(() => MovementSelected(0));
        _rightArrow.onClick.AddListener(() => MovementSelected(1));
        _downArrow.onClick.AddListener(() => MovementSelected(2));
        _leftArrow.onClick.AddListener(() => MovementSelected(3));
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

    public void RemoveSlotImages() {
        for (int i = 0; i < MoveSlot.MaxSlots; ++i) {
            _slotImages[i].sprite = null;
        }
    }
    
    private async UniTaskVoid OnButtonClicked(string moveID) {
        int slotIndex = await _moveSlot.RequestExecuteAsync(moveID);
        if (slotIndex != -1) {
            _slotImages[slotIndex].sprite = _skillIconDictionary[moveID];
        }
    }

    private void MovementSelected(int areaIndex) {
        _moveSlot.RequestExecuteMovement(Move_Movement.MoveID, areaIndex);
    }
}
