using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "Effect_YinYangOrbs", menuName = "Effects/Reimu/YinYangOrbs")]
public class Effect_YinYangOrbs : EffectExecuter {
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

        if (effectConfig.Targets.Count > 0) {
            if (effectConfig.Targets[0].obj) {
                effectConfig.Targets[0].obj.OnCharacterHit();
                await UniTask.Delay(System.TimeSpan.FromSeconds(0.1));
            }
        }
        
        await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f));
        

        t = 0;
        while (t < 1f) {
            _mainCamera.backgroundColor = Color.Lerp(Color.black, color, t);
            
            await UniTask.Yield();

            t += Time.deltaTime * 4f;
        }
        _mainCamera.backgroundColor = color;
    }
}
