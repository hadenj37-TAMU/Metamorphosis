using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodCollection : MonoBehaviour
{
    private int collected = 0;

    [SerializeField] private Text FoodText;
    [SerializeField] public GameObject nextForm;

    private void Mature() 
    {
        GameObject newModel = Instantiate(nextForm, transform.position, transform.rotation);
        Transform cam = transform.GetChild(0);
        collected = 0;
        FoodText.text = "Food: " + collected + "/10";
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
            collected++;
            FoodText.text = "Food: " + collected + "/10";
            if (collected == 10) {
                Invoke("Mature", 2f);
            }
        }
    }
}
