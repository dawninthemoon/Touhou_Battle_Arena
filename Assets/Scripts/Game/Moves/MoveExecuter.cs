using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moves;
using System.Reflection;
using System.Linq;

public class MoveExecuter : MonoBehaviour {
    private Dictionary<string, MoveInfo> _moveInfoDictionary;
    private Dictionary<string, MoveBase> _moveInstanceDictionary;

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
    }

    public void Execute(string moveID, int areaIndex) {
        if (!_moveInstanceDictionary.TryGetValue(moveID, out MoveBase instance)) {
            Debug.LogError("Move not exists");
        }
        instance.Execute(areaIndex);
    }
}
