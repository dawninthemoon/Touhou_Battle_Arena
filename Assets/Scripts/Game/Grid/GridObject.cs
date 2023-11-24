using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GridObject : MonoBehaviour {
    public int MaxHealth {
        get;
        private set;
    }
    public int Health {
        get;
        private set;
    }
    public TeamColor Color {
        get;
        private set;
    }
    private UnityAction _onReceiveDamage;

    public virtual void Initialize(TeamColor color, int initialHeatlh, UnityAction onReceiveDamage) {
        Color = color;
        Health = MaxHealth = initialHeatlh;
        _onReceiveDamage = onReceiveDamage;
    }

    public void ReceiveDamage(int amount) {
        Health = Mathf.Max(Health - amount, 0);
        _onReceiveDamage?.Invoke();
    }
}
