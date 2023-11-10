using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerTest : NetworkBehaviour {
    [SerializeField] private float _moveSpeed;

    private void Update() {
        if (this.isLocalPlayer) {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector3 moveDir = new Vector3(x, y);

            transform.position += moveDir * _moveSpeed * Time.deltaTime;
        }
    }
}
