using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect {
    private string name;
    private bool isBuff;
    private PassiveAttribute attribute;
    private float amount;
    private float duration;
    private float timer;

    public string Name { get { return name; } }
    public bool IsBuff { get { return isBuff; } }
    public PassiveAttribute Attribute { get { return attribute; } }
    public float Amount { get { return amount; } }
    public float Duration { get { return duration; } }
    public float CurrentTimer { get { return timer; } }

    public Effect(string name, bool isBuff, PassiveAttribute attribute, float amount, float duration) {
        this.name = name;
        this.isBuff = isBuff;
        this.attribute = attribute;
        this.amount = amount;
        this.duration = duration;
        timer = duration;
    }

    public bool Increment(float increment) {
        timer = Mathf.Clamp(timer - increment, 0.0f, timer);
        return IsActive();
    }

    public bool IsActive() {
        return timer > 0.0f;
    }

    public void Reset() {
        timer = duration;
    }
}
