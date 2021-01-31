using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlants : MonoBehaviour
{
    void OnEnable()
    {
        PlantManager.Instance.ShowAllPlants();
    }

    void OnDisable()
    {
        PlantManager.Instance.HideAllPlants();
    }
}
