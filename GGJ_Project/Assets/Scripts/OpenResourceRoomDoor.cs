using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenResourceRoomDoor : MonoBehaviour
{
    void OnMouseDown()
    {
        RoomController.Instance.showRoom(1);
    }
}
