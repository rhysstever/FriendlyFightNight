using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    Buff,
    Debuff
}

public enum Attribute {
    Armor,
    BulletGravity,
    Damage,
    DamageMod,
    Health,
    MoveSpeed
}

public class ApplyEffect : SpecialAbility {
    [SerializeField]
    protected EffectType effectType;
    [SerializeField]
    protected Attribute attribute;
    [SerializeField]
    protected float effectPercentage;

    public EffectType EffectType { get { return effectType; } }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        while(effectPercentage > 1.0f) {
            effectPercentage /= 10.0f;
        }
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        
        if(!isActive) {
            UseSpecial();
        }
    }

    public override void UseSpecial() {
        if(effectType == EffectType.Debuff)
            ApplyDebuff(attribute, effectPercentage);
        else
            ApplyBuff(attribute, effectPercentage);
    }

    protected void ApplyBuff(Attribute attribute, float amount) {
        Effect effect = new Effect(abilityName, true, attribute, amount, 0.0f, 1.0f);
        GetComponent<PlayerCombat>().ApplyEffect(effect);
    }

    protected void ApplyDebuff(Attribute attribute, float amount) {
        int childCount = PlayerManager.instance.PlayerInputs.Count;
        for(int i = 0; i < childCount; i++) {
            Transform childTran = PlayerManager.instance.PlayerInputs[i].gameObject.transform.GetChild(0);
            if(childTran != gameObject.transform) {
                Effect effect = new Effect(abilityName, false, attribute, amount, 0.0f, 1.0f);
                childTran.GetComponent<PlayerCombat>().ApplyEffect(effect);
            }
        }
    }
}
