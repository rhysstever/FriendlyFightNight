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
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();

        if(!isActive && effectOverTimeAttribute != Attribute.Damage) {
            UseSpecial();
        }
    }

    public void UseSpecial(GameObject target) {
        if(CanUseSpecial()) {
            base.UseSpecial();

            if(target.GetComponent<PlayerCombat>() == null) {
                target = target.GetComponentInChildren<PlayerCombat>().gameObject;
            }

            switch(effectOverTimeAttribute) {
                case Attribute.Damage:
                    Debug.Log(target.name);
                    target.GetComponent<PlayerCombat>().TakeDamage(instantAmount);
                    Effect damageOverTime = new Effect(
                        abilityName,
                        false,
                        Attribute.Damage,
                        tickAmount,
                        tickRate,
                        duration);
                    target.GetComponent<PlayerCombat>().ApplyEffect(damageOverTime);
                    break;
                case Attribute.Health:
                    target.GetComponent<PlayerCombat>().Heal(instantAmount);
                    Effect healOverTime = new Effect(
                        abilityName,
                        true,
                        Attribute.Health,
                        tickAmount,
                        tickRate,
                        duration);
                    target.GetComponent<PlayerCombat>().ApplyEffect(healOverTime);
                    break;
            }
        }
    }

    public override void UseSpecial() {
        UseSpecial(gameObject);
    }
}
