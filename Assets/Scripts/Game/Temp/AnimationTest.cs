using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace Test {
    public class AnimationTest : MonoBehaviour {
        [SerializeField] private BezierTest _missle;
        [SerializeField] private GameObject[] _targets;

        [SerializeField] private int _shot = 5;
        [SerializeField] private double _delay = 1.0;
        [SerializeField] private double _shotDelay = 0.1;

        private async UniTaskVoid Start() {
            while (gameObject.activeSelf) {
                for (int i = 0; i < _shot; ++i) {
                    BezierTest missile = Instantiate(_missle, transform.position, Quaternion.identity);
                    missile.Initialize(gameObject, _targets[i]);
                    missile.gameObject.SetActive(true);

                    await UniTask.Delay(TimeSpan.FromSeconds(_shotDelay));
                }
                await UniTask.Delay(TimeSpan.FromSeconds(_delay));
            }
        }
    }
}