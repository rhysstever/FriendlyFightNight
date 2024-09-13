using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActiveType
{
    Default,
    SpecialAttack,
    Heal,
    Buff,
    Debuff
}

public class ActiveAbility : SpecialAbility
{
    [SerializeField]
    private float cooldown;

    internal ActiveType activeType;
    private float cooldownTimer;

    public ActiveType ActiveType { get { return activeType; } }

    // Start is called before the first frame update
    void Start()
    {
        abilityType = AbilityType.Active;
        activeType = ActiveType.Default;
        cooldownTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        cooldownTimer += Time.deltaTime;
    }

    public bool CanUseSpecial()
    {
        return cooldownTimer >= cooldown;
    }

    public virtual void UseSpecial()
    {
        cooldownTimer = 0.0f;
    }
}
