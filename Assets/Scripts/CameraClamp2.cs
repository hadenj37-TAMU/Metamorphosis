using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClamp2 : MonoBehaviour
{

    [SerializeField] private Transform Carl2_Movement;
    [SerializeField] private float xMax;
    [SerializeField] private float yMax;
    [SerializeField] private float xMin;
    [SerializeField] private float yMin;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(Carl2_Movement.position.x, xMin, xMax),
            Mathf.Clamp(Carl2_Movement.position.y, yMin, yMax),
            transform.position.z);
    }
}
