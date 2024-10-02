using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FirePosition
{
    Head,
    Shoulder,
    Torso,
    Knees,
    Feet
}

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private Character characterName;
    [SerializeField]
    private float fireRate, bulletSpeed, currentHealth, maxHealth, damage, bulletGravity, armor;
    [SerializeField]
    private GameObject bullet;

    private float fireTimer;
    
    public Character CharacterName { get { return characterName; } }
    public float HealthPercentage { get { return currentHealth / maxHealth; } }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        fireTimer = fireRate;   // can fire right away
    }

    private void FixedUpdate()
    {
        fireTimer += Time.deltaTime;
    }

    private bool CanFire()
    {
        return fireTimer >= fireRate;
    }

    public GameObject FireBullet()
    {
        GameObject newBullet = Fire(bullet, new Vector2(bulletSpeed, 0.0f), FirePosition.Torso, new Vector2(0.25f, 0.0f));
        if(newBullet != null)
        {
            newBullet.GetComponent<Bullet>().damage = damage;
            fireTimer = 0.0f;
        }
        return newBullet;
    }

    public GameObject Fire(GameObject bullet, Vector2 bulletSpeed, FirePosition origin, Vector2 extraOffset, bool usesGravity = true)
    {
        if(CanFire())
        {
            float facingDirection = transform.parent.GetComponent<PlayerMovement>().FacingDirection;
            // Ensure facingDirection is either -1 or 1
            facingDirection /= Mathf.Abs(facingDirection);
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
            // Set the bullet's velocity
            Vector2 bulletSpeedWithDirection = bulletSpeed;
            bulletSpeedWithDirection.x *= facingDirection;
            newBullet.GetComponent<Rigidbody2D>().velocity = bulletSpeedWithDirection;
            float newBulletGravity = bulletGravity;
            if(usesGravity)
                newBulletGravity = 0.0f;
            newBullet.GetComponent<Rigidbody2D>().gravityScale = newBulletGravity;

            return newBullet;
        }

        return null;
    }

    private float GetOffsetYFromFireOrigin(GameObject gameObject, FirePosition origin, Vector2 extraOffset)
    {
        float offsetY = 0.0f;
        switch(origin)
        {
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

    public void TakeDamage(float amount)
    {
        currentHealth -= amount * (1 - armor);
        UIManager.instance.UpdatePlayerHealth();

        if(currentHealth <= 0.0f)
        {
            Debug.Log(gameObject.name + "Dead!");
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, currentHealth, maxHealth);
    }

    public void AdjustDamage(float percentage)
    {
        damage += damage * percentage;
    }

    public void AdjustArmor(float percentage)
    {
        armor += armor * percentage;
    }

    public void AdjustBulletGravity(float percentage)
    {
        bulletGravity += bulletGravity * percentage;
    }
}
