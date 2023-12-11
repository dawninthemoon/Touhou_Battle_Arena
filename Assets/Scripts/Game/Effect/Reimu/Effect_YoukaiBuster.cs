using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Test;

[CreateAssetMenu(fileName = "Effect_YoukaiBuster", menuName = "Effects/Reimu/YoukaiBuster")]
public class Effect_YoukaiBuster : EffectExecuter {
    [SerializeField] private DisposableEffect _slashEffect;
    private Quaternion[] _effectRotations;

    public override void Initialize() {
        _effectRotations = new Quaternion[] {
            Quaternion.Euler(0f, 0f, 90f),
            Quaternion.Euler(0f, 0f, 0f),
            Quaternion.Euler(0f, 0f, -90f),
            Quaternion.Euler(0f, 0f, 180f),
        };
    }

    public override async UniTask Execute(PlayerCharacter caster, EffectConfig effectConfig, SharedData sharedData) {
        Vector3 center = effectConfig.Targets[0].pos;
        bool isSlashEnd = false;

        var effect = Instantiate(_slashEffect, center, _effectRotations[effectConfig.AreaIndex]);
        effect.Initialize(() => isSlashEnd = true);

        await UniTask.WaitUntil(() => isSlashEnd);
        
        double waitTime = 0.0;
        for (int i = 0; i < effectConfig.Targets.Count; ++i) {
            if (effectConfig.Targets[0].obj) {
                effectConfig.Targets[0].obj.OnCharacterHit();
                waitTime = 0.1;
            }
        }
        await UniTask.Delay(System.TimeSpan.FromSeconds(waitTime));

        isSlashEnd = false;
    }
}
