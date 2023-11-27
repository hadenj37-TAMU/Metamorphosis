using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClamp : MonoBehaviour
{

    [SerializeField]
    private Transform Carl4_Movement;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(Carl4_Movement.position.x, 13.5f, 94.0f),
            Mathf.Clamp(Carl4_Movement.position.y, 0.0f, 49.0f),
            transform.position.z);
    }
}
