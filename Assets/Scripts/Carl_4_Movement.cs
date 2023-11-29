using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carl4_Movement : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 4.0f;
    [SerializeField] public float swimSpeed = 2.0f;
    [SerializeField] public float jumpPower = 5.0f;
    [SerializeField] public float doubleJumpPower = 6.0f;
    [SerializeField] private float moveInput;
    private float xDir = 0.0f;
    private float yDir = 0.0f;
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Vector2 _respawnPoint;
    private bool doubleJump;
    private bool flyJump;
    private bool slowFall;
    float flightTime;
    float flyLimit;
    int jumpCount;
    [SerializeField] private LayerMask waterFalling;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask swimWater;
    [SerializeField] private bool _active = true;

    private enum MovementState {idle, walking, jumping, falling, swimming};

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        SetRespawnPoint(transform.position);
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

    private bool inWaterFall()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, 0.1f, waterFalling);
    }

    public void Die()
    {
        _active = false;
        coll.enabled = false;
        StartCoroutine(Respawn());
    }

    public void SetRespawnPoint(Vector2 position)
    {
        _respawnPoint = position;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        transform.position = _respawnPoint;
        _active = true;
        coll.enabled = true;
    }

    private void Update()
    {
        if (!_active)
        {
            return;
        }

        flightTime = 0.0f;
        flyLimit = 5.0f;
        xDir = Input.GetAxisRaw("Horizontal");
        yDir = Input.GetAxisRaw("Vertical");

        // Movement
        if (inWater())
        {
            rb.drag = 20.0f; //Carl needs to sink slower than he falls, or at minimum, slow down when hitting water
            rb.velocity = new Vector2(xDir * swimSpeed, yDir * swimSpeed);
            rb.gravityScale = 1.2f;
        }
        else if (inWaterFall())
        {
            rb.gravityScale = 10;
            rb.velocity = new Vector2(xDir * swimSpeed, yDir * swimSpeed);
        }
        else
        {
            rb.drag = 0.0f;
            rb.velocity = new Vector2(xDir * moveSpeed, rb.velocity.y);
            rb.gravityScale = 1.2f;
        }

        //Vertical Movement
        //Swimming
        if (Input.GetButton("Jump") && inWater())
        {
            rb.velocity = new Vector2(rb.velocity.x, swimSpeed);
        }
        //Waterfall
        if (Input.GetButton("Jump") && inWaterFall())
        {
            rb.velocity = new Vector2(rb.velocity.x, swimSpeed);
        }

        //Jump Management
        if (isGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
            flyJump = false;
            slowFall = false;
        }
        if (Input.GetButtonDown("Jump") && (isGrounded() || doubleJump))
        {
            rb.velocity = new Vector2(rb.velocity.x, doubleJump ? doubleJumpPower : jumpPower);
            doubleJump = !doubleJump;
            print("jump");

        }
        if (Input.GetButtonUp("Jump") && doubleJump == false)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            flyJump = !flyJump;
        }
        //Fly-jump mechanic
        if (Input.GetButton("Jump") && flyJump)
        {
            print("flying");
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
            flyJump = false;
            print("flyjump: " + flyJump);
        }
        //Drift mechanic
        if (Input.GetKey("space") && rb.velocity.y < 0)
        {
            fallDrift();
        }

        UpdateAnimationUpdate();
    }

    private void UpdateAnimationUpdate()
    {
        MovementState state;

        //swimming
        if (inWater() || inWaterFall())
        {
            state = MovementState.swimming;
            if (xDir > 0f)
            {
                sprite.flipX = true;
            }
            else if (xDir < 0f)
            {
                sprite.flipX = false;
            }
        }
        //walking
        else if (xDir > 0f && isGrounded())
        {
            state = MovementState.walking;
            sprite.flipX = true;
        }
        else if (xDir < 0f && isGrounded())
        {
            state = MovementState.walking;
            sprite.flipX = false;
        }
        //idle
        else
        {
            state = MovementState.idle;
        }

        //jumping/flying/gliding
        if (Input.GetKey("space") && !inWater() && !inWaterFall())
        {
            state = MovementState.jumping;
            if (xDir > 0f)
            {
                sprite.flipX = true;
            }
            else if (xDir < 0f)
            {
                sprite.flipX = false;
            }
        }
        else if (rb.velocity.y < -.1f || !isGrounded() && !inWater() && !inWaterFall())
        {
            state = MovementState.falling;
            if (xDir > 0f)
            {
                sprite.flipX = true;
            }
            else if (xDir < 0f)
            {
                sprite.flipX = false;
            }
        }

        anim.SetInteger("state", (int)state);
    }
}