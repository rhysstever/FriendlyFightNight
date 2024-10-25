using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOverTime : SpecialAbility {
    [SerializeField]
    private float instantAmount, duration, tickRate, tickAmount;
    [SerializeField]
    private Attribute effectOverTimeAttribute;

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

    public override void UseSpecial() {
        if(CanUseSpecial()) {
            base.UseSpecial();

            switch(effectOverTimeAttribute) {
                case Attribute.Damage:
                    gameObject.GetComponent<PlayerCombat>().Heal(instantAmount);
                    Effect damageOverTime = new Effect(
                        abilityName,
                        false,
                        Attribute.Damage,
                        tickAmount,
                        duration);
                    gameObject.GetComponent<PlayerCombat>().ApplyEffect(damageOverTime);
                    break;
                case Attribute.Health:
                    gameObject.GetComponent<PlayerCombat>().Heal(instantAmount);
                    Effect healOverTime = new Effect(
                        abilityName,
                        true,
                        Attribute.Health,
                        tickAmount,
                        duration);
                    gameObject.GetComponent<PlayerCombat>().ApplyEffect(healOverTime);
                    break;
            }
        }
    }
}
