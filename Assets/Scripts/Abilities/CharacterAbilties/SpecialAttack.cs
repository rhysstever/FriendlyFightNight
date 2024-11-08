using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : SpecialAbility {
    [SerializeField]
    private GameObject attackProjectile;
    [SerializeField]
    private Vector2 projectileSpeed;
    [SerializeField]
    private float projectileDamage, projectileLifespan;
    [SerializeField]
    private FirePosition firePosition;
    [SerializeField]
    private bool usesGravity;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    public override bool UseSpecial() {
        if(!base.UseSpecial()) {
            return false;
        }

        GameObject specialProjectile = gameObject.GetComponent<PlayerCombat>().Fire(
            attackProjectile,
            projectileSpeed,
            firePosition,
            new Vector2(0.45f, 0.25f),
            usesGravity);
        specialProjectile.GetComponent<SpecialProjectile>().SetValues(projectileDamage, projectileLifespan);
        return true;
    }
}
