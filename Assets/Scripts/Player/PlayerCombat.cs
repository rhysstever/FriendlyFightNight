using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum FirePosition {
    Head,
    Shoulder,
    Torso,
    Knees,
    Feet
}

public class PlayerCombat : MonoBehaviour {

    [SerializeField]
    private Character character;
    [SerializeField]
    private float fireRate, bulletSpeed, currentHealth, maxHealth, damage, bulletGravity, armor;
    [SerializeField]
    private float damageMod, bulletGravityMod, armorMod;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private SpecialAbility passiveAbility, activeAbility;

    private float fireTimer;
    private float takeDamageDuration, takeDamageTimer;
    private bool isTakingDamage;

    public Character Character { get { return character; } }
    public float HealthPercentage { get { return currentHealth / maxHealth; } }
    public SpecialAbility PassiveAbility { get { return passiveAbility; } }
    public SpecialAbility ActiveAbility { get { return activeAbility; } }

    private void Awake() {
        currentHealth = maxHealth;
    }

    // Start is called before the first frame update
    void Start() {
        fireTimer = fireRate;   // can fire right away

        takeDamageDuration = 0.25f;
        takeDamageTimer = 0.0f;
        isTakingDamage = false;

        damageMod = 0.0f;
        bulletGravityMod = 0.0f;
        armorMod = 0.0f;
    }

    private void Update() {
        
    }

    private void FixedUpdate() {
        if(GameManager.instance.CurrentMenuState == MenuState.Game) {
            fireTimer += Time.deltaTime;

            CheckTakeDamage(Time.deltaTime);
        }
    }

    /// <summary>
    /// Whether the player can fire given the game's and character's current state
    /// </summary>
    /// <returns>A bool for if the player can fire</returns>
    private bool CanFire() {
        return GameManager.instance.CurrentMenuState == MenuState.Game && fireTimer >= fireRate;
    }

    /// <summary>
    /// Tries to fire a normal bullet projectile
    /// </summary>
    /// <returns>The bullet GameObject that was fired</returns>
    public GameObject FireBullet() {
        GameObject newBullet = Fire(bullet, new Vector2(bulletSpeed, 0.0f), FirePosition.Torso, new Vector2(0.25f, 0.0f));
        if(newBullet != null) {
            newBullet.GetComponent<Bullet>().damage = damage + damageMod;
            newBullet.GetComponent<Bullet>().source = gameObject;
            fireTimer = 0.0f;
        }
        return newBullet;
    }

    /// <summary>
    /// Fires a specified projectile
    /// </summary>
    /// <param name="bullet">The prefab GameObject for the bullet</param>
    /// <param name="bulletSpeed">The speed of the bullet once it is fired</param>
    /// <param name="origin">The position where the bullet is fired from (based on the player's character thats firing it)</param>
    /// <param name="extraOffset">Additional position offset where the bullet is fired from</param>
    /// <param name="usesGravity">Whether the bullet is using gravity once it is fired</param>
    /// <returns></returns>
    public GameObject Fire(GameObject bullet, Vector2 bulletSpeed, FirePosition origin, Vector2 extraOffset, bool usesGravity = true) {
        if(CanFire()) {
            float facingDirection = transform.parent.GetComponent<PlayerMovement>().FacingDirection;
            // Figure out the x-offset based on the direction of the player
            float offsetX = gameObject.transform.localScale.x / 2;
            offsetX += extraOffset.x;
            offsetX *= facingDirection;
            // Figure out the y-offset based on the fire position
            float offsetY = GetOffsetYFromFireOrigin(gameObject, origin, extraOffset.y);
            // Calculate the bullet's starting position based on the player and the offsets
            Vector3 bulletPos = new Vector3(
                transform.position.x + offsetX,
                transform.position.y + offsetY,
                transform.position.z);
            
            // Create the bullet in the scene under the bullet parent gameObject
            GameObject newBullet = Instantiate(bullet, bulletPos, Quaternion.identity, GameManager.instance.Bullets.transform);
            // Set the bullet's stats
            Vector2 bulletSpeedWithDirection = bulletSpeed;
            bulletSpeedWithDirection.x *= facingDirection;
            newBullet.GetComponent<Rigidbody2D>().velocity = bulletSpeedWithDirection;
            float newBulletGravity = bulletGravity + bulletGravityMod;
            if(!usesGravity) {
                newBulletGravity = 0.0f;
            }
            newBullet.GetComponent<Rigidbody2D>().gravityScale = newBulletGravity;

            return newBullet;
        }

        return null;
    }

