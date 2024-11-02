using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect {
    private string name;
    private bool isBuff;
    private Attribute attribute;
    private float amount;
    private float tickRate;
    private float duration;
    private float timerTotal;
    private float timerTick;

    public string Name { get { return name; } }
    public bool IsBuff { get { return isBuff; } }
    public Attribute Attribute { get { return attribute; } }
    public float Amount { get { return amount; } }
    public float TickRate { get { return tickRate; } }
    public float Duration { get { return duration; } }
    public float CurrentTotalTimer { get { return timerTotal; } }
    public float CurrentTickTimer { get { return timerTick; } }

    public Effect(string name, bool isBuff, Attribute attribute, float amount, float tickRate, float duration) {
        this.name = name;
        this.isBuff = isBuff;
        this.attribute = attribute;
        this.amount = amount;
        this.tickRate = tickRate;
        this.duration = duration;
        timerTotal = duration;
        timerTick = 0.0f;
    }

    public bool Increment(float increment) {
        timerTotal = Mathf.Clamp(timerTotal - increment, 0.0f, timerTotal);
        timerTick += increment;
        return IsActive();
    }

    public bool Tick() {
        if(timerTick >= tickRate) {
            timerTick = 0.0f;
            return true;
        }

        return false;
    }

    public bool IsActive() {
        return timerTotal > 0.0f;
    }

    public void Reset() {
        timerTotal = duration;
    }
}
