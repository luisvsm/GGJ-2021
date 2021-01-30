using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlants : MonoBehaviour
{
    void OnEnable()
    {
        PlantManager.Instance.showAllPlants();
    }

    void OnDisable()
    {
        PlantManager.Instance.hideAllPlants();
    }
}