    /// <summary>
    /// Resets the player's combat timers
    /// </summary>
    public void ResetTimers() {
        fireTimer = fireRate;
        takeDamageTimer = 0.0f;
        activeAbility.ResetCooldown();
    }

    /// <summary>
    /// A helper function to get the vertical offset
    /// </summary>
    /// <param name="gameObject">The gameObject firing the projectile (needed for its size)</param>
    /// <param name="origin">The firing position that will be the main factor for the projectile's origin</param>
    /// <param name="extraOffsetY">Additional offset for the projectile's origin</param>
    /// <returns>The float offset for the y-position of the projectile</returns>
    private float GetOffsetYFromFireOrigin(GameObject gameObject, FirePosition origin, float extraOffsetY) {
        float offsetY = 0.0f;
        switch(origin) {
            case FirePosition.Head:
                offsetY = gameObject.transform.localScale.y / 2;
                offsetY += extraOffsetY;
                break;
            case FirePosition.Shoulder:
                offsetY = gameObject.transform.localScale.y / 4;
                offsetY += extraOffsetY;
                break;
            case FirePosition.Torso:
                offsetY += extraOffsetY;
                break;
            case FirePosition.Knees:
                offsetY = -gameObject.transform.localScale.y / 4;
                offsetY -= extraOffsetY;
                break;
            case FirePosition.Feet:
                offsetY = -gameObject.transform.localScale.y / 2;
                offsetY -= extraOffsetY;
                break;
        }
        return offsetY;
    }

    /// <summary>
    /// Deal damage to the player object
    /// </summary>
    /// <param name="amount">The amount of damage (pre-armor reductions) dealth</param>
    public void TakeDamage(float amount) {
        // Calculate damage taken after armor reductions
        float damageTaken = amount * (2 - armor + armorMod);
        currentHealth -= damageTaken;
        isTakingDamage = true;

        // Update UI
        UIManager.instance.UpdatePlayerHealth();

        // Check for the player's death
        if(currentHealth <= 0.0f) {
            GameManager.instance.ChangeMenuState(MenuState.Results);
        }
    }

    /// <summary>
    /// Handles when the player takes damage (mostly visual)
    /// </summary>
    /// <param name="increment">How much time should be added to the timer if it is active</param>
    private void CheckTakeDamage(float increment) {
        if(isTakingDamage) {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            // Toggle between red and white for the material
            spriteRenderer.color = Color.red;

            // Increment timer and check if its done
            takeDamageTimer += increment;
            if(takeDamageTimer >= takeDamageDuration) {
                // Revert values
                isTakingDamage = false;
                takeDamageTimer = 0.0f;
                spriteRenderer.color = Color.white;
            }
        }
    }

