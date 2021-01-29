using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPot : MonoBehaviour
{
    [SerializeField] private BasePlant _plant;
    
    //Set up here in case we want to decorate the pot and change out teh sprite
    [SerializeField] private SpriteRenderer _potSprite;

    public BasePlant GetPlant()
    {
        return _plant;
    }
}
