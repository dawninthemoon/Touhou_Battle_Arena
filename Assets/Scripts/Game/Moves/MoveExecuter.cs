using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;
using System.Reflection;
using System.Linq;

public class MoveExecuter : MonoBehaviour {
    [SerializeField] private GridControl _gridControl;
    [SerializeField] private CharacterControl _characterControl;

    private Dictionary<string, MoveInfo> _moveInfoDictionary;
    private Dictionary<string, MoveBase> _moveInstanceDictionary;

    private SharedData _sharedData;

    private void Awake() {
        MoveDataParser parser = new MoveDataParser();
        AssetLoader.Instance.LoadAssetAsync<TextAsset>("MoveInfo", (op) => {
            MoveInfo[] moveInfoArray = parser.Parse(op.Result.ToString());
            _moveInfoDictionary = moveInfoArray.ToDictionary(x => x.moveID);

            System.Type moveType = typeof(MoveBase);
            var moveTypes = Assembly.GetAssembly(moveType).GetTypes();
            _moveInstanceDictionary 
                = moveTypes
                    .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(moveType))
                    .ToDictionary(type => type.Name, 
                                  type => System.Activator.CreateInstance(type, _moveInfoDictionary[type.Name]) as MoveBase);
        });

        _sharedData = new SharedData(_gridControl, _characterControl);
    }

    public void Execute(string moveID, int areaIndex) {
        TeamColor player = TeamColor.RED;
        MoveBase instance = GetMoveInstance(moveID);
        instance.Execute(player, areaIndex, _sharedData);
    }

    public MoveBase GetMoveInstance(string moveID) {
        if (!_moveInstanceDictionary.TryGetValue(moveID, out MoveBase instance)) {
            Debug.LogError("Move not exists");
        }
        return instance;
    }
}
