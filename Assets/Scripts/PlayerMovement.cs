using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    [SerializeField] private Animator anim;
    private SpriteRenderer sprite;
    private float dirX = 0.0f;
    private float dirY = 0.0f;
    private bool doubleJump;
    private float doubleJumpingPower = 12f;
     private float jumpingPower = 16f;
     [SerializeField] public float swimSpeed = 4.0f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask swimWater;
    [SerializeField] private LayerMask rapidWater;
    // Start is called before the first frame update
    private bool isJumping;
    private bool isFalling;
    [SerializeField] private float rapidSpeed = 2.0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
         dirX = Input.GetAxisRaw("Horizontal");
         dirY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);

       if (inWater() || inRapid())
        {
            // Swimming in water
            rb.drag = 10.0f; // Adjust drag for water physics
            if (inRapid())
            {
                // Move player to the left slowly when in rapids
                rb.velocity = new Vector2(-rapidSpeed, rb.velocity.y);
            }

            if (dirY == 0.0f && dirX == 0.0f)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            else
                rb.velocity = new Vector2(dirX * swimSpeed, dirY * swimSpeed);
        }
        else
        {
            // Standard jumping on non-water ground
            rb.drag = 0.0f;

            if (IsGrounded() && !Input.GetButton("Jump"))
            {
                doubleJump = false;
            }

            if (Input.GetButtonDown("Jump") && (IsGrounded() || doubleJump))
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJump ? doubleJumpingPower : jumpingPower);
                doubleJump = !doubleJump;
                isJumping = true;
            }

            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }

        UpdateAnimationUpdate();
        
    }

    private bool IsGrounded(){
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);


    }

    private bool inWater()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, 0.1f, swimWater);
    }

    private bool inRapid()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, 0.1f, rapidWater);
    }


    private void UpdateAnimationUpdate()
    {
        anim.SetBool("IsJumping", isJumping);
        anim.SetBool("IsFalling", isFalling);
        if (dirX > 0f)
        {
            sprite.flipX = true;
        }
        else if (dirX < 0f)
        {
            sprite.flipX = false;
        }
        else if (rb.velocity.y < -.1f || !IsGrounded())
        {
            isFalling = true;
            if(dirX > 0f)
            {
                sprite.flipX = true;
            }
            else if (dirX < 0f)
            {
                sprite.flipX = false;
            }
        }
        else
        {
            // Player is not moving horizontally, set IsJumping to false
            isJumping = false;
            isFalling = false;
        }
    
    }
}
