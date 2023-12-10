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
            Vector2 pos = Bezier.GetPoint(_points[0], _points[1], _points[2], _points[3], _t);
            Vector3 derivative = Bezier.GetFirstDerivative(_points[0], _points[1], _points[2], _points[3], _t);
            float radian = Mathf.Atan2(derivative.y, derivative.x);
            transform.rotation = Quaternion.Euler(0f, 0f, radian * Mathf.Rad2Deg);
            transform.position = pos;
        }
    }
}
