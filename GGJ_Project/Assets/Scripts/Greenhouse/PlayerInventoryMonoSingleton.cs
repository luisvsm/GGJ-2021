using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryMonoSingleton : MonoBehaviourSingleton<PlayerInventoryMonoSingleton>
{
    public delegate void UpdatePlayerInventory(int newValue);
    public UpdatePlayerInventory OnWaterUpdated;
    public UpdatePlayerInventory OnPooUpdated;
    
    public delegate void UpdatePlayerInventoryDecorationCount(string name, int newValue);
    public UpdatePlayerInventoryDecorationCount OnDecorationCountUpdated;
    
    private int _water;
    private int _poo;
    private Dictionary<string, int> _unassignedDecorations;


    public int Water => _water;
    public int Poo => _poo;
    public Dictionary<string, int> Decorations => _unassignedDecorations;

    public void CollectWater(int amount =1)
    {
        _water += amount;
        OnWaterUpdated?.Invoke(_water);
    }
    
    public void UseWater(int amount =1)
    {
        _water -= amount;
        OnWaterUpdated?.Invoke(_water);
    }
    
    public void CollectPoo(int amount =1)
    {
        _poo += amount;
        OnPooUpdated?.Invoke(_poo);
    }
    
    public void UsePoo(int amount =1)
    {
        _poo -= amount;
        OnPooUpdated?.Invoke(_poo);
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