    /// <summary>
    /// Heal the player object
    /// </summary>
    /// <param name="amount">The minimum amount of damage healed (cannot heal past max)</param>
    public void Heal(float amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, currentHealth, maxHealth);
        UIManager.instance.UpdatePlayerHealth();
    }

    /// <summary>
    /// Modify the player's armor
    /// </summary>
    /// <param name="amount">The amount of armor gained or lost</param>
    public void AdjustArmor(float amount) {
        armorMod += amount;
    }

    /// <summary>
    /// Modify the player's bullet gravity
    /// </summary>
    /// <param name="amount">The amount of bullet gravity gained or lost</param>
    public void AdjustBulletGravity(float amount) {
        bulletGravityMod += amount;
    }

    /// <summary>
    /// Modify the amount of damage the player deals
    /// </summary>
    /// <param name="amount">The amount of damage gained or lost</param>
    public void AdjustDamage(float amount) {
        damageMod += amount;
    }

    /// <summary>
    /// Applies an Effect to the player
    /// </summary>
    /// <param name="effectName">The name of the Effect</param>
    /// <param name="isActive">Whether the Effect is currently active</param>
    /// <param name="isBuff">Whether the Effect is helping the player</param>
    /// <param name="attribute">The Attribute the Effect is affecting</param>
    /// <param name="amount">The amount of the attribute the Effect is affecting</param>
    /// <returns>The Effect component that is applied to the player</returns>
    public Effect AddEffect(
        string effectName, bool isActive, bool isBuff, 
        Attribute attribute, float amount
    ) {
        Effect effect = GetEffect(effectName);
        if(effect != null) {
            return effect;
        } else {
            // Create a new Effect and add it to the player
            Effect newEffect = gameObject.AddComponent<Effect>();
            newEffect.EffectName = effectName;
            newEffect.IsActive = isActive;
            newEffect.IsBuff = isBuff;
            newEffect.Attribute = attribute;
            newEffect.Amount = amount;
            return newEffect;
        }
    }

    /// <summary>
    /// Applies an Active Effect to the player
    /// </summary>
    /// <param name="effectName">The name of the Effect</param>
    /// <param name="isActive">Whether the Effect is currently active</param>
    /// <param name="isBuff">Whether the Effect is helping the player</param>
    /// <param name="attribute">The Attribute the Effect is affecting</param>
    /// <param name="amount">The amount of the attribute the Effect is affecting</param>
    /// <param name="duration">The duration this Effect is active for</param>
    /// <returns>The Active Effect component that is applied to the player. If the Effect was already on the player, its duration is reset</returns>
    public Effect AddEffect(
        string effectName, bool isActive, bool isBuff, 
        Attribute attribute, float amount, float duration
    ) {
        ActiveEffect effect = (ActiveEffect)GetEffect(effectName);
        if(effect != null) {
            // If the Effect already exists, reset its cooldown
            effect.Reset();
            return effect;
        } else {
            // Create a new ActiveEffect Effect and add it to the player
            ActiveEffect newEffect = gameObject.AddComponent<ActiveEffect>();
            newEffect.EffectName = effectName;
            newEffect.IsActive = isActive;
            newEffect.IsBuff = isBuff;
            newEffect.Attribute = attribute;
            newEffect.Amount = amount;
            newEffect.Duration = duration;
            return newEffect;
        }
    }

    /// <summary>
    /// Applies an Tick Effect to the player
    /// </summary>
    /// <param name="effectName">The name of the Effect</param>
    /// <param name="isActive">Whether the Effect is currently active</param>
    /// <param name="isBuff">Whether the Effect is helping the player</param>
    /// <param name="attribute">The Attribute the Effect is affecting</param>
    /// <param name="amount">The amount of the attribute the Effect applies/removes</param>
    /// <param name="duration">The duration this Effect is active for</param>
    /// <param name="tickRate">The rate that this Effect "ticks" and affects the player</param>
    /// <param name="tickAmount">The amount of the attribute the Effect applies/removes when it ticks</param>
    /// <returns>The Tick Effect component that is applied to the player. If the Effect was already on the player, its duration is reset</returns>
    public Effect AddEffect(
        string effectName, bool isActive, bool isBuff, 
        Attribute attribute, float amount, float duration,
        float tickRate, float tickAmount
    ) {
        TickEffect effect = (TickEffect)GetEffect(effectName);
        if(effect != null) {
            // If the Effect already exists, reset its cooldown
            effect.Reset();
            return effect;
        } else {
            // Create a new TickEffect Effect and add it to the player
            TickEffect newEffect = gameObject.AddComponent<TickEffect>();
            newEffect.EffectName = effectName;
            newEffect.IsActive = isActive;
            newEffect.IsBuff = isBuff;
            newEffect.Attribute = attribute;
            newEffect.Amount = amount;
            newEffect.Duration = duration;
            newEffect.TickRate = tickRate;
            newEffect.TickAmount = tickAmount;
            newEffect.ApplyAtStart = Mathf.Abs(amount) > 0.0f;
            return newEffect;
        }
    }

    /// <summary>
    /// Check the player for an Effect by name
    /// </summary>
    /// <param name="effectName">The name of the Effect</param>
    /// <returns>The Effect component on the player or null if the player does not have the Effect</returns>
    public Effect GetEffect(string effectName) {
        Effect[] effects = GetComponents<Effect>();
        foreach(Effect effect in effects) {
            if(effectName == effect.EffectName) {
                return effect;
            }
        }
        return null;
    }

    /// <summary>
    /// Updates an Effect's active-ness on the player
    /// </summary>
    /// <param name="effectName">The name of the Effect</param>
    /// <param name="isActive">Whether the Effect should now be active</param>
    /// <returns>Whether the Effect exists on the player</returns>
    public bool UpdateEffect(string effectName, bool isActive) {
        Effect effect = GetEffect(effectName);
        if(effect != null) {
            effect.Toggle(isActive);
            return true;
        } else {
            return false;
        }
    }

    /// <summary>
    /// Remove an Effect from the player
    /// </summary>
    /// <param name="effectName">The name of the effect</param>
    /// <returns>Whether the Effect was on the player and successfully removed</returns>
    public bool RemoveEffect(string effectName) {
        Effect effect = GetEffect(effectName);
        if(effect != null) {
            Destroy(effect);
            return true;
        } else {
            return false;
        }
    }
}
