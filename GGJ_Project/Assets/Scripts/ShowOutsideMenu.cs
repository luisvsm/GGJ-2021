using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOutsideMenu : MonoBehaviour
{
    void OnEnable()
    {
        MenuController.Instance.showMenu(0);
    }
}
