using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOverTime : ApplyEffect {
    [SerializeField]
    private float duration, tickRate, tickAmount;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    public bool UseSpecial(GameObject target) {
        if(!base.UseSpecial(false)) {
            return false;
        }

        if(effectType == EffectType.Buff)
            ApplyBuff(target, attribute, amount);
        else
            ApplyDebuff(target, attribute, amount);

        return true;
    }

    public override bool UseSpecial() {
        return UseSpecial(gameObject);
    }

    private void ApplyBuff(GameObject target, Attribute attribute, float amount) {
        // Apply the heal over time tick effect
        TickEffect healOverTime = target.AddComponent<TickEffect>();
        healOverTime.EffectName = abilityName;
        healOverTime.IsActive = true;
        healOverTime.IsBuff = true;
        healOverTime.Attribute = attribute;
        healOverTime.Amount = amount;
        healOverTime.Duration = duration;
        healOverTime.TickRate = tickRate;
        healOverTime.TickAmount = tickAmount;
        if(amount > 0) {
            healOverTime.ApplyAtStart = true;
        }
    }

    private void ApplyDebuff(GameObject target, Attribute attribute, float amount) {
        // Ensure the target is the Character Object
        if(target.GetComponent<PlayerCombat>() == null) {
            target = target.GetComponentInChildren<PlayerCombat>().gameObject;
        }
        // Apply the damage over time tick effect
        TickEffect damageOverTime = target.AddComponent<TickEffect>();
        damageOverTime.EffectName = abilityName;
        damageOverTime.IsActive = true;
        damageOverTime.IsBuff = false;
        damageOverTime.Attribute = attribute;
        damageOverTime.Amount = amount;
        damageOverTime.Duration = duration;
        damageOverTime.TickRate = tickRate;
        damageOverTime.TickAmount = tickAmount;
        if(amount > 0) {
            damageOverTime.ApplyAtStart = true;
        }
    }
}
