using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RieslingUtils;

namespace Test {
    public class BezierTest : MonoBehaviour {
        private Vector2[] _points = new Vector2[4];
        private Animator _animator;
        [SerializeField] private GameObject _effect;

        [SerializeField] private float _t = 0f;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _posA = 5.5f;
        [SerializeField] private float _posB = 4.5f;

        public void Initialize(GameObject master, GameObject target) {
            _points[0] = master.transform.position;
            _points[1] = SetBezierPoint(master.transform.position);
            _points[2] = SetBezierPoint(target.transform.position);
            _points[3] = target.transform.position;
        }

        private void Update() {
            if (_t > 1f) {
                Instantiate(_effect, transform.position, Quaternion.identity).SetActive(true);
                gameObject.SetActive(false);
                return;
            }
            _t += Time.deltaTime * _speed;
            DrawTrajectory();
        }

        private Vector2 SetBezierPoint(Vector2 origin) {
            Vector2 pos;
            pos.x = _posA * Mathf.Cos(Random.Range(0, 2f * Mathf.PI)) + origin.x;
            pos.y = _posB * Mathf.Sin(Random.Range(0, 2f * Mathf.PI)) + origin.y;
            return pos;
        }

        private void DrawTrajectory() {
            //Vector2 pos = Bezier.GetPoint(_points[0], _points[1], _points[2], _points[3], _t);
            float x = GetPoint(_points[0].x, _points[1].x, _points[2].x, _points[3].x, _t);
            float y = GetPoint(_points[0].y, _points[1].y, _points[2].y, _points[3].y, _t);
            Vector2 pos = new Vector2(x, y);
            transform.position = pos;
        }

        private float GetPoint(float a, float b, float c, float d, float t) {
            float oneMinusT = Mathf.Clamp01(1f - t);
            return Mathf.Pow(oneMinusT, 3f) * a
                    + Mathf.Pow(oneMinusT, 2f) * 3f * t * b
                    + Mathf.Pow(t, 2f) * 3f * oneMinusT * c
                    + Mathf.Pow(t, 3f) * d;
        }
    }
}
