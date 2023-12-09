using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test {
    public class MissileTemp : MonoBehaviour {
        private void DestroySelf() {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
