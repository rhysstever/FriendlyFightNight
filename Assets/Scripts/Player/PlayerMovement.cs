using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField]
    private float moveSpeed, jumpSpeed, facingDirection;

    [SerializeField]
    private float moveSpeedMod;

    private Vector2 moveDirection;
    private bool grounded;

    private Rigidbody2D rb;
    private PlayerInputControls input;
    private Animator animator;

    public float FacingDirection { get { return facingDirection; } }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputControls>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        grounded = true;
        moveSpeedMod = 1.0f;
    }

    void Update() {
        moveDirection = input.GetMove();

        if(GameManager.instance.CurrentMenuState == MenuState.Game) {
            if(moveDirection.x != 0.0f) {
                // Ensure facing direction is either -1 or 1
                facingDirection = moveDirection.x;
                facingDirection /= Mathf.Abs(facingDirection);
                // Flip the sprite based on the facing direction
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = facingDirection < 0.0f;
            }

            animator.SetBool("canMove", rb.velocity.x != 0.0f);
            if(rb.velocity.x != 0.0f) {
                animator.speed = Mathf.Abs(moveDirection.x);
            } else {
                animator.speed = 1.0f;
            }
            grounded = CheckGrounded();
        }
    }

    void FixedUpdate() {
        if(GameManager.instance.CurrentMenuState == MenuState.CharacterSelect) {
            int playerNum = PlayerManager.instance.GetPlayerNum(gameObject);
            if(playerNum >= 0) {
                CharacterSelectManager.instance.UpdateCharacterSelection(moveDirection, playerNum);
            }
        } else if(GameManager.instance.CurrentMenuState == MenuState.Game) {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * moveSpeedMod, rb.velocity.y);
            animator.SetFloat("yVelocity", rb.velocity.y);
        }
    }

    /// <summary>
    /// Checks if the player is grounded on a wall or another player
    /// </summary>
    /// <returns>Whether the player is on a jumpable surface</returns>
    private bool CheckGrounded() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            new Vector2(
                gameObject.transform.position.x,
                gameObject.transform.position.y - gameObject.transform.localScale.y / 2
            ),
            Vector2.down,
            0.75f);

        foreach(RaycastHit2D hit in hits) {
            // If the raycast hits a different object with a rigidbody
            if(hit.rigidbody != null && hit.rigidbody != rb) {

                // If the raycast hits a wall or player
                if(hit.rigidbody.gameObject.layer == 6 ||
                    hit.rigidbody.gameObject.layer == 8) {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Have the player jump up in the air
    /// </summary>
    public void Jump() {
        if(grounded) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpSpeed);
        }
    }

    /// <summary>
    /// Change the player's move speed modifier
    /// </summary>
    /// <param name="amount">The amount change</param>
    public void AdjustMoveSpeed(float amount) {
        moveSpeedMod += amount;
    }

    /// <summary>
    /// Reset the modifier for the player's move speed
    /// </summary>
    public void ResetMoveSpeedMod() {
        moveSpeedMod = 1.0f;
    }

    /// <summary>
    /// Set a new animator for the player object
    /// </summary>
    /// <param name="animator">An animator component</param>
    public void SetNewAnimator(Animator animator) {
        this.animator = animator;
    }
}
