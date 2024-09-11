using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FirePosition
{
    Up,
    Middle,
    Down
}

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private float fireRate, bulletSpeed, currentHealth, maxHealth, damage, bulletGravity, armor;
    [SerializeField]
    private GameObject bullet;

    private float fireTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
        GameObject newBullet = Fire(bullet, new Vector2(bulletSpeed, 0.0f), FirePosition.Middle, new Vector2(0.25f, 0.0f));
        if(newBullet != null)
            newBullet.GetComponent<Bullet>().damage = damage;
        return newBullet;
    }

    public GameObject Fire(GameObject bullet, Vector2 bulletSpeed, FirePosition origin, Vector2 extraOffset)
    {
        if(CanFire())
        {
            float facingDirection = gameObject.GetComponent<PlayerMovement>().FacingDirection;
            // Figure out the x-offset based on the direction of the player
            float offsetX = gameObject.transform.localScale.x / 2;
            offsetX += extraOffset.x;
            offsetX *= facingDirection;
            // Figure out the y-offset based on the fire position
            float offsetY = 0.0f;
            if(origin == FirePosition.Up)
            {
                offsetY = gameObject.transform.localScale.y / 2;
                offsetY += extraOffset.y;
            }
            else if(origin == FirePosition.Down)
            {
                offsetY = -gameObject.transform.localScale.y / 2;
                offsetY -= extraOffset.y;
            }
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
            newBullet.GetComponent<Rigidbody2D>().gravityScale = bulletGravity;
            // Reset the fire timer
            fireTimer = 0.0f;

            return newBullet;
        }

        return null;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount * (1 - armor);

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
