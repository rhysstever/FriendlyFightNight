using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassiveType
{
    Buff,
    Debuff
}

public enum PassiveAttribute
{
    Damage,
    Armor,
    MoveSpeed,
    BulletGravity
}

public class PassiveAbility : SpecialAbility
{
    [SerializeField]
    private PassiveType passiveType;
    [SerializeField]
    private PassiveAttribute passiveAttribute;
    [SerializeField]
    private float passivePercentage, passiveRange;

    public PassiveType PassiveType { get { return passiveType; } }

    // Start is called before the first frame update
    void Start()
    {
        abilityType = AbilityType.Passive;

        if(passiveType == PassiveType.Buff)
            ApplyBuff(passiveAttribute, passivePercentage);
    }

    // Update is called once per frame
    void Update()
    {
        if(passiveType == PassiveType.Debuff)
            ApplyDebuff(passiveAttribute, passivePercentage, passiveRange);
    }

    private void ApplyBuff(PassiveAttribute attribute, float amount)
    {
        switch(attribute)
        {
            case PassiveAttribute.Damage:
                GetComponent<PlayerCombat>().AdjustDamage(amount);
                break;
            case PassiveAttribute.Armor:
                GetComponent<PlayerCombat>().AdjustArmor(amount);
                break;
            case PassiveAttribute.MoveSpeed:
                GetComponent<PlayerMovement>().AdjustMoveSpeed(amount);
                break;
            case PassiveAttribute.BulletGravity:
                GetComponent<PlayerCombat>().AdjustBulletGravity(amount);
                break;
        }
    }

    private void ApplyDebuff(PassiveAttribute attribute, float amount, float range)
    {
        int childCount = GameManager.instance.Players.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = GameManager.instance.Players.transform.GetChild(i).gameObject;
            if(child != gameObject)
            {
                if(Vector3.Distance(child.transform.position, gameObject.transform.position) <= range)
                {
                    ApplyBuff(attribute, -amount);
                }
            }
        }
    }
}
