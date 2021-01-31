using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
//using Data.Util;
using UnityEngine;

public class PlayerInventoryMonoSingleton : MonoBehaviourSingleton<PlayerInventoryMonoSingleton>
{
    public delegate void UpdatePlayerInventory(int newValue);
    public UpdatePlayerInventory OnWaterUpdated;
    public UpdatePlayerInventory OnPooUpdated;
    
    public delegate void UpdatePlayerInventoryDecorationCount(string name, int newValue);
    public UpdatePlayerInventoryDecorationCount OnDecorationCountUpdated;
    
    private int _water = 10;
    private int _maxWater = 50;
    private int _poo = 10;
    private int _maxPoo = 10;
    private Dictionary<string, int> _unassignedDecorations;

    public int Water => _water;
    public int MaxWater => _maxWater;
    public int Poo => _poo;
    public int MaxPoo => _maxPoo;
    public Dictionary<string, int> Decorations => _unassignedDecorations;

    public void CollectWater(int amount =1)
    {
        _water += amount;
        _water = Mathf.Min(_water, _maxWater);
        OnWaterUpdated?.Invoke(_water);
    }
    
    public bool UseWater(int amount =1)
    {
        if (_water >= amount)
        {
            _water -= amount;
            OnWaterUpdated?.Invoke(_water);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void CollectPoo(int amount =1)
    {
        _poo += amount;
        _poo = Mathf.Min(_poo, _maxPoo);
        OnPooUpdated?.Invoke(_poo);
    }
    
    public bool UsePoo(int amount =1)
    {
        if (_poo >= amount)
        {
            _poo -= amount;
            OnPooUpdated?.Invoke(_poo);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    
    public void CollectDecoration(string decoName)
    {
        if (_unassignedDecorations == null)
        {
            _unassignedDecorations = new Dictionary<string, int>();
        }

        if (_unassignedDecorations.ContainsKey(decoName))
        {
            int value = _unassignedDecorations[decoName];
            _unassignedDecorations[decoName] = value + 1;
            OnDecorationCountUpdated?.Invoke(decoName, _unassignedDecorations[decoName]);
        }
        else
        {
            _unassignedDecorations.Add(decoName,1);
            OnDecorationCountUpdated?.Invoke(decoName, _unassignedDecorations[decoName]);
        }
    }
    
    public void UseDecoration(string decoName)
    {
        if (_unassignedDecorations == null)
        {
            _unassignedDecorations = new Dictionary<string, int>();
        }

        if (_unassignedDecorations.ContainsKey(decoName))
        {
            int value = _unassignedDecorations[decoName];
            if (value > 0)
            {
                _unassignedDecorations[decoName] = value - 1;
                OnDecorationCountUpdated?.Invoke(decoName, _unassignedDecorations[decoName]);
            }
        }
        else
        {
            Debug.Log(string.Format("<color=red>OH NOES!!! Cannot find decoration {0} in game inventory </color>", decoName));
        }
    }


    public bool HasDecoration(string decoName)
    {
        if (_unassignedDecorations == null)
        {
            _unassignedDecorations = new Dictionary<string, int>();
        }

        if (_unassignedDecorations.ContainsKey(decoName))
        {
            int value = _unassignedDecorations[decoName];
            if (value > 0)
            {
                return true;
            }
        }

        return false;
    }
    
}
