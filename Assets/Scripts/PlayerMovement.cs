using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private bool doubleJump;
    private float doubleJumpingPower = 12f;
     private float jumpingPower = 16f;
    [SerializeField] private LayerMask jumpableGround;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);

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
}
