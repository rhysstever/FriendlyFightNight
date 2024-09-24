using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : ActiveAbility
{
    [SerializeField]
    private float instantAmount;

    // Start is called before the first frame update
    void Start()
    {
        activeType = ActiveType.Heal;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void UseSpecial()
    {
        if(CanUseSpecial())
        {
            base.UseSpecial();
            gameObject.GetComponent<PlayerCombat>().Heal(instantAmount);
        }
    }
}
