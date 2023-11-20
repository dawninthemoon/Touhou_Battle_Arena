using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveExecuter : MonoBehaviour {
    private void Awake() {
        MoveDataParser parser = new MoveDataParser();
        AssetLoader.Instance.LoadAssetAsync<TextAsset>("MoveInfo", (op) => {
            parser.Parse(op.Result.ToString());
        });
    }
}
