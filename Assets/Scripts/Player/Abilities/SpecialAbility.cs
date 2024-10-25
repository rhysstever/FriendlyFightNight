using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbility : MonoBehaviour {
    [SerializeField]
    protected string abilityName;
    [SerializeField]
    protected float cooldown;
    protected float cooldownTimer;
    [SerializeField]
    protected bool isActive;

    public string AbilityName { get { return abilityName; } }
    public float Cooldown { get { return cooldown; } }
    public float CooldownTimer { get { return cooldownTimer; } }
    public bool IsActive { get { return isActive; } }
    public float CooldownPercentage { get { return cooldownTimer / cooldown; } }

    // Start is called before the first frame update
    protected virtual void Start() {
        cooldownTimer = 0.0f;
    }

    // Update is called once per frame
    protected virtual void Update() {

    }

    protected virtual void FixedUpdate() {
        if(cooldownTimer < cooldown)
            cooldownTimer += Time.deltaTime;
    }

    public virtual void UseSpecial() {
        if(!CanUseSpecial()) {
            return;
        }

        cooldownTimer = 0.0f;
    }

    public bool CanUseSpecial() {
        return cooldownTimer >= cooldown;
    }
}
