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
    protected bool isPassive;

    public string AbilityName { get { return abilityName; } }
    public float Cooldown { get { return cooldown; } }
    public float CooldownTimer { get { return cooldownTimer; } }
    public bool IsPassive { get { return isPassive; } }
    public float CooldownPercentage { get { return cooldownTimer / cooldown; } }

    // Start is called before the first frame update
    protected virtual void Start() {
        if(isPassive) {
            cooldown = 0.0f;
        }
        cooldownTimer = 0.0f;
    }

    protected virtual void FixedUpdate() {
        if(GameManager.instance.CurrentMenuState == MenuState.Game) {
            cooldownTimer += Time.deltaTime;
        }
    }

    public virtual bool UseSpecial() {
        if(!CanUseSpecial()) {
            return false;
        }

        ResetCooldown();
        return true;
    }

    protected bool CanUseSpecial() {
        return cooldownTimer >= cooldown;
    }

    public void ResetCooldown() {
        cooldownTimer = 0.0f;
    }
}
