using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using Moves;

public class MoveDataContainer : MonoBehaviour, ILoadable {
    private Dictionary<string, MoveBase> _moveInstanceDictionary;
    public bool IsLoadCompleted {
        get;
        private set;
    }

    private void Awake() {
        MoveDataParser parser = new MoveDataParser();
        AssetLoader.Instance.LoadAssetAsync<TextAsset>("MoveInfo", (op) => {
            MoveInfo[] moveInfoArray = parser.Parse(op.Result.ToString());
            var moveInfoDictionary = moveInfoArray.ToDictionary(x => x.moveID);

            System.Type moveType = typeof(MoveBase);
            var moveTypes = Assembly.GetAssembly(moveType).GetTypes();
            _moveInstanceDictionary 
                = moveTypes
                    .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(moveType))
                    .ToDictionary(type => type.Name, 
                                  type => System.Activator.CreateInstance(type, moveInfoDictionary[type.Name]) 
                                  as MoveBase);

            IsLoadCompleted = true;
        });
    }

    public MoveBase[] GetMoveInstancesByCharacter(string characterKey) {
        MoveBase[] moveInstances = _moveInstanceDictionary
                                    .Select(pair => pair.Value)
                                    .Where(x => (x.Info.characterKey != null) && x.Info.characterKey.Equals(characterKey))
                                    .ToArray();
        return moveInstances;
    }

    public MoveBase GetMoveInstance(string moveID) {
        if (!_moveInstanceDictionary.TryGetValue(moveID, out MoveBase instance)) {
            Debug.LogError("Move not exists");
            return null;
        }
        return instance;
    }
}
