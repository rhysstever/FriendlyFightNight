using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : ApplyEffect
{
    [SerializeField]
    private float range;
    [SerializeField]
    private bool affectWithinRange;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    protected void Update() {
        if(isPassive) {
            UseAura();
        }
    }

    public override bool UseSpecial() {
        Debug.Log("Using Aura");
        if(!base.UseSpecial(false)) {
            return false;
        }

        return UseAura();
    }

    private bool UseAura() {
        if(effectType == EffectType.Buff)
            ApplyBuff(attribute, amount);
        else
            ApplyDebuff(attribute, amount, range);
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

    private void ApplyDebuff(Attribute attribute, float amount, float range) {
        int childCount = PlayerManager.instance.PlayerInputs.Count;
        for(int i = 0; i < childCount; i++) {
            GameObject childCharObject = PlayerManager.instance.PlayerInputs[i].gameObject.transform.GetChild(0).gameObject;
            if(childCharObject != gameObject) {
                if((affectWithinRange           //  1) If the aura affects within range
                    && Vector3.Distance(        // AND the target is within range
                        childCharObject.transform.position, 
                        gameObject.transform.position
                        ) <= range)
                    || (!affectWithinRange      // OR 2) if the aura affects outside of range
                        && Vector3.Distance(    // AND the target is outside of range
                            childCharObject.transform.position, 
                            gameObject.transform.position
                        ) >= range)) {
                    Effect debuff = childCharObject.AddComponent<Effect>();
                    debuff.EffectName = abilityName;
                    debuff.IsActive = true;
                    debuff.IsBuff = false;
                    debuff.Attribute = attribute;
                    debuff.Amount = amount;
                }
            }
        }
    }
}
