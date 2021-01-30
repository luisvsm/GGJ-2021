using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOutsideDoor : MonoBehaviour
{
    public void OpenDoor()
    {
        Debug.Log("Playing Audio");
        AudioController.Play("SFX_Door_Open");
        RoomController.Instance.showRoom(3);
    }
}
