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

        if(isPassive) {
            UseSpecial();
        }
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    public override bool UseSpecial() {
        if(!base.UseSpecial()) {
            return false;
        }

        if(effectType == EffectType.Debuff)
            ApplyDebuff(attribute, effectPercentage);
        else
            ApplyBuff(attribute, effectPercentage);

        return true;
    }

    protected void ApplyBuff(Attribute attribute, float amount) {
        Effect buff = gameObject.AddComponent<Effect>();
        buff.EffectName = abilityName;
        buff.IsActive = true;
        buff.IsBuff = true;
        buff.Attribute = attribute;
        buff.Amount = amount;
    }

    protected void ApplyDebuff(Attribute attribute, float amount) {
        int childCount = PlayerManager.instance.PlayerInputs.Count;
        for(int i = 0; i < childCount; i++) {
            Transform childTran = PlayerManager.instance.PlayerInputs[i].gameObject.transform.GetChild(0);
            if(childTran != gameObject.transform) {
                Effect debuff = gameObject.AddComponent<Effect>();
                debuff.EffectName = abilityName;
                debuff.IsActive = true;
                debuff.IsBuff = false;
                debuff.Attribute = attribute;
                debuff.Amount = amount;
            }
        }
    }
}
