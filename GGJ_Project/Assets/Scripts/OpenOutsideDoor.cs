using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOutsideDoor : MonoBehaviour
{
    public GameObject padlock;
    public GameObject wood;
    public Collider2D doorCollider;
    public void UnlockDoor()
    {
        AudioController.Play("SFX_Door_Unlock");
        wood.SetActive(false);
        padlock.SetActive(false);
        doorCollider.enabled = true;
    }

}
