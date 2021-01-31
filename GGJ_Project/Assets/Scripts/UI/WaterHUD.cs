using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaterHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    // Start is called before the first frame update
    void OnEnable()
    {
        _text.text = string.Format("{0}",PlayerInventoryMonoSingleton.Instance.Water);
        PlayerInventoryMonoSingleton.Instance.OnWaterUpdated += OnWaterUpdated;
    }

    private void OnWaterUpdated(int newvalue)
    {
        _text.text = string.Format("{0}", newvalue);
    }

    // Update is called once per frame
    void OnDisable()
    {
        PlayerInventoryMonoSingleton.Instance.OnWaterUpdated -= OnWaterUpdated; 
    }
}
