using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;
    private bool doubleJump;
    private float doubleJumpingPower = 12f;
     private float jumpingPower = 16f;
     [SerializeField] public float swimSpeed = 2.0f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask swimWater;
    // Start is called before the first frame update
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
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);

        if(inWater()){
            rb.drag = 10.0f; //Carl needs to sink slower than he falls, or at least, slow down when hitting water
            if(dirY == 0.0f && dirX == 0.0f) 
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            else 
                rb.velocity = new Vector2(dirX * swimSpeed, dirY * swimSpeed);

            // jump button in water swims up, too
            if (Input.GetButton("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, swimSpeed);
            }
        }
        if(IsGrounded() && !Input.GetButton("Jump")){
            doubleJump = false;
        }

        if(Input.GetButtonDown("Jump") && (IsGrounded() || doubleJump)){
            rb.velocity = new Vector3(rb.velocity.x, doubleJump ? doubleJumpingPower : jumpingPower);
            doubleJump = !doubleJump;
        }

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f){
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        
    }

    private bool IsGrounded(){
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);

    }

    private bool inWater()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, 0.1f, swimWater);
    }
}
