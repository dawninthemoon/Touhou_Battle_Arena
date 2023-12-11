using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Moves;
using RieslingUtils;

[CreateAssetMenu(fileName = "Effect_HomingAmulet", menuName = "Effects/Reimu/HomingAmulet")]
public class Effect_HomingAmulet : EffectExecuter {
    private struct AmuletConfig {
        public GameObject obj;
        public Vector3 start;
        public Vector3 controlPoint1;
        public Vector3 controlPoint2;
        public EffectTarget target;
        public float speed;
    }

    [SerializeField] private GameObject _amuletPrefab;
    [SerializeField] private GameObject _boomEffect;
    [SerializeField] private Vector2 _bezierInterval;
    [SerializeField] private float _shotDelay;
    [SerializeField] private float _amuletSpeed;
    private AmuletConfig[] _amulets;
    private int _numOfAmulets;

    public override void Initialize() {
        _amulets = new AmuletConfig[Move_HomingAmulet.NumOfAmulets];
    }

    public override async UniTask Execute(PlayerCharacter caster, EffectConfig effectConfig, SharedData sharedData) {
        Vector3 origin = caster.transform.position;
        _numOfAmulets = effectConfig.Targets.Count;
        for (int i = 0; i < _numOfAmulets; ++i) {
            GameObject amuletObj = Instantiate(_amuletPrefab, origin, Quaternion.identity);

            AmuletConfig config = new AmuletConfig() {
                obj = amuletObj,
                start = origin,
                target = effectConfig.Targets[i],
                controlPoint1 = SetBezierControlPoint(origin),
                controlPoint2 = SetBezierControlPoint(effectConfig.Targets[i].pos),
                speed = _amuletSpeed
            };
            _amulets[i] = config;
        }

        float elpasedTime = 0f;
        float targetTime = 1f + _shotDelay * _numOfAmulets;
        while (elpasedTime < targetTime) {
            Progress(elpasedTime);
            await UniTask.Yield();
            elpasedTime += Time.deltaTime * _amuletSpeed;
        }
    }

    private void Progress(float elapsedTime) {
        for (int i = 0; i < _numOfAmulets; ++i) {
            float t = elapsedTime - i * _shotDelay;
            DrawTrajectory(_amulets[i], t);
        }
    }

    private Vector2 SetBezierControlPoint(Vector2 origin) {
        Vector2 pos;
        pos.x = _bezierInterval.x * Mathf.Cos(Random.Range(0, 2f * Mathf.PI)) + origin.x;
        pos.y = _bezierInterval.y * Mathf.Sin(Random.Range(0, 2f * Mathf.PI)) + origin.y;
        return pos;
    }

    private void DrawTrajectory(AmuletConfig config, float t) {
        if (t > 1f) {
            config.obj.SetActive(false);
            config.target.obj?.OnCharacterHit();
            Instantiate(_boomEffect, config.obj.transform.position, Quaternion.identity).SetActive(true);
        }
        Vector2 p1 = config.start;
        Vector2 p2 = config.controlPoint1;
        Vector2 p3 = config.controlPoint2;
        Vector2 p4 = config.target.pos;

        Vector2 pos = Bezier.GetPoint(p1, p2, p3, p4, t);
        Vector3 derivative = Bezier.GetFirstDerivative(p1, p2, p3, p4, t);
        float radian = Mathf.Atan2(derivative.y, derivative.x);

        config.obj.transform.rotation = Quaternion.Euler(0f, 0f, radian * Mathf.Rad2Deg);
        config.obj.transform.position = pos;
    }
}
