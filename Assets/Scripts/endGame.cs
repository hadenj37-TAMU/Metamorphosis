using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endGame : MonoBehaviour
{
    public GameObject cutscene;
    public GameObject foodText;
    private bool gotFood = false;

    private void Update()
    {
        if (foodText.GetComponent<UnityEngine.UI.Text>().text == "Food: 10/10")
        {
            print("gotFood");
            gotFood = true;
            print(gotFood);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gotFood)
        {
                if (collision.gameObject.tag == "Player")
            {
                cutscene.SetActive(true);
                StartCoroutine(FinishCut());
            }
        }
    }

    IEnumerator FinishCut()
    {
        yield return new WaitForSeconds((float)15.50);
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 5);
    }
}
