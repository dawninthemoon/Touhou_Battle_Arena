using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillButton : MonoBehaviour {
    private Button _button;
    
    private void Awake() {
        _button = GetComponent<Button>();
    }
    
    public void AddListener(UnityAction action) {
        _button.onClick.AddListener(action);
    }
}
