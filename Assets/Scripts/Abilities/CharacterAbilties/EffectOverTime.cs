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

        // Ensure the target is the Character Object
        if(target.GetComponent<PlayerCombat>() == null) {
            target = target.GetComponentInChildren<PlayerCombat>().gameObject;
        }

        target.GetComponent<PlayerCombat>().AddEffect(
                abilityName, true, effectType == EffectType.Buff, attribute,
                amount, duration, tickRate, tickAmount);
        return true;
    }

    public override bool UseSpecial() {
        return UseSpecial(gameObject);
    }
}
