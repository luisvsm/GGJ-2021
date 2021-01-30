using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShelfRoomDoor : MonoBehaviour
{
    void OnMouseDown()
    {
        RoomController.Instance.showRoom(2);
    }
}
