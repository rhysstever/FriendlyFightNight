using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : ActiveAbility
{
    [SerializeField]
    private GameObject attackProjectile;
    [SerializeField]
    private Vector2 projectileSpeed;
    [SerializeField]
    private float projectileDamage, projectileLifespan;
    [SerializeField]
    private FirePosition firePosition;

    // Start is called before the first frame update
    void Start()
    {
        activeType = ActiveType.SpecialAttack;
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
            GameObject specialProjectile = gameObject.GetComponent<PlayerCombat>().Fire(
                attackProjectile,
                projectileSpeed,
                FirePosition.Head,
                new Vector2(0.45f, 0.25f));
            specialProjectile.GetComponent<SpecialProjectile>().SetValues(projectileDamage, projectileLifespan);
        }
    }
}
