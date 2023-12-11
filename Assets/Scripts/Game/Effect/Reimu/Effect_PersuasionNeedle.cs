using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

[CreateAssetMenu(fileName = "Effect_PersuasionNeedle", menuName = "Effects/Reimu/PersuasionNeedle")]
public class Effect_PersuasionNeedle : EffectExecuter {
    private struct NeedleConfig {
        public GameObject obj;
        public Vector3 start;
        public EffectTarget target;
        public float speed;
    }

    [SerializeField] private GameObject _needleObject;
    [SerializeField] private GameObject _boomEffect;
    [SerializeField] private float _needleSpeed;
    private Quaternion[] _needleRotations;

    public override void Initialize() {
        _needleRotations = new Quaternion[8];
        for (int i = 0; i < 8; ++i) {
            float startDegree = 135f;
            float interval = -45f;
            _needleRotations[i] = Quaternion.Euler(0f, 0f, startDegree + i * interval);
        }
    }

    public override async UniTask Execute(PlayerCharacter caster, EffectConfig effectConfig, SharedData sharedData) {
        Vector3 origin = caster.transform.position;
        NeedleConfig config = new NeedleConfig() {
            obj = Instantiate(_needleObject, origin, _needleRotations[effectConfig.AreaIndex]),
            start = origin,
            target = effectConfig.Targets[0],
            speed = _needleSpeed
        };
        
        float t = 0f;
        while (!config.obj.transform.position.Equals(config.target.pos)) {
            MoveLerpBySpeed(config.obj.transform, config.target.pos, _needleSpeed);

            await UniTask.Yield();

            t += Time.deltaTime * _needleSpeed;
        }

        if (config.target.obj) {
            config.target.obj.OnCharacterHit();
            await UniTask.Delay(TimeSpan.FromSeconds(0.1));
        }

        Instantiate(_boomEffect, config.obj.transform.position, Quaternion.identity).SetActive(true);
        config.obj.SetActive(false);
        Destroy(config.obj);
    }

    private void MoveLerpBySpeed(Transform t, Vector3 target, float speed) {
        float dist = Vector3.Distance(t.position, target);
        float finalSpeed = dist / speed;
        t.position = Vector3.Lerp(t.position, target, Time.deltaTime / finalSpeed);
    }
}
