using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour {
    private SpriteRenderer _renderer;
    private static readonly string HighlightKey = "_ApplyAmount";

    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void SetTileSprite(Sprite sprite) {
        _renderer.sprite = sprite;
    }

    public void HighlightSelf() {
        _renderer.material.SetFloat(HighlightKey, 1f);
    }

    public void RemoveHighlight() {
        _renderer.material.SetFloat(HighlightKey, 0f);
    }
}
