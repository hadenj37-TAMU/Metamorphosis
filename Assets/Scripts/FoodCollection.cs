using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodCollection : MonoBehaviour
{
    private int collected = 0;

    [SerializeField] private Text FoodText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        { 
            Destroy(collision.gameObject);
            collected++;
            FoodText.text = "Food: " + collected + "/10";
        }
    }
}
