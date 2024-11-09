using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField]
    protected string effectName;
    [SerializeField]
    protected bool isActive, isBuff;
    [SerializeField]
    protected Attribute attribute;
    [SerializeField]
    protected float amount;

    public string EffectName { get { return effectName; } set { effectName = value; } }
    public bool IsActive { set { isActive = value; } }
    public bool IsBuff { set { isBuff = value; } }
    public Attribute Attribute { set { attribute = value; } }
    public float Amount { set { amount = value; } }

    protected PlayerMovement movement;
    protected PlayerCombat combat;

    protected virtual void Awake() {
        movement = transform.parent.GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        ApplyEffect();
    }

    protected void ApplyEffect() {
        if(isActive) {
            if(!isBuff && attribute != Attribute.Damage)
                amount *= -1;

            AddAmount(attribute, amount);
        }
    }

    protected void AddAmount(Attribute attribute, float amount) {
        switch(attribute) {
            case Attribute.Armor:
                combat.AdjustArmor(amount);
                break;
            case Attribute.BulletGravity:
                combat.AdjustBulletGravity(amount);
                break;
            case Attribute.Damage:
                combat.TakeDamage(amount);
                break;
            case Attribute.DamageMod:
                combat.AdjustDamage(amount);
                break;
            case Attribute.Health:
                combat.Heal(amount);
                break;
            case Attribute.MoveSpeed:
                movement.AdjustMoveSpeed(amount);
                break;
        }
    }

    public void Toggle(bool newActiveState) {
        if(isActive == newActiveState) {
            return;
        } else {
            isActive = newActiveState;
            if(newActiveState) {
                AddAmount(attribute, amount);
            } else {
                AddAmount(attribute, -amount);
            }
        }
    }
}
