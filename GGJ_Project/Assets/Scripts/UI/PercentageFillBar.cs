using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageFillBar : MonoBehaviour
{
    //TODO: Change bar colour when health drops
    [SerializeField] private Image _bar;
    //TODO:Change icon when health drops
    [SerializeField] private Image _icon;
    private float _currentPercentage; 
    //private float _toPercentage;
    
    public void Initialize(float startingPercentage)
    {
        _currentPercentage = startingPercentage;
        _bar.type = Image.Type.Filled;
        _bar.fillMethod = Image.FillMethod.Horizontal;
        _bar.fillAmount = startingPercentage;
    }

    public void UpdateBar(float newPercentage)
    {
        _currentPercentage = newPercentage;
        //TODO:Lerp Animation
        _bar.fillAmount = newPercentage;
    }
}
