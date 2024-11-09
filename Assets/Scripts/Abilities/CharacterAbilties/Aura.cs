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
        if(effectType == EffectType.Buff) {
            gameObject.GetComponent<PlayerCombat>().AddEffect(
                abilityName, true, true, attribute, amount);
        }
        else {
            // Loop through all players and find all other players
            int childCount = PlayerManager.instance.PlayerInputs.Count;
            for(int i = 0; i < childCount; i++) {
                // Convert from the parent player object to the child character gameObject
                GameObject childCharObject = PlayerManager.instance.PlayerInputs[i].gameObject.transform.GetChild(0).gameObject;
                // Affect the gameObject if it is not this gameObject 
                if(childCharObject != gameObject) {
                    // Check if the character has the effect already,
                    if(childCharObject.GetComponent<PlayerCombat>().GetEffect(abilityName) != null) {
                        // If so, update whether it is active (based on range)
                        childCharObject.GetComponent<PlayerCombat>().UpdateEffect(
                            abilityName, IsInAffectedRange(childCharObject, range, affectWithinRange));
                    } else {
                        // If not, add the effect
                        childCharObject.GetComponent<PlayerCombat>().AddEffect(
                            abilityName, IsInAffectedRange(childCharObject, range, affectWithinRange),
                            false, attribute, amount);
                    }
                }
            }
        }
        return true;
    }

    private bool IsInAffectedRange(GameObject target, float range, bool affectWithinRange) {
            // 1) If the aura affects within range AND the target is within range
        return (affectWithinRange 
            && Vector3.Distance(
                target.transform.position,
                gameObject.transform.position
            ) <= range) ||  // OR
            // 2) if the aura affects outside of range AND the target is outside of range
            (!affectWithinRange
            && Vector3.Distance(
                target.transform.position,
                gameObject.transform.position
            ) >= range);
    }
}
