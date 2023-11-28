using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTriggerCarl3 : MonoBehaviour
{
    private PlayableDirector timeline;
    private Rigidbody2D rb;
    private bool hasTriggered = false;

    private void Start()
    {
        timeline = GetComponent<PlayableDirector>();
        if (timeline == null)
        {
            Debug.LogError("PlayableDirector component not found on the same GameObject.");
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the same GameObject.");
        }
        else
        {
            rb.sharedMaterial = new PhysicsMaterial2D { friction = 0.001f };
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && timeline && !hasTriggered)
        {
            timeline.Play();
            hasTriggered = true;
        }
    }
}
