using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviourSingleton<MenuController>
{
    public List<GameObject> MenuList;
    public int startingRoom = 0;

    public static MenuController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("MenuController").GetComponent<MenuController>();
            }
            return _instance;
        }
    }
    private static MenuController _instance;
    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        showMenu(startingRoom);
    }

    public void showMenu(int menuIndex)
    {
        for (int i = 0; i < MenuList.Count; i++)
        {
            MenuList[i].SetActive(i == menuIndex);
        }
    }
}
