using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carl_sibling2_Movement : MonoBehaviour
{

    
    private Animator anim;
    [SerializeField] public bool jumping;
    [SerializeField] public bool grounded;
    

    private void Start()
    {
        
        anim = GetComponent<Animator>();
        anim.SetBool("inWater", false);
        anim.SetBool("moving", true);
    }

    
    

    private void Update()
    {
        
        UpdateAnimationUpdate();
    }

    private void UpdateAnimationUpdate()
    {
        
        
        anim.SetBool("jumping", jumping);
        anim.SetBool("grounded", grounded);
        
    }

}