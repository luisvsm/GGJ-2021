using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlant : MonoBehaviour
{
    [SerializeField] private string _plantName;
    //Set up here in case we want to decorate the pot and change out teh sprite
    [SerializeField] private SpriteRenderer _plantSprite;
    [SerializeField] private SpriteRenderer _needSprite;
    //[SerializeField] private SpriteRenderer _emojiSprite;
    
    [SerializeField] private int _startingPoo = 8;
    [SerializeField] private int _minPoo = 5;
    [Tooltip("How fast the plant uses the poo units per second")] 
    [SerializeField] private float _pooConsumptionRate = 0.05f;

    [SerializeField] private int _startingWater = 7;
    [SerializeField] private int _minWater = 5;
    [SerializeField] private int _maxWater = 20;
    [Tooltip("How fast the plant uses the water units per second")] 
    [SerializeField] private float _waterConsumptionRate = 0.02f;
    
    [Tooltip("How much sun does the plant need between 1-10 the higher the value the more sun it needs")] 
    [Range(1, 10)]
    [SerializeField] private int _minSun = 5;
    
    [Range(1, 10)]
    [SerializeField] private int _maxSun = 8;
    
    //TODO: set global health percentage to icon rates
    [SerializeField] private int _startingHealth = 10;
    [SerializeField] private int _maxHealth = 20;
    [Tooltip("How fast does the plant sicken if it's needs aren't met")] 
    [SerializeField] private float _sicknessRate = 0.1f;
    
    //TODO: set global happiness percentage to icon rates
    [SerializeField] private int _maxHappiness = 20;
    [SerializeField] private int _startingHappiness = 10;
    [Tooltip("How fast does the plant sicken if it's needs aren't met")] 
    [SerializeField] private float _sadnesssRate = 0.1f;
    
    [SerializeField] private float _sadToSickThresholdPercentage = 0.5f;
    
    private float _pooLevel;
    private int _currentSun;
    private float _waterLevel;
    private float _health;
    private float _happiness;

    private bool _isDead = false;
    
    private float nextActionTime = 0.0f;
    private float period = 0.1f;
    
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
                nextActionTime += period;
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
    }
    
    public void AddWater(int amount = 1)
    {
        _waterLevel += amount;
    }
    
    
    public void SetSunLevel(int level)
    {
        _currentSun += level;
    }
    
}
