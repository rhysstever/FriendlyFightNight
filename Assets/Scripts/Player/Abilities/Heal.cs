using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : SpecialAbility {
    [SerializeField]
    private float instantAmount, duration, tickRate, tickAmount;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    public override void UseSpecial() {
        if(CanUseSpecial()) {
            base.UseSpecial();
            gameObject.GetComponent<PlayerCombat>().Heal(instantAmount);
            Effect healingOverTime = new Effect(
                abilityName,
                true,
                Attribute.Damage,
                tickAmount,
                duration);
            gameObject.GetComponent<PlayerCombat>().ApplyEffect(healingOverTime);
        }
    }
}
