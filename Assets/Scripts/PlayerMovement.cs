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
        if(rb == null) rb = GetComponent<Rigidbody2D>();
        grounded = true;
        input = GetComponent<PlayerInputControls>();
    }

    void Update()
    {
        moveDirection = input.GetMove();
        grounded = CheckGrounded();
    }

    void FixedUpdate()
    {
        if(moveDirection.x != 0.0f)
            facingDirection = moveDirection.x;
        
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
}
