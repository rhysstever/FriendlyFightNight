using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Leech : SpecialAbility {
    [SerializeField]
    private List<Attribute> leechingAttributes;

    [SerializeField]
    private float leechingAmountPercentage, leechingDuration;

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    public override bool UseSpecial() {
        if(!base.UseSpecial()) {
            return false;
        }

        // Get the target
        GameObject target = GetTarget();
        if(target == null) {
            return false;
        }

        // Get the attribute being leeched
        Attribute leechingAttribute = leechingAttributes[Random.Range(0, leechingAttributes.Count)];
        string leechingName = string.Format("{0}: {1}", AbilityName, leechingAttribute.ToString());
        Debug.Log(leechingName);

        // Take amount away from target
        ActiveEffect targetDebuff = gameObject.AddComponent<ActiveEffect>();
        targetDebuff.EffectName = leechingName;
        targetDebuff.IsActive = true;
        targetDebuff.IsBuff = false;
        targetDebuff.Attribute = leechingAttribute;
        targetDebuff.Amount = leechingAmountPercentage;
        targetDebuff.Duration = leechingDuration;

        // Give amount to player
        ActiveEffect selfBuff = gameObject.AddComponent<ActiveEffect>();
        selfBuff.EffectName = leechingName;
        selfBuff.IsActive = true;
        selfBuff.IsBuff = true;
        selfBuff.Attribute = leechingAttribute;
        selfBuff.Amount = leechingAmountPercentage;
        selfBuff.Duration = leechingDuration;

        return true;
    }

    private GameObject GetTarget() {
        foreach(PlayerInput playerInput in PlayerManager.instance.PlayerInputs) {
            if(playerInput.gameObject.transform.GetChild(0).gameObject != gameObject) {
                return playerInput.gameObject.transform.GetChild(0).gameObject;
            }
        }

        return null;
    }
}
