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

        while(passivePercentage > 1.0f)
        {
            passivePercentage /= 10.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(passiveType == PassiveType.Debuff)
            ApplyDebuff(passiveAttribute, passivePercentage, passiveRange);
        else
            ApplyBuff(passiveAttribute, passivePercentage);
    }

    private void ApplyBuff(PassiveAttribute attribute, float amount)
    {
        Effect effect = new Effect(abilityName, true, attribute, amount, 1.0f);
        GetComponent<PlayerCombat>().ApplyEffect(effect);
    }

    private void ApplyDebuff(PassiveAttribute attribute, float amount, float range)
    {
        int childCount = PlayerManager.instance.PlayerInputs.Count;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTran = PlayerManager.instance.PlayerInputs[i].gameObject.transform.GetChild(0);
            if(childTran != gameObject.transform)
            {
                if(Vector3.Distance(childTran.position, gameObject.transform.position) <= range)
                {
                    Effect effect = new Effect(abilityName, false, attribute, amount, 1.0f);
                    childTran.GetComponent<PlayerCombat>().ApplyEffect(effect);
                }
            }
        }
    }
}
