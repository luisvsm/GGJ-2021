using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Plant manager is the bestest boii
public class PlantManager : MonoBehaviourSingleton<PlantManager>
{
    public List<GameObject> plantList;

    public void showAllPlants()
    {
        for (int i = 0; i < plantList.Count; i++)
        {
            plantList[i].SetActive(true);
        }
    }

    public void hideAllPlants()
    {
        for (int i = 0; i < plantList.Count; i++)
        {
            plantList[i].SetActive(false);
        }
    }

    public void showPlant(int plantIndex)
    {
        hideAllPlants();
        plantList[plantIndex].SetActive(true);
    }
}
