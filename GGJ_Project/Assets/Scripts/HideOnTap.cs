using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnTap : MonoBehaviour
{
    public GameObject gameObjectToSetActive;
    public GameObject gameObjectToHide;
    void OnMouseDown()
    {
        Debug.Log("HideOnTap");
        gameObject.SetActive(false);
        AudioController.Play("SFX_Door_Open");

        if (gameObjectToSetActive != null)
            gameObjectToSetActive.SetActive(true);

        if (gameObjectToHide != null)
            gameObjectToHide.SetActive(false);
    }
}
