using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carl2_Movement : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 4.0f;
    [SerializeField] public float swimSpeed = 2.0f;
    [SerializeField] public float jumpPower = 5.0f;
    [SerializeField] public float doubleJumpPower = 6.0f;
    [SerializeField] private float moveInput;
    private float xDir = 0.0f;
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private bool doubleJump;
    private bool flyJump;
    private bool slowFall;
    float flightTime;
    float flyLimit;
    int jumpCount;
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

    private void fallDrift()
    {
        rb.drag = 15;
    }

    private bool inWater()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, 0.1f, swimWater);
    }

    private void Update()
    {
        flightTime = 0.0f;
        flyLimit = 5.0f;
        xDir = Input.GetAxisRaw("Horizontal");

        // Horizontal movement
        if (inWater())
        {
            rb.drag = 20.0f; //Carl needs to sink slower than he falls, or at minimum, slow down when hitting water
            rb.velocity = new Vector2(xDir * swimSpeed, rb.velocity.y);
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
        if (isGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
            flyJump = false;
            slowFall = false;
            jumpCount = 0;
        }
        if (Input.GetButtonDown("Jump") && (isGrounded() || doubleJump))
        {
            rb.velocity = new Vector2(rb.velocity.x, doubleJump ? doubleJumpPower : jumpPower);
            doubleJump = !doubleJump;
            jumpCount += 1;

        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            if (doubleJump == false)
            {
                flyJump = !flyJump;
            }
        }
        //Fly-jump mechanic
        if (Input.GetButton("Jump") && jumpCount == 2 & flyJump)
        {
            float jumpDrag = 15;
            while (flightTime < flyLimit)
            {
                float x = flightTime;
                double charUp = 0.092 * System.Math.Pow(x, 2) * (9.0 - x);
                rb.velocity = new Vector2(rb.velocity.x, (flightTime * System.Convert.ToSingle(charUp) / 5f));
                flightTime += Time.deltaTime;
                if (flightTime < 1.5)
                {
                    rb.drag = 50;
                }
                else if (flightTime > 4.5)
                {
                    rb.drag = 30;
                }
                else
                {
                    rb.drag = jumpDrag;
                }
            }
            jumpCount += 1;
            if (flightTime >= flyLimit)
            {
                flyJump = false;
                slowFall = !slowFall;
            }
        }
        //Drift mechanic, only available after fly-jump
        if (Input.GetKey("space") && rb.velocity.y < 0 && slowFall)
        {
            fallDrift();
        }
        slowFall = !slowFall;

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