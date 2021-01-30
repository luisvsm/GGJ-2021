using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOutsideDoor : MonoBehaviour
{
    public void OpenDoor()
    {
        RoomController.Instance.showRoom(3);
    }
}
