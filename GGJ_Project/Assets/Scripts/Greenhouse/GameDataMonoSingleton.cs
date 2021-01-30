using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMonoSingleton : MonoBehaviourSingleton<GameDataMonoSingleton>
{
    #region Data set up

    public enum DECORATION_POSITION
    {
        top,
        middle,
        bottom
    }
    
    public enum RESOURCE_TYPE
    {
        water,
        poo,
        love
    }
    [Serializable]
    public struct PlatDecoration
    {
        public string Name;
        public GameDataMonoSingleton.DECORATION_POSITION Position;
        public Sprite Sprite;
    }
    

    #endregion
    
    [Header("Need Icons")]

    [SerializeField] private Sprite _water;

    [SerializeField] private Sprite _poo;

    [SerializeField] private Sprite _heat;

    [SerializeField] private Sprite _heart;
    
    
    [Header("Happiness/sickness Emojis")]
    
    [SerializeField] private Sprite _happy;
    [SerializeField] private Sprite _gimmePoop;
    [SerializeField] private Sprite _hot;
    [SerializeField] private Sprite _cold;
    [SerializeField] private Sprite _sick;
    [SerializeField] private Sprite _almostDead;
    [SerializeField] private Sprite _dead;

    [Header("Configuration Values")] 
    [SerializeField] private float _sickThreshold = 0.5f;
    [SerializeField] private float _almostDeadThreshold = 0.25f;
    [SerializeField] private float _TickerTimeIntervalInSeconds = 1.0f;

    [Header("Decorations")] 
    [SerializeField] private PlatDecoration[] _decorations;

    #region encapsulated fields

    public Sprite Water => _water;

    public Sprite Poo => _poo;

    public Sprite Heat => _heat;

    public Sprite Heart => _heart;

    public Sprite Happy => _happy;

    public Sprite GimmePoop => _gimmePoop;

    public Sprite Hot => _hot;

    public Sprite Sick => _sick;

    public Sprite AlmostDead => _almostDead;

    public Sprite Cold => _cold;

    public Sprite Dead => _dead;

    public float TickerTimeIntervalInSeconds => _TickerTimeIntervalInSeconds;

    #endregion
    

    public bool IsHappy(float healthPercentage)
    {
        if (healthPercentage > _sickThreshold)
        {
            return true;
        }

        return false;
    }

    //Temperature <0 = cold >0 = hot 0 = allgood
    public Sprite GetEmojiIcon(float healthPercentage,  int temperature, bool needIconSet)
    {
        if (!needIconSet)
        {
            if (healthPercentage > _sickThreshold)
            {
                if (temperature > 0)
                {
                    return _hot;
                }
                else if (temperature < 0)
                {
                    return _cold;
                }
            
                return _happy;
            }
         
            if (healthPercentage > _almostDeadThreshold)
            {
                return _sick;
            } 
            
            return _almostDead;
        }
        
        if (healthPercentage < _almostDeadThreshold)
        {
            return _almostDead;
        }

        return null;
    }

    public Sprite GetDectorationSprite(string name)
    {
        for (int i = 0; i  < _decorations.Length; i++)
        {
            if (name.Equals(_decorations[i].Name))
            {
                return _decorations[i].Sprite;
            }
        }
        Debug.Log(string.Format("<color=red>OH NOES!!! Cannot find decoration {0} in game data </color>", name));

        return null;
    }

    public bool IsValidSlot(string name, DECORATION_POSITION requestedSlot)
    {
        bool found = false;
        for (int i = 0; i < _decorations.Length; i++)
        {
            if (name.Equals(_decorations[i].Name))
            {
                found = true;
                if (_decorations[i].Position == requestedSlot)
                {
                    return true;
                }
            }
        }

        if (!found)
        {
            Debug.Log(string.Format("<color=red>OH NOES!!! Cannot find decoration {0} in game data </color>", name));

        }

        return false;
    }
}
