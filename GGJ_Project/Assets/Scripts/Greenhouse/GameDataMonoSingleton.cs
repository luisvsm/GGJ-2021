using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMonoSingleton : MonoBehaviourSingleton<GameDataMonoSingleton>
{
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

    [Header("Emoji Threshold percentages 0-1")] 
    [SerializeField] private float _sickThreshold = 0.5f;
    [SerializeField] private float _almostDeadThreshold = 0.5f;

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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
