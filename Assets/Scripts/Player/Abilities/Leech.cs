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

    // Update is called once per frame
    protected override void Update()
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

        // Take amount away from target
        Effect targetDebuff = new Effect(leechingName, false, leechingAttribute, leechingAmountPercentage, 1.0f, leechingDuration);
        target.GetComponent<PlayerCombat>().ApplyEffect(targetDebuff);

        // Give amount to player
        Effect selfBuff = new Effect(leechingName, true, leechingAttribute, leechingAmountPercentage, 1.0f, leechingDuration);
        gameObject.GetComponent<PlayerCombat>().ApplyEffect(selfBuff);

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
