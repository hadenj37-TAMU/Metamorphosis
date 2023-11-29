using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carl_4_CutsceneControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    [SerializeField] private int animationState = 0;

    private enum MovementState { idle, walking, jumping, falling, swimming };

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        

        UpdateAnimationUpdate();
    }

    private void UpdateAnimationUpdate()
    {
        MovementState state;

        //swimming
        if (animationState == 4)
        {
            state = MovementState.swimming;
        }
        //walking
        else if (animationState == 1)
        {
            state = MovementState.walking;
            sprite.flipX = true;
        }
        //idle
        else
        {
            state = MovementState.idle;
        }

        //jumping/flying/gliding
        if (animationState == 2)
        {
            state = MovementState.jumping;
        }
        else if (animationState == 3)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }
}
