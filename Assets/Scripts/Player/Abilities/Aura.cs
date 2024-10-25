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
        if(!isActive) {
            UseSpecial();
        }
    }

    public override void UseSpecial() {
        if(effectType == EffectType.Debuff)
            ApplyDebuff(attribute, effectPercentage, range);
        else
            ApplyBuff(attribute, effectPercentage, range);
    }

    protected void ApplyBuff(Attribute attribute, float amount, float range) {
        Effect effect = new Effect(abilityName, true, attribute, amount, 1.0f);
        GetComponent<PlayerCombat>().ApplyEffect(effect);
    }

    protected void ApplyDebuff(Attribute attribute, float amount, float range) {
        int childCount = PlayerManager.instance.PlayerInputs.Count;
        for(int i = 0; i < childCount; i++) {
            Transform childTran = PlayerManager.instance.PlayerInputs[i].gameObject.transform.GetChild(0);
            if(childTran != gameObject.transform) {
                if(affectWithinRange) {
                    if(Vector3.Distance(childTran.position, gameObject.transform.position) <= range) {
                        Effect effect = new Effect(abilityName, false, attribute, amount, 1.0f);
                        childTran.GetComponent<PlayerCombat>().ApplyEffect(effect);
                    }
                } else {
                    if(Vector3.Distance(childTran.position, gameObject.transform.position) >= range) {
                        Effect effect = new Effect(abilityName, false, attribute, amount, 1.0f);
                        childTran.GetComponent<PlayerCombat>().ApplyEffect(effect);
                    }
                }
            }
        }
    }
}
