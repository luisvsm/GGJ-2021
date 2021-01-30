using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviourSingleton<RoomController>
{
    public List<GameObject> RoomList;
    public int startingRoom = 0;

    public static RoomController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("RoomController").GetComponent<RoomController>();
            }
            return _instance;
        }
    }
    private static RoomController _instance;
    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        showRoom(startingRoom);
    }

    public void showRoom(int roomIndex)
    {
        for (int i = 0; i < RoomList.Count; i++)
        {
            RoomList[i].SetActive(i == roomIndex);
        }

        MenuController.Instance.showMenu(roomIndex);
    }
}
