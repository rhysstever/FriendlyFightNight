using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireAngle
{
    Up,
    Middle,
    Down
}

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private float fireRate, bulletSpeed;
    [SerializeField]
    private GameObject bullet;

    private float health;
    private float fireTimer;

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

    public GameObject Fire(FireAngle angle)
    {
        return Fire(bullet, new Vector2(bulletSpeed, 0.0f), angle, 0.25f);
    }

    public GameObject Fire(GameObject bullet, Vector2 bulletSpeed, FireAngle angle, float extraOffset)
    {
        if(CanFire())
        {
            float facingDirection = gameObject.GetComponent<PlayerMovement>().FacingDirection;
            float offset = gameObject.transform.localScale.x / 2;
            offset += extraOffset;
            offset *= facingDirection;
            
            Vector3 bulletPos = new Vector3(
                transform.position.x + offset,
                transform.position.y,
                transform.position.z);
            GameObject newBullet = Instantiate(bullet, bulletPos, Quaternion.identity, GameManager.instance.Bullets.transform);
            
            Vector2 bulletSpeedWithDirection = bulletSpeed;
            bulletSpeedWithDirection.x *= facingDirection;
            newBullet.GetComponent<Rigidbody2D>().velocity = bulletSpeedWithDirection;
            
            fireTimer = 0.0f;

            return newBullet;
        }

        return null;
    }
}
