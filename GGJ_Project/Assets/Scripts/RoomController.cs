using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviourSingleton<RoomController>
{
    public List<GameObject> RoomList;
    public int startingRoom = 0;

    // Start is called before the first frame update
    void Start()
    {
        showRoom(startingRoom);
    }

    public void showRoom(int roomIndex)
    {
        MenuController.Instance.closeMenu();

        FadeController.Instance.Fade(() =>
        {
            for (int i = 0; i < RoomList.Count; i++)
            {
                RoomList[i].SetActive(i == roomIndex);
            }
            Debug.Log("HalfWay");
        },
        () =>
        {
            Debug.Log("Complted");
            MenuController.Instance.showMenu(roomIndex);
        });
    }
}
