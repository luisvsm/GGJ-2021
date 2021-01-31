﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PooHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    // Start is called before the first frame update
    void OnEnable()
    {
        _text.text = string.Format("{0}",PlayerInventoryMonoSingleton.Instance.Poo);
        PlayerInventoryMonoSingleton.Instance.OnPooUpdated += OnWaterUpdated;
    }

    private void OnWaterUpdated(int newvalue)
    {
        _text.text = string.Format("{0}", newvalue);
    }

    // Update is called once per frame
    void OnDisable()
    {
        PlayerInventoryMonoSingleton.Instance.OnPooUpdated -= OnWaterUpdated; 
    }
}
