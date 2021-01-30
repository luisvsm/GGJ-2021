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
        public string AssignedDecorationName;
        public GameDataMonoSingleton.DECORATION_POSITION Position;
        public SpriteRenderer SpriteRend;
    }
    
    #endregion
    
    [Header("Plant set up")]
    [SerializeField] private string _plantName;
    [SerializeField] private Sprite _plantIcon;
    [SerializeField] private SpriteRenderer _needIconSprite;    
    [SerializeField] private SpriteRenderer _dialogSprite;
    [SerializeField] private PercentageFillBar _healthPercentageBar;
    [SerializeField] private PercentageFillBar _happinessPercentageBar;
    [SerializeField] private StartConversation _startConversation;

    [Header("Decoration")]
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
    [SerializeField] private float _healingRate = 0.2f;
    
    [Header("Happiness")]
    //TODO: set global happiness percentage to icon rates
    [SerializeField] private int _maxHappiness = 20;
    [Tooltip("How fast does the plant get sad if it's needs aren't met")] 
    [SerializeField] private float _sadnesssRate = 0.1f;
    [SerializeField] private float _happinessRate = 0.2f;
    [Tooltip("How at what point the plant goes from being sad to sickening")] 
    [SerializeField] private float _sadToSickThresholdPercentage = 0.5f;
    
    [Header("Starting Values")]
    [SerializeField] private int _startingHealth = 15;
    [SerializeField] private int _startingHappiness = 15;
    [SerializeField] private int _startingWater = 7;
    [SerializeField] private int _startingPoo = 8;

    [Header("DEBUG")] 
    [SerializeField] private bool _logPlantStats;
   
    #region Private vars

    private float _pooLevel;
    private int _currentSun;
    private float _waterLevel;
    private float _health;
    private float _happiness;

    private bool _isDead = false;
    
    private bool _isWaitingToTalk = false;
    private float _nextTalkTimeStamp;
    private bool _isNextToTalk;

    
    private float _nextActionTime = 0.0f;

    public Sprite Icon => _plantIcon;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        _pooLevel = _startingPoo;
        _waterLevel = _startingWater;
        _health = _startingHealth;
        _happiness = _startingHappiness;
        
        _happinessPercentageBar.Initialize(_happiness/_maxHappiness);
        _healthPercentageBar.Initialize(_health/_maxHealth);
        
        PlayerInventoryMonoSingleton.Instance.AddPlant(this);
        //debug 
        _currentSun = _minSun + 1;
        
        UpdatePlantValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameDataMonoSingleton.Instance.TickerPaused)
        {
            //If the ticker is paused don't do anything.
            return;
        }
        
        if (!_isDead)
        {
            if (_isNextToTalk && Time.time > _nextTalkTimeStamp)
            {
                WantsToTalk();
            }
            else if (Time.time > _nextActionTime ) {
                _nextActionTime += GameDataMonoSingleton.Instance.TickerTimeIntervalInSeconds;
                UpdatePlantValues();
            } 
        }
    }

    private void WantsToTalk()
    {
        if (!_isWaitingToTalk)
        {
            _needIconSprite.sprite = GameDataMonoSingleton.Instance.ToTalk;
            _startConversation.Activate();
            _isWaitingToTalk = true;
        }
    }

    public void StartConversation()
    {
        _startConversation.Deactivate();
        Debug.Log("<color=blue>START CONVERSATION</color>");
        GameDataMonoSingleton.Instance.StartNextConversation(_plantName);
        
        //StartConversation
    }
    
    
    
    public void EndConversation()
    {
        _isWaitingToTalk = false;
        _isNextToTalk = false;
        PlayerInventoryMonoSingleton.Instance.PlantTalked();
    }

    private void UpdatePlantValues()
    {
        if (_logPlantStats)
        {
            Debug.Log("***** UpdatePlantValues");
        }
        bool iconSet = false;
        bool healthUpdated = false;
        bool happinessUpdated = false;
        _waterLevel -= _waterConsumptionRate;
        float sadToSickThreshold = _maxHappiness * _sadToSickThresholdPercentage;

        if (_logPlantStats)
        {
            Debug.Log(string.Format("***** _waterLevel {0}", _waterLevel));
        }

        if (_waterLevel < _minWater)
        {
            if (_waterLevel < 0)
            {
                _waterLevel = 0;
            }

            _happiness -= _sadnesssRate;
            happinessUpdated = true;
            if (_happiness < sadToSickThreshold)
            {
                _health -= _sicknessRate;
                healthUpdated = true;
            }

            _needIconSprite.sprite = GameDataMonoSingleton.Instance.Water;
            iconSet = true;
        }
        else if (_waterLevel > _maxWater)
        {
            _happiness -= _sadnesssRate;
            happinessUpdated = true;
            if (_happiness < sadToSickThreshold)
            {
                _health -= _sicknessRate;
                healthUpdated = true;
            }
        }

        _pooLevel -= _pooConsumptionRate;
        if (_logPlantStats)
        {
            Debug.Log(string.Format("***** _pooLevel {0}", _pooLevel));
        }

        if (_pooLevel < _minPoo)
        {
            if (_pooLevel < 0)
            {
                _pooLevel = 0;
            }

            _happiness -= _sadnesssRate;
            happinessUpdated = true;
            if (_happiness < sadToSickThreshold)
            {
                _health -= _sicknessRate;
                healthUpdated = true;
            }

            if (!iconSet)
            {
                _needIconSprite.sprite = GameDataMonoSingleton.Instance.Poo;
                iconSet = true;
            }
        }

        int temp = 0;
        if (_currentSun > _maxSun)
        {
            temp = 1;
            _happiness -= _sadnesssRate;
            happinessUpdated = true;
            if (_happiness < sadToSickThreshold)
            {
                _health -= _sicknessRate;
                healthUpdated = true;
            }
        }
        else if (_currentSun < _minSun)
        {
            temp = -1;
            _happiness -= _sadnesssRate;
            happinessUpdated = true;
            if (_happiness < sadToSickThreshold)
            {
                _health -= _sicknessRate;
                healthUpdated = true;
            }

            if (!iconSet)
            {
                _needIconSprite.sprite = GameDataMonoSingleton.Instance.Heat;
                iconSet = true;
            }
        }

        if (_happiness < 0)
        {
            _happiness = 0;
        }

       if( _logPlantStats)
       {
           Debug.Log(string.Format("***** _happiness {0}", _happiness));
           Debug.Log(string.Format("***** _health {0}", _health));
       }

        //TODo: work out happiness vs health & decide on priority
        float happinessPercentage = _happiness / (float) _maxHappiness;
        float healthPercentage = _health / (float) _maxHealth;
        Sprite needed = GameDataMonoSingleton.Instance.GetEmojiIcon(healthPercentage, temp, iconSet);
        if (needed != null)
        {
            _needIconSprite.sprite = needed;
        }


        if (_health < 0)
        {
            _health = 0;
            Debug.Log(string.Format("<color=red>OH NOES!!! {0} IS DEAD!</color>", _plantName));
            _needIconSprite.sprite = GameDataMonoSingleton.Instance.Dead;
            _isDead = true;
        }

        if (happinessUpdated)
        {
           _happinessPercentageBar.UpdateBar(happinessPercentage);
        }
        
        if (healthUpdated)
        {
            _healthPercentageBar.UpdateBar(healthPercentage);
        }

        //This means all the plant's needs are met
        if (!happinessUpdated && !healthUpdated)
        {
            _happiness += _happinessRate;
            _health += _healingRate;
            if (_happiness > _maxHappiness)
            {
                _happiness = _maxHappiness;
            }
            
            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }
            _healthPercentageBar.UpdateBar(healthPercentage);
            _happinessPercentageBar.UpdateBar(happinessPercentage);
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
    
    private void AddLove()
    {
        _happiness = _happiness + 1;
        if (_happiness > _maxHappiness)
        {
            _happiness = _maxHappiness;
        }
        _happinessPercentageBar.UpdateBar(_happiness/_maxHappiness);
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

        if (!string.IsNullOrEmpty(_decorationSlots[slotIndex].AssignedDecorationName))
        {
            PlayerInventoryMonoSingleton.Instance.CollectDecoration(_decorationSlots[slotIndex].AssignedDecorationName);
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

        if (!string.IsNullOrEmpty(_decorationSlots[slotIndex].AssignedDecorationName))
        {
            PlayerInventoryMonoSingleton.Instance.CollectDecoration(_decorationSlots[slotIndex].AssignedDecorationName);
        }
        
        _decorationSlots[slotIndex].AssignedDecorationName = decoName;
        _decorationSlots[slotIndex].SpriteRend.sprite = GameDataMonoSingleton.Instance.GetDectorationSprite(decoName);
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

    public void AddResource(GameDataMonoSingleton.RESOURCE_TYPE resourceType)
    {
        switch (resourceType)
        {
            case GameDataMonoSingleton.RESOURCE_TYPE.poo:
                AddPoo();
                break;
            case GameDataMonoSingleton.RESOURCE_TYPE.water:
                AddWater();
                break;
            case GameDataMonoSingleton.RESOURCE_TYPE.love:
                AddLove();
                break;
        }
    }

    public void QueueForRandomConversation(float nextTalkTimestamp)
    {
        _isNextToTalk = true;
        _nextTalkTimeStamp = nextTalkTimestamp;
    }
}
