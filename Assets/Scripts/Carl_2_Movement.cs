using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carl2_Movement : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 4.0f;
    [SerializeField] public float swimSpeed = 2.0f;
    [SerializeField] public float jumpPower = 5.0f;
    private float xDir = 0.0f;
    private float yDir = 0.0f;
    private Rigidbody2D rb;
    private Animator anim;
    private PolygonCollider2D coll;
    private SpriteRenderer sprite;
    private Transform trans;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask swimWater;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<PolygonCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
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
            rb.drag = 10.0f; //Carl needs to sink slower than he falls, or at least, slow down when hitting water
            
            // swim fast if input, otherwise sink
            if(yDir == 0.0f && xDir == 0.0f) 
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            else 
                rb.velocity = new Vector2(xDir * swimSpeed, yDir * swimSpeed);

            // jump button in water swims up, too
            if (Input.GetButton("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, swimSpeed);
            }
        }
        else if (isGrounded())
        {
            // Initiate jump
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }

            rb.drag = 0.0f;
            rb.velocity = new Vector2(xDir * moveSpeed, rb.velocity.y);
        }
        else //in air
        {
            if (Input.GetButtonUp("Jump"))
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f);
            }

            rb.drag = 0.0f;
            rb.velocity = new Vector2(xDir * moveSpeed, rb.velocity.y);
        }

        UpdateAnimationUpdate();
    }

    private void UpdateAnimationUpdate()
    {
        if (xDir > 0f)
        {
            trans.localScale = new Vector3(-1.0f,1.0f,1.0f);
            anim.SetBool("moving", true);
        }
        else if (xDir < 0f)
        {
            trans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            anim.SetBool("moving", true);
        }
        else 
        {
            if (yDir == 0f) { anim.SetBool("moving", false); }
            else { anim.SetBool("moving", true); }
        }

        anim.SetBool("jumping", Input.GetButton("Jump"));
        anim.SetBool("grounded", isGrounded());
        anim.SetBool("inWater", inWater());
    }

}