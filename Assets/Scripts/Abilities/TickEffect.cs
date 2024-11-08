using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickEffect : ActiveEffect {
    [SerializeField]
    private bool applyAtStart;
    [SerializeField]
    private float tickRate, tickAmount;

    private float tickTimer;

    public float TickRate { set { tickRate = value; } }
    public float TickAmount { set { tickAmount = value; } }
    public bool ApplyAtStart { set { applyAtStart = value; } }

    protected override void Awake() {
        tickTimer = 0.0f;
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start() {
        if(applyAtStart) { 
            base.Start();
        }
    }

    protected override void FixedUpdate() {
        if(isActive) {
            tickTimer += Time.deltaTime;

            if(tickTimer > tickRate) { 
                Tick();
            }
        }

        base.FixedUpdate();
    }

    protected void Tick() {
        AddAmount(attribute, tickAmount);
        tickTimer = 0.0f;
    }
}
