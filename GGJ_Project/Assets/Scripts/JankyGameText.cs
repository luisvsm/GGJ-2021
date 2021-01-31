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
        }
        else
        {
            RoomController.Instance.showRoom(3);
        }
    }
}
