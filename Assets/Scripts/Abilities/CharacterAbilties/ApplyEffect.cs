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
        if(!base.UseSpecial()) {
            return false;
        }

        if(isApplying) {
            if(effectType == EffectType.Buff) {
                gameObject.GetComponent<PlayerCombat>().AddEffect(
                    abilityName, true, true, attribute, amount);
            } else {
                int childCount = PlayerManager.instance.PlayerInputs.Count;
                for(int i = 0; i < childCount; i++) {
                    Transform childTran = PlayerManager.instance.PlayerInputs[i].gameObject.transform.GetChild(0);
                    if(childTran != gameObject.transform) {
                        childTran.gameObject.GetComponent<PlayerCombat>().AddEffect(
                            abilityName, true, false, attribute, amount);
                    }
                }
            }
        }

        return true;
    }
}
