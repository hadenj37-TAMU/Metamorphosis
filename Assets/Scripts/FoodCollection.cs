using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodCollection : MonoBehaviour
{
    private int collected = 0;

    [SerializeField] private Text FoodText;
    [SerializeField] public GameObject nextForm;
    public AudioClip playFoodSound;
    public AudioSource myAudioSource;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void Mature() 
    {
        collected = 0;
        FoodText.text = "Food: " + collected + "/10";

        GameObject newModel = Instantiate(nextForm, transform.position, transform.rotation);
        gameObject.transform.SetParent(newModel.transform);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            //play collection sound
            myAudioSource.PlayOneShot(playFoodSound, 1);

            collision.gameObject.SetActive(false);
            collected++;
            FoodText.text = "Food: " + collected + "/10";
            if (collected == 10) {
                Invoke("Mature", 0.5f);
            }
        }
    }
}
