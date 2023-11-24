using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillButton : MonoBehaviour {
    private Button _button;
    private Image _image;
    
    private void Awake() {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
    }

    public void SetInteraction(bool interactable) {
        _button.interactable = interactable;
    }
    
    public void AddListener(UnityAction action) {
        _button.onClick.AddListener(action);
    }

    public void SetSprite(Sprite sprite) {
        _image.sprite = sprite;
    }
}
