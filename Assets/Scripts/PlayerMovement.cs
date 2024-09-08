using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float moveSpeed, jumpSpeed, bulletSpeed, fireRate, facingDirection;
    [SerializeField]
    private InputActionReference move, fire, block;
    [SerializeField]
    private GameObject bullet;

    private Vector2 moveDirection;
    private bool grounded;
    private float fireTimer;

    void Start()
    {
        if(rb == null) rb = GetComponent<Rigidbody2D>();
        grounded = true;
        fireTimer = fireRate;   // can fire right away
    }

    private void Update()
    {
        GetMove();
        grounded = CheckGrounded();
    }

    void FixedUpdate()
    {
        float yMove = 0.0f;
        if(grounded && moveDirection.y > 0.0f)
        {
            yMove = jumpSpeed;
        }

        if(moveDirection.x != 0.0f)
            facingDirection = moveDirection.x;
        
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y + yMove);
        fireTimer += Time.deltaTime;
    }

    private void OnEnable()
    {
        fire.action.started += Fire;
        block.action.started += Block;
    }

    private void OnDisable()
    {
        fire.action.started -= Fire;
        block.action.started -= Block;
    }

    private void GetMove()
    {
        moveDirection = move.action.ReadValue<Vector2>();
    }

    private bool CheckGrounded()
    {
        return rb.velocity.y == 0.0f;
    }

    private bool CanFire()
    {
        return fireTimer >= fireRate;
    }

    private void Fire(InputAction.CallbackContext obj)
    {
        if(CanFire())
        {
            Vector3 bulletPos = new Vector3(
                transform.position.x + (gameObject.transform.localScale.x / 2 * facingDirection),
                transform.position.y,
                transform.position.z);
            GameObject newBullet = Instantiate(bullet, bulletPos, Quaternion.identity, GameManager.instance.gameObject.transform);
            GameManager.instance.bullets.Add(newBullet);
            newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(
                bulletSpeed * facingDirection, 0.0f);
            fireTimer = 0.0f;
        }
    }

    private void Block(InputAction.CallbackContext obj)
    {
        Debug.Log("Blocked");
    }
}
