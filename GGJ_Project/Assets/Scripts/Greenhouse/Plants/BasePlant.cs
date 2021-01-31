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
    [SerializeField] private GameObject _needIndicatorDetailed;
    [SerializeField] private GameObject _needIndicatorAlert;
    [SerializeField] private PercentageFillBar _healthPercentageBar;
    //[SerializeField] private PercentageFillBar _happinessPercentageBar;
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
    //private float _happiness;

    //private bool _isDead = false;
    
    private bool _isWaitingToTalk = false;
    private float _nextTalkTimeStamp;
    private bool _isNextToTalk;
    private bool _firstConversation = true;

    private bool _isZoomedOutView = true;
    private bool _needsAttention;

    
    private float _nextActionTime = 0.0f;
    private bool _hitmaxhealth = false;

    public Sprite Icon => _plantIcon;

    public string PlantName => _plantName;

    public bool IsZoomedOutView
    {
        get => _isZoomedOutView;
        set => _isZoomedOutView = value;
    }

    private void OnMouseDown()
    {
        if (_isZoomedOutView)
        {
            PlantManager.Instance.GoToPlantView(_plantName);
            _needIndicatorAlert.gameObject.SetActive(false);
            _needIndicatorDetailed.gameObject.SetActive(true);
        }
    }

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        _pooLevel = _startingPoo;
        _waterLevel = _startingWater;
        _health = _startingHealth;
       // _happiness = _startingHappiness;
        
        //_happinessPercentageBar.Initialize(_happiness/_maxHappiness);
        _healthPercentageBar.Initialize(_health/_maxHealth);
        
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
        
