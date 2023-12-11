using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Test {
    public class DisposableEffect : MonoBehaviour {
        private UnityAction _onDestroy;
        public void Initialize(UnityAction onDestroy) {
            _onDestroy = onDestroy;
        }
        
        private void DestroySelf() {
            _onDestroy?.Invoke();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
