using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float moveSpeed, jumpSpeed, facingDirection;

    private Vector2 moveDirection;
    private bool grounded;
    private PlayerInputControls input;

    public float FacingDirection { get { return facingDirection; } }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputControls>();
        grounded = true;
    }

    void Update()
    {
        moveDirection = input.GetMove();
        grounded = CheckGrounded();
    }

    void FixedUpdate()
    {
        if(moveDirection.x != 0.0f)
        {
            facingDirection = moveDirection.x;
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = facingDirection < 0.0f;
        }
        
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
    }

    private bool CheckGrounded()
    {
        return rb.velocity.y == 0.0f;
    }
    
    public void Jump()
    {
        if(grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpSpeed);
        }
    }

    public void AdjustMoveSpeed(float percentage)
    {
        moveSpeed += moveSpeed * percentage;
    }
}
