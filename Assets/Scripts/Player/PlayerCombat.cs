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

    private SpecialAbility passiveAbility, activeAbility;
    private float fireTimer;

    public Character Character { get { return character; } }
    public float HealthPercentage { get { return currentHealth / maxHealth; } }
    public SpecialAbility PassiveAbility { get { return passiveAbility; } }
    public SpecialAbility ActiveAbility { get { return activeAbility; } }

    private void Awake() {
        currentHealth = maxHealth;

        SpecialAbility[] specialAbilities = GetComponents<SpecialAbility>();
        foreach(SpecialAbility specialAbility in specialAbilities) {
            if(specialAbility.IsPassive)
                passiveAbility = specialAbility;
            else 
                activeAbility = specialAbility;
        }
    }

    // Start is called before the first frame update
    void Start() {
        fireTimer = fireRate;   // can fire right away
        damageMod = 0.0f;
        bulletGravityMod = 0.0f;
        armorMod = 0.0f;
    }

    private void Update() {

    }

    private void FixedUpdate() {
        fireTimer += Time.deltaTime;
    }

    private bool CanFire() {
        return fireTimer >= fireRate;
    }

    public GameObject FireBullet() {
        GameObject newBullet = Fire(bullet, new Vector2(bulletSpeed, 0.0f), FirePosition.Torso, new Vector2(0.25f, 0.0f));
        if(newBullet != null) {
            newBullet.GetComponent<Bullet>().damage = damage * damageMod;
            newBullet.GetComponent<Bullet>().source = gameObject;
            fireTimer = 0.0f;
        }
        return newBullet;
    }

    public GameObject Fire(GameObject bullet, Vector2 bulletSpeed, FirePosition origin, Vector2 extraOffset, bool usesGravity = true) {
        if(CanFire()) {
            float facingDirection = transform.parent.GetComponent<PlayerMovement>().FacingDirection;
            // Figure out the x-offset based on the direction of the player
            float offsetX = gameObject.transform.localScale.x / 2;
            offsetX += extraOffset.x;
            offsetX *= facingDirection;
            // Figure out the y-offset based on the fire position
            float offsetY = GetOffsetYFromFireOrigin(gameObject, origin, extraOffset);
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
            Debug.Log(newBulletGravity);
            newBullet.GetComponent<Rigidbody2D>().gravityScale = newBulletGravity;

            return newBullet;
        }

        return null;
    }

    private float GetOffsetYFromFireOrigin(GameObject gameObject, FirePosition origin, Vector2 extraOffset) {
        float offsetY = 0.0f;
        switch(origin) {
            case FirePosition.Head:
                offsetY = gameObject.transform.localScale.y / 2;
                offsetY += extraOffset.y;
                break;
            case FirePosition.Shoulder:
                offsetY = gameObject.transform.localScale.y / 4;
                offsetY += extraOffset.y;
                break;
            case FirePosition.Torso:
                offsetY += extraOffset.y;
                break;
            case FirePosition.Knees:
                offsetY = -gameObject.transform.localScale.y / 4;
                offsetY -= extraOffset.y;
                break;
            case FirePosition.Feet:
                offsetY = -gameObject.transform.localScale.y / 2;
                offsetY -= extraOffset.y;
                break;
        }
        return offsetY;
    }

    public void TakeDamage(float amount) {
        float damageTaken = amount * (2 - armor * armorMod);
        currentHealth -= damageTaken;
        UIManager.instance.UpdatePlayerHealth();

        if(currentHealth <= 0.0f) {
            Debug.Log(gameObject.name + "Dead!");
        }
    }

    public void Heal(float amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, currentHealth, maxHealth);
    }

    public void AdjustArmor(float percentage) {
        armorMod += armor * percentage;
    }

    public void AdjustBulletGravity(float percentage) {
        bulletGravityMod += bulletGravity * percentage;
    }

    public void AdjustDamage(float percentage) {
        damageMod += damage * percentage;
    }
}
