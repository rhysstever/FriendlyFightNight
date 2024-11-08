using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOverTime : SpecialAbility {
    [SerializeField]
    private float instantAmount, duration, tickRate, tickAmount;
    [SerializeField]
    private Attribute effectOverTimeAttribute;

    public Attribute EffectOverTimeAttribute { get { return effectOverTimeAttribute; } }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        if(isPassive && effectOverTimeAttribute != Attribute.Damage) {
            UseSpecial();
        }
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    public bool UseSpecial(GameObject target) {
        if(!base.UseSpecial()) {
            return false;
        }

        if(target.GetComponent<PlayerCombat>() == null) {
            target = target.GetComponentInChildren<PlayerCombat>().gameObject;
        }

        Debug.Log(target.name);

        switch(effectOverTimeAttribute) {
            case Attribute.Damage:
                TickEffect damageOverTime = gameObject.AddComponent<TickEffect>();
                damageOverTime.EffectName = abilityName;
                damageOverTime.IsActive = true;
                damageOverTime.IsBuff = false;
                damageOverTime.Attribute = Attribute.Damage;
                damageOverTime.Amount = instantAmount;
                damageOverTime.Duration = duration;
                damageOverTime.TickRate = tickRate;
                damageOverTime.TickAmount = tickAmount;
                if(instantAmount > 0) {
                    damageOverTime.ApplyAtStart = true;
                }
                break;
            case Attribute.Health:
                TickEffect healOverTime = gameObject.AddComponent<TickEffect>();
                healOverTime.EffectName = abilityName;
                healOverTime.IsActive = true;
                healOverTime.IsBuff = false;
                healOverTime.Attribute = Attribute.Health;
                healOverTime.Amount = instantAmount;
                healOverTime.Duration = duration;
                healOverTime.TickRate = tickRate;
                healOverTime.TickAmount = tickAmount;
                if(instantAmount > 0) {
                    healOverTime.ApplyAtStart = true;
                }
                break;
        }

        return true;
    }

    public override bool UseSpecial() {
        return UseSpecial(gameObject);
    }
}
