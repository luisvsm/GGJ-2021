using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMenus : MonoBehaviour
{
    void OnEnable()
    {
        MenuController.Instance.HideAllMenu();
    }
}
