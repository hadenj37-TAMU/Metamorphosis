using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditsButton : MonoBehaviour
{

    [SerializeField] GameObject creditsMenu;

    public static bool creditsOpen = false;

    public void getCredits()
    {
        creditsMenu.SetActive(true);
        creditsOpen = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        creditsMenu.SetActive(false);
    }
}
