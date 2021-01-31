using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantRoom : MonoBehaviour
{
    private BasePlant _selectedPlant;

    private Vector3 _initialPosition;

    private Vector3 _initialScale;

    private Vector3 _viewScale = new Vector3(0.4f, 0.4f, 1f);
    
    // Start is called before the first frame update
    void OnEnable()
    {
       _selectedPlant= PlantManager.Instance.GetSelectedPlant();
       var plantGO = _selectedPlant.gameObject;
       _initialPosition = plantGO.transform.position;
       _initialScale = plantGO.transform.localScale;
       plantGO.transform.position = PlantManager.Instance.PlantViewPosition.position;
       plantGO.transform.localScale = _viewScale;
    }

    private void OnDisable()
    {
        var plantGO = _selectedPlant.gameObject;
        plantGO.transform.localScale = _initialScale;
        plantGO.transform.position = _initialPosition;
    }


}
