using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlant : MonoBehaviour
{
    #region setup
    [Serializable]
    public struct PlatDecorationSlots
    {
        public string Name;
        public GameDataMonoSingleton.DECORATION_POSITION Position;
        public SpriteRenderer Sprite;
    }
    

    

    #endregion
    
    [Header("Plant set up")]
    [SerializeField] private string _plantName;
    //Set up here in case we want to decorate the pot and change out teh sprite
    [SerializeField] private SpriteRenderer _plantSprite;
    [SerializeField] private SpriteRenderer _needSprite;

    [SerializeField] private PlatDecorationSlots[] _decorationSlots;
    //[SerializeField] private SpriteRenderer _emojiSprite;

    [Header("Poo")]
    [SerializeField] private int _minPoo = 5;
    [Tooltip("How fast the plant uses the poo units per second")] 
    [SerializeField] private float _pooConsumptionRate = 0.1f;

    [Header("Water")]
    [SerializeField] private int _minWater = 5;
    [SerializeField] private int _maxWater = 20;
    [Tooltip("How fast the plant uses the water units per second")] 
    [SerializeField] private float _waterConsumptionRate = 0.3f;
    
    [Header("Sun")]
    [Tooltip("How much sun does the plant need between 1-10 the higher the value the more sun it needs")] 
    [Range(1, 10)]
    [SerializeField] private int _minSun = 5;
    
    [Range(1, 10)]
    [SerializeField] private int _maxSun = 8;
    
    //TODO: set global health percentage to icon rates
    [Header("Health")]
    [SerializeField] private int _maxHealth = 20;
    [Tooltip("How fast does the plant sicken if it's needs aren't met")] 
    [SerializeField] private float _sicknessRate = 0.1f;
    
    [Header("Happiness")]
    //TODO: set global happiness percentage to icon rates
    [SerializeField] private int _maxHappiness = 20;
    [Tooltip("How fast does the plant get sad if it's needs aren't met")] 
    [SerializeField] private float _sadnesssRate = 0.1f;
    [Tooltip("How at what point the plant goes from being sad to sickening")] 
    [SerializeField] private float _sadToSickThresholdPercentage = 0.5f;
    
    [Header("Starting Values")]
    [SerializeField] private int _startingHealth = 10;
    [SerializeField] private int _startingHappiness = 10;
    [SerializeField] private int _startingWater = 7;
    [SerializeField] private int _startingPoo = 8;

    #region Private vars

    private float _pooLevel;
    private int _currentSun;
    private float _waterLevel;
    private float _health;
    private float _happiness;

    private bool _isDead = false;
    
    private float nextActionTime = 0.0f;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        _pooLevel = _startingPoo;
        _waterLevel = _startingWater;
        _health = _startingHealth;
        _happiness = _startingHappiness;
        
        //debug 
        _currentSun = _minSun + 1;
        
        UpdatePlantValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDead)
        {
            if (Time.time > nextActionTime ) {
                nextActionTime += GameDataMonoSingleton.Instance.TickerTimeIntervalInSeconds;
                UpdatePlantValues();
            } 
        }
    }

    private void UpdatePlantValues()
    {
        Debug.Log("***** UpdatePlantValues");
        bool iconSet = false;

        _waterLevel -= _waterConsumptionRate;

        Debug.Log(string.Format("***** _waterLevel {0}", _waterLevel));
        if (_waterLevel < _minWater)
        {
            if (_waterLevel < 0)
            {
                _waterLevel = 0;
            }

            _happiness -= _sadnesssRate;

            if (_happiness < (_maxHappiness * _sadToSickThresholdPercentage))
            {
                _health -= _sicknessRate;
            }

            _needSprite.sprite = GameDataMonoSingleton.Instance.Water;
            iconSet = true;
        }
        else if (_waterLevel > _maxWater)
        {
            _happiness -= _sadnesssRate;

            if (_happiness < (_maxHappiness * _sadToSickThresholdPercentage))
            {
                _health -= _sicknessRate;
            }
        }

        _pooLevel -= _pooConsumptionRate;
        Debug.Log(string.Format("***** _pooLevel {0}", _pooLevel));
        if (_pooLevel < _minPoo)
        {
            if (_pooLevel < 0)
            {
                _pooLevel = 0;
            }

            _happiness -= _sadnesssRate;

            if (_happiness < (_maxHappiness * _sadToSickThresholdPercentage))
            {
                _health -= _sicknessRate;
            }

            if (!iconSet)
            {
                _needSprite.sprite = GameDataMonoSingleton.Instance.Poo;
                iconSet = true;
            }
        }

        int temp = 0;
        if (_currentSun > _maxSun)
        {
            temp = 1;
            _happiness -= _sadnesssRate;

            if (_happiness < (_maxHappiness * _sadToSickThresholdPercentage))
            {
                _health -= _sicknessRate;
            }
        }
        else if (_currentSun < _minSun)
        {
            temp = -1;
            _happiness -= _sadnesssRate;

            if (_happiness < (_maxHappiness * _sadToSickThresholdPercentage))
            {
                _health -= _sicknessRate;
            }

            if (!iconSet)
            {
                _needSprite.sprite = GameDataMonoSingleton.Instance.Heat;
                iconSet = true;
            }
        }

        if (_happiness < 0)
        {
            _happiness = 0;
        }

        Debug.Log(string.Format("***** _happiness {0}", _happiness));
        Debug.Log(string.Format("***** _health {0}", _health));

        //TODo: work out happiness vs health & decide on priority
        float happinessPercentage = _happiness / (float) _maxHappiness;
        float healthPercentage = _health / (float) _maxHealth;
        Sprite needed = GameDataMonoSingleton.Instance.GetEmojiIcon(healthPercentage, temp, iconSet);
        if (needed != null)
        {
            _needSprite.sprite = needed;
        }


        if (_health < 0)
        {
            _health = 0;
            Debug.Log(string.Format("<color=red>OH NOES!!! {0} IS DEAD!</color>", _plantName));
            _needSprite.sprite = GameDataMonoSingleton.Instance.Dead;
            _isDead = true;
        }
    }

    public void AddPoo(int amount = 1)
    {
        _pooLevel += amount;
        PlayerInventoryMonoSingleton.Instance.UsePoo(amount);
    }
    
    public void AddWater(int amount = 1)
    {
        _waterLevel += amount;
        PlayerInventoryMonoSingleton.Instance.UseWater(amount);
    }
    
    
    public void SetSunLevel(int level)
    {
        _currentSun += level;
    }

    public bool RemoveDecoration(GameDataMonoSingleton.DECORATION_POSITION slot)
    {
        int slotIndex = GetSlotIndex(slot);
        if (slotIndex < 0)
        {
            Debug.Log(string.Format("<color=red>OH NOES!!! {0} Does not have a slot at position {1}!</color>",
                _plantName, slot));
            return false;
        }

        if (!string.IsNullOrEmpty(_decorationSlots[slotIndex].Name))
        {
            PlayerInventoryMonoSingleton.Instance.CollectDecoration(_decorationSlots[slotIndex].Name);
            return true;
        }
        
        Debug.Log(string.Format("<color=red>OH NOES!!! {0} Does not have a decoration at position {1}!</color>",
            _plantName, slot));
        return false;
    }

    public bool AttachDecoration(string decoName, GameDataMonoSingleton.DECORATION_POSITION slot)
    {
        bool canUseDecoration = PlayerInventoryMonoSingleton.Instance.HasDecoration(decoName);
        if (!canUseDecoration)
        {
            Debug.Log(string.Format("<color=red>OH NOES!!! The player does not have enough {0}!</color>", decoName));
            return false;
        }
        bool canPutInSlot = GameDataMonoSingleton.Instance.IsValidSlot(decoName, slot);
        if (!canPutInSlot)
        {
            Debug.Log(string.Format("<color=red>OH NOES!!! {0} cannot go into slot {1}!</color>", decoName, slot));
            return false;
        }
        
        int slotIndex = GetSlotIndex(slot);
        if (slotIndex < 0)
        {
            Debug.Log(string.Format("<color=red>OH NOES!!! {0} Does not have a slot at position {1}!</color>",
                _plantName, slot));
            return false;
        }

        if (!string.IsNullOrEmpty(_decorationSlots[slotIndex].Name))
        {
            PlayerInventoryMonoSingleton.Instance.CollectDecoration(_decorationSlots[slotIndex].Name);
        }
        
        _decorationSlots[slotIndex].Name = decoName;
        _decorationSlots[slotIndex].Sprite.sprite = GameDataMonoSingleton.Instance.GetDectorationSprite(decoName);
        PlayerInventoryMonoSingleton.Instance.UseDecoration(decoName);

        return false;
    }

    private int GetSlotIndex(GameDataMonoSingleton.DECORATION_POSITION slot)
    {
        for (int i = 0; i < _decorationSlots.Length; i++)
        {
            if (_decorationSlots[i].Position == slot)
            {
                return i;
            }
        }

        return -1;
    }
    
}
