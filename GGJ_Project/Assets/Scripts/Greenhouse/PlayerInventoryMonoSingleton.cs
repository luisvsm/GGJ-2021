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
    
    
    public void CollectDecoration(string name)
    {
        if (_unassignedDecorations == null)
        {
            _unassignedDecorations = new Dictionary<string, int>();
        }

        if (_unassignedDecorations.ContainsKey(name))
        {
            int value = _unassignedDecorations[name];
            _unassignedDecorations[name] = value + 1;
            OnDecorationCountUpdated?.Invoke(name, _unassignedDecorations[name]);
        }
        else
        {
            _unassignedDecorations.Add(name,1);
            OnDecorationCountUpdated?.Invoke(name, _unassignedDecorations[name]);
        }
    }
    
    public void UseDecoration(string name)
    {
        if (_unassignedDecorations == null)
        {
            _unassignedDecorations = new Dictionary<string, int>();
        }

        if (_unassignedDecorations.ContainsKey(name))
        {
            int value = _unassignedDecorations[name];
            if (value > 0)
            {
                _unassignedDecorations[name] = value - 1;
                OnDecorationCountUpdated?.Invoke(name, _unassignedDecorations[name]);
            }
        }
        else
        {
            Debug.Log(string.Format("<color=red>OH NOES!!! Cannot find decoration {0} in game inventory </color>", name));
        }
    }
    
    
}
