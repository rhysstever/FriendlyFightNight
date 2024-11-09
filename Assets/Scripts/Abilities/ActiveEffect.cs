using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEffect : Effect {
    [SerializeField]
    private float duration;
    private float timer;

    public float Duration { set { duration = value; } }

    protected override void Awake() {
        timer = 0.0f;
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    protected virtual void FixedUpdate() {
        if(isActive) {
            timer += Time.deltaTime;

            if(timer >= duration) {
                RemoveEffect();
            }
        }
    }

    protected void RemoveEffect() {
        AddAmount(attribute, -amount);
        Destroy(this);
    }

    public void Reset() {
        timer = 0.0f;
    }
}
