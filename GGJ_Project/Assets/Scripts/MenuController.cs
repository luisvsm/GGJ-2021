using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviourSingleton<MenuController>
{
    public List<Animator> MenuListAnimators;

    public void showMenu(int menuIndex)
    {
        for (int i = 0; i < MenuListAnimators.Count; i++)
        {
            MenuListAnimators[i].SetBool("open", menuIndex==i);
        }
    }
}
