using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnTap : MonoBehaviour
{
    public GameObject gameObjectToSetActive;
    void OnMouseDown()
    {
        Debug.Log("HideOnTap");
        gameObject.SetActive(false);
        AudioController.Play("SFX_Door_Open");

        gameObjectToSetActive.SetActive(true);
    }
}
