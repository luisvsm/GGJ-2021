using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviourSingleton<MenuController>
{
    public List<Animator> MenuListAnimators;
    private Animator prevAnimator;

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
    }

    public void closeMenu()
    {
        if (prevAnimator != null)
            prevAnimator.SetTrigger("Close");
    }

    public void showMenu(int menuIndex)
    {
        if (MenuListAnimators[menuIndex] != prevAnimator)
        {
            closeMenu();
            MenuListAnimators[menuIndex].SetTrigger("Open");
            prevAnimator = MenuListAnimators[menuIndex];
        }
    }
}
