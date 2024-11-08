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
    protected float amount;

    public EffectType EffectType { get { return effectType; } }
    public Attribute Attribute { get { return attribute; } }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        if(isPassive && effectType == EffectType.Buff) {
            UseSpecial();
        }
    }

    public override bool UseSpecial() {
        return UseSpecial();
    }

    public bool UseSpecial(bool isApplying = true) {
        Debug.Log("Using Apply Effect");
        if(!base.UseSpecial()) {
            return false;
        }

        if(isApplying) {
            if(effectType == EffectType.Debuff)
                ApplyDebuff(attribute, amount);
            else
                ApplyBuff(attribute, amount);
        }

        return true;
    }

    private void ApplyBuff(Attribute attribute, float amount) {
        Effect buff = gameObject.AddComponent<Effect>();
        buff.EffectName = abilityName;
        buff.IsActive = true;
        buff.IsBuff = true;
        buff.Attribute = attribute;
        buff.Amount = amount;
    }

    private void ApplyDebuff(Attribute attribute, float amount) {
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
