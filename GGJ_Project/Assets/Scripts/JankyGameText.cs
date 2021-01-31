using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JankyGameText : MonoBehaviour
{
    public GameObject nextGameText;

    public void OnMouseDown()
    {
        if (nextGameText != null)
        {
            nextGameText.SetActive(true);
            gameObject.SetActive(false);
			AudioController.Play("SFX_Generic_Tap");
        }
        else
        {
            RoomController.Instance.showRoom(3);
			AudioController.Play("SFX_Generic_Tap");
		}
    }
}
