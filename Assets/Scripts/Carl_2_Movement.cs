using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carl2_Movement : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 4.0f;
    [SerializeField] public float swimSpeed = 2.0f;
    [SerializeField] public float jumpPower = 5.0f;
    [SerializeField] private float moveInput;
    private float xDir = 0.0f;
    private float yDir = 0.0f;
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask swimWater;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private bool inWater()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, 0.1f, swimWater);
    }

    private void Update()
    {
        xDir = Input.GetAxisRaw("Horizontal");
        yDir = Input.GetAxisRaw("Vertical");
        // Movement
        if (inWater())
        {
            rb.drag = 15.0f; //Carl needs to sink slower than he falls, or at minimum, slow down when hitting water
            rb.velocity = new Vector2(xDir * swimSpeed, yDir * swimSpeed);
        }
        else
        {
            rb.drag = 0.0f;
            rb.velocity = new Vector2(xDir * moveSpeed, rb.velocity.y);
        }

        //Vertical Movement
        //Swimming
        if (Input.GetButton("Jump") && inWater())
        {
            rb.velocity = new Vector2(rb.velocity.x, swimSpeed);
        }

        //Jump Management
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        UpdateAnimationUpdate();
    }

    private void UpdateAnimationUpdate()
    {
        if (xDir > 0f)
        {
            sprite.flipX = true;
        }
        else if (xDir < 0f)
        {
            sprite.flipX = false;
        }
    }

}