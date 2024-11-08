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

    // Update is called once per frame
    protected override void Update() {
        
    }

    public override bool UseSpecial() {
        if(!base.UseSpecial()) {
            return false;
        }

        if(effectType == EffectType.Debuff)
            ApplyDebuff(attribute, effectPercentage, range);
        else
            ApplyBuff(attribute, effectPercentage, range);

        return true;
    }

    protected void ApplyBuff(Attribute attribute, float amount, float range) {
        Effect buff = gameObject.AddComponent<Effect>();
        buff.EffectName = abilityName;
        buff.IsActive = true;
        buff.IsBuff = true;
        buff.Attribute = attribute;
        buff.Amount = amount;
    }

    protected void ApplyDebuff(Attribute attribute, float amount, float range) {
        int childCount = PlayerManager.instance.PlayerInputs.Count;
        for(int i = 0; i < childCount; i++) {
            Transform childTran = PlayerManager.instance.PlayerInputs[i].gameObject.transform.GetChild(0);
            if(childTran != gameObject.transform) {
                if((affectWithinRange 
                    && Vector3.Distance(
                        childTran.position, 
                        gameObject.transform.position
                        ) <= range
                    ) || (!affectWithinRange 
                    && Vector3.Distance(
                        childTran.position, 
                        gameObject.transform.position
                        ) >= range)
                ) {
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
}