//        if (!_isDead)
//        {
            if (_isNextToTalk && Time.time > _nextTalkTimeStamp)
            {
                WantsToTalk();
            }
            else if (Time.time > _nextActionTime ) {
                _nextActionTime += GameDataMonoSingleton.Instance.TickerTimeIntervalInSeconds;
                UpdatePlantValues();
            } 
       // }
    }

    private void WantsToTalk()
    {
        if (!_isWaitingToTalk)
        {
            _needIconSprite.sprite = GameDataMonoSingleton.Instance.ToTalk;
            _startConversation.Activate();
            _isWaitingToTalk = true;
            _needIndicatorAlert.gameObject.SetActive(_isZoomedOutView);
            _needIndicatorDetailed.gameObject.SetActive(!_isZoomedOutView);
        }
    }

    public void StartConversation()
    {
        _startConversation.Deactivate();
        Debug.Log("<color=blue>START CONVERSATION</color>");
        GameDataMonoSingleton.Instance.StartNextConversation(_plantName, _firstConversation);
        
        
        //StartConversation
    }
    
    
    
    public void EndConversation()
    {
        _firstConversation = false;
        _isWaitingToTalk = false;
        _isNextToTalk = false;
        UpdatePlantValues();
    }

    private void UpdatePlantValues()
    {
        if (_logPlantStats)
        {
            Debug.Log("***** UpdatePlantValues");
        }

        bool iconSet = false;
        bool healthUpdated = false;
        bool hasNeed = false;
        _waterLevel -= _waterConsumptionRate;


        if (_logPlantStats)
        {
            Debug.Log(string.Format("***** _waterLevel {0}", _waterLevel));
        }

        if (_waterLevel < _minWater)
        {
            hasNeed = true;
            if (_waterLevel < 0)
            {
                _waterLevel = 0;
            }

            _health -= _sicknessRate;
            healthUpdated = true;

            _needIconSprite.sprite = GameDataMonoSingleton.Instance.Water;
            iconSet = true;
        }
        else if (_waterLevel > _maxWater)
        {
            _health -= _sicknessRate;
            healthUpdated = true;
        }

        _pooLevel -= _pooConsumptionRate;
        if (_logPlantStats)
        {
            Debug.Log(string.Format("***** _pooLevel {0}", _pooLevel));
        }

        if (_pooLevel < _minPoo)
        {
            hasNeed = true;
            if (_pooLevel < 0)
            {
                _pooLevel = 0;
            }

            _health -= _sicknessRate;
            healthUpdated = true;


            if (!iconSet)
            {
                _needIconSprite.sprite = GameDataMonoSingleton.Instance.Poo;
                iconSet = true;
            }
        }

        int temp = 0;
        if (_currentSun > _maxSun)
        {
            hasNeed = true;
            temp = 1;

            _health -= _sicknessRate;
            healthUpdated = true;

        }
        else if (_currentSun < _minSun)
        {
            hasNeed = true;
            temp = -1;

            _health -= _sicknessRate;
            healthUpdated = true;

            if (!iconSet)
            {
                _needIconSprite.sprite = GameDataMonoSingleton.Instance.Heat;
                iconSet = true;
            }
        }

        if (_logPlantStats)
        {
            Debug.Log(string.Format("***** _health {0}", _health));
        }

        //TODo: work out happiness vs health & decide on priority
        // float happinessPercentage = _happiness / (float) _maxHappiness;
        float healthPercentage = _health / (float) _maxHealth;
        /*Sprite needed = GameDataMonoSingleton.Instance.GetEmojiIcon(healthPercentage, temp, iconSet);
        if (needed != null)
        {
            _needIconSprite.sprite = needed;
        }*/


        if (_health < 0)
        {
            _health = 0;
            Debug.Log(string.Format("<color=red>OH NOES!!! {0} IS DEAD!</color>", _plantName));
            //_needIconSprite.sprite = GameDataMonoSingleton.Instance.Dead;
           // _isDead = true;
        }
        
       // Debug.Log(string.Format("<color=magenta><b>Health {0} maxHealth {1} plant {2}!</b></color>", _health, _maxHealth, _plantName));

        if (healthUpdated)
        {
            if (healthPercentage < 1.0f && _hitmaxhealth)
            {
                MusicController.Instance.PauseSong(_plantName);
                _hitmaxhealth = false;
            }
            _healthPercentageBar.UpdateBar(healthPercentage);
            
        }
        else
        {
            _health = _health + 1;
            _healthPercentageBar.UpdateBar(healthPercentage);
        }
        
        if (_health >= _maxHealth)
        {
            //Debug.Log(string.Format("<color=magenta><b>MAX HEALTH UPDATE {0}!</b></color>", _plantName));

            _needIndicatorAlert.gameObject.SetActive(false);
            _needIndicatorDetailed.gameObject.SetActive(false);
            _waterLevel = _maxWater;
            _pooLevel = _minPoo + 3;
            _health = _maxHealth;
            _hitmaxhealth = true;
            _nextActionTime += GameDataMonoSingleton.Instance.HappyPlantHappyTimeInSeconds;
            MusicController.Instance.PlaySong(_plantName);
            AudioController.Play("SFX_Give_Special");
            playMaxHealth();
        }
        else
        {
            if (hasNeed)
            {
                _needIndicatorAlert.gameObject.SetActive(_isZoomedOutView);
                _needIndicatorDetailed.gameObject.SetActive(!_isZoomedOutView);
            }
            else
            {
                _needIndicatorAlert.gameObject.SetActive(false);
                _needIndicatorDetailed.gameObject.SetActive(false);
            }
            
        }

        playAttention(healthPercentage);
	}

	private void playMaxHealth()
	{
		if (PlantName == "BonScot")
			AudioController.Play("VO_BonScot_MaxHealth");
		else if (PlantName == "Vera")
			AudioController.Play("VO_Vera_MaxHealth");
		else if (PlantName == "Basil")
			AudioController.Play("VO_Basil_MaxHealth");
		else if (PlantName == "ReginaGretchenKaren")
			AudioController.Play("VO_FlyTraps_FullHealth");
		else if (PlantName == "Pepper")
			AudioController.Play("VO_Pepper_MaxHealth");
		else if (PlantName == "Arnold")
			AudioController.Play("VO_Arnie_FullHealth");
	}

	private static float nextTimeThatWeCanPlayAttentionSound;

	private void playAttention(float healthPercentage)
	{
		if (healthPercentage < 0.2 && nextTimeThatWeCanPlayAttentionSound < Time.time)
		{
			nextTimeThatWeCanPlayAttentionSound = Time.time + 3f;
			if (PlantName == "BonScot")
				AudioController.Play("VO_BonScot_Attention");
			else if (PlantName == "Vera")
				AudioController.Play("VO_Vera_Attention");
			else if (PlantName == "Basil")
				AudioController.Play("VO_Basil_Attention");
			else if (PlantName == "ReginaGretchenKaren")
				AudioController.Play("VO_FlyTraps_Attention");
			else if (PlantName == "Pepper")
				AudioController.Play("VO_Pepper_Attention");
			else if (PlantName == "Arnold")
				AudioController.Play("VO_Arnie_Attention");
		}
	}


	public void AddPoo(int amount = 1)
    {
        bool canDo = PlayerInventoryMonoSingleton.Instance.UsePoo(amount);
        if (canDo)
        {
            _pooLevel += amount;
            _health += GameDataMonoSingleton.Instance.ResourceHealAmount;
            _healthPercentageBar.UpdateBar(_health/_maxHealth);
        }
        else
        {
            Debug.Log(string.Format("<color=red>OH NOES!!! Noo more POO!</color>"));
        }
    }

    public void AddWater(int amount = 1)
    {
        bool canDo = PlayerInventoryMonoSingleton.Instance.UseWater(amount);
        if (canDo)
        {
            _waterLevel += amount;
            _health += GameDataMonoSingleton.Instance.ResourceHealAmount;
            _healthPercentageBar.UpdateBar(_health/_maxHealth);
        }
        else
        {
            Debug.Log(string.Format("<color=red>OH NOES!!! Noo more WATER!</color>"));
        }
    }
    
    private void AddLove()
    {
        _health = _health + 1;
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
        _healthPercentageBar.UpdateBar(_health/_maxHealth);
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
		if (_plantName == "ReginaGretchenKaren")
			AudioController.Play("VO_FlyTraps_Heal");

		else if (_plantName == "Pepper")
			AudioController.Play("VO_Pepper_Heal");

		else if (_plantName == "BonScot")
			AudioController.Play("VO_BonScot_Heal");

		else if (_plantName == "Basil")
			AudioController.Play("VO_Basil_Heal");

		else if (_plantName == "Arnold")
			AudioController.Play("VO_Arnie_Heal");

		else if (_plantName == "Vera")
			AudioController.Play("VO_Vera_Heal");


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
            case GameDataMonoSingleton.RESOURCE_TYPE.sun:
                Debug.Log("TODOL: Move plant to sunny spot <3");
                break;
        }
    }

    public void QueueForRandomConversation(float nextTalkTimestamp)
    {
        _isNextToTalk = true;
        _nextTalkTimeStamp = nextTalkTimestamp;
    }
}
