using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backButton : MonoBehaviour
{
    [SerializeField] GameObject CreditsMenu;
    public void goBack()
    {
        CreditsMenu.SetActive(false);
    }
}
