using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EffectControl : MonoBehaviour {
    [SerializeField, Tooltip("Temp Option")]
    private Effect_HomingAmulet _temp;
    private static readonly string EffectExecuterLabel = "EffectExecuter";
    private Dictionary<string, EffectExecuter> _executerDictionary;

    private void Awake() {
        _executerDictionary = new Dictionary<string, EffectExecuter>();
        AssetLoader.Instance.LoadAssetsAsync<EffectExecuter>(EffectExecuterLabel, (op) => {
            List<EffectExecuter> effectExecuter = op.Result as List<EffectExecuter>;
            foreach (EffectExecuter executer in effectExecuter) {
                executer.Initialize();
                string typeName = executer.GetType().ToString();
                _executerDictionary.Add(typeName, executer);
            }
        });
    }

    public async UniTask StartExecuteEffect(string effectName, PlayerCharacter caster, EffectConfig targets, SharedData sharedData) {
        if (_executerDictionary.TryGetValue(effectName, out EffectExecuter executer)) {
            await executer.Execute(caster, targets, sharedData);
        }
    }
}
