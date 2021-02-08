using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviourSingleton<RoomController>
{
    public delegate void RoomChanged(int room);
    public RoomChanged OnRoomChanged;

    public List<GameObject> RoomList;
    public int startingRoom = 0;
    public int greenhouseRoom = 1;
    public int plantShelfRoom = 2;
    public int plantViewRoom = 3;
    // Start is called before the first frame update

    private int _currentRoom = 0;
    void Start()
    {
        showRoom(startingRoom, true);
    }

    public void showRoom(int roomIndex, bool strartingRoom = false)
    {
        bool roomChanged = false;
        if (!strartingRoom)
            AudioController.Play("SFX_Generic_Transition");

        FadeController.Instance.Fade(() =>
        {
            for (int i = 0; i < RoomList.Count; i++)
            {
                RoomList[i].SetActive(i == roomIndex);
                if (i == roomIndex)
                {
                    if (_currentRoom != i)
                    {
                        roomChanged = true;
                    }
                    _currentRoom = i;
                }
            }
            if (!strartingRoom)
                MenuController.Instance.showMenu(roomIndex);
        },
        () =>
        {
            if (roomChanged)
            {
                OnRoomChanged?.Invoke(roomIndex);
            }
        });
    }

    public bool CanGoBack()
    {
        return _currentRoom > 1;
    }

    public void GoBack()
    {
        if (_currentRoom > 1)
        {
            showRoom(_currentRoom - 1);
        }
    }

    public void ShowPlantRoom()
    {
        showRoom(plantViewRoom);
    }

    public void ShowPlantShelfRoom()
    {
        showRoom(plantShelfRoom);
    }
}
