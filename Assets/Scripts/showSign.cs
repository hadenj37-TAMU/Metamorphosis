using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showSign : MonoBehaviour
{

    public GameObject foodText;
    public GameObject textBox;

    // Update is called once per frame
    void Update()
    {
        if (foodText.GetComponent<UnityEngine.UI.Text>().text == "Food: 30/30")
        {
            textBox.SetActive(false);
        }
    }
}
