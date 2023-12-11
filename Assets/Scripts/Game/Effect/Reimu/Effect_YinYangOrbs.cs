using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Test;

[CreateAssetMenu(fileName = "Effect_YinYangOrbs", menuName = "Effects/Reimu/YinYangOrbs")]
public class Effect_YinYangOrbs : EffectExecuter {
    [SerializeField] private DisposableEffect _yinYangOrbPrefab;
    private Camera _mainCamera;
    public override void Initialize() {
        _mainCamera = Camera.main;
    }

    public override async UniTask Execute(PlayerCharacter caster, EffectConfig effectConfig, SharedData sharedData) {
        Color color = _mainCamera.backgroundColor;
        _mainCamera.backgroundColor = Color.black;

        float t = 0f;
        while (t < 1f) {
             _mainCamera.backgroundColor = Color.Lerp(color, Color.black, t);
            
            await UniTask.Yield();

            t += Time.deltaTime * 4f;
        }


        bool effectEnd = false;
        var orb = Instantiate(_yinYangOrbPrefab, effectConfig.Targets[0].pos, Quaternion.identity);
        orb.Initialize(() => effectEnd = true);

        for (int i = 0; i < effectConfig.Targets.Count; ++i) {
            if (effectConfig.Targets[i].obj != null) {
                effectConfig.Targets[i].obj.OnCharacterHit();
            }
        }

        await UniTask.WaitUntil(() => effectEnd);

        t = 0f;
        while (t < 1f) {
            _mainCamera.backgroundColor = Color.Lerp(Color.black, color, t);
            
            await UniTask.Yield();

            t += Time.deltaTime * 4f;
        }
        _mainCamera.backgroundColor = color;
    }
}
