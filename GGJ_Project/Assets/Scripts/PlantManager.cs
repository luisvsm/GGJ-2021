using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Plant manager is the bestest boii
public class PlantManager : MonoBehaviourSingleton<PlantManager>
{
    [SerializeField] private List<BasePlant> _plantList;
    [SerializeField] private  Transform _plantViewPosition;
    
    private float _nextTalkTimestamp;
    private int _nextPlantTalk = -1;
    private string _selectedPlant = "BonScot";
    private bool _firstLoadHack = true;

    public string SelectedPlant => _selectedPlant;

    public Transform PlantViewPosition => _plantViewPosition;

    public bool FirstLoadHack
    {
        get => _firstLoadHack;
        set => _firstLoadHack = value;
    }

    public void ShowAllPlants()
    {
        for (int i = 0; i < _plantList.Count; i++)
        {
            _plantList[i].gameObject.SetActive(true);
            _plantList[i].IsZoomedOutView = true;
        }

        //triggers the random conversation system
        if (_nextPlantTalk == -1)
        {
            PlantTalked();
        }
    }

    public void HideAllPlants()
    {
        for (int i = 0; i < _plantList.Count; i++)
        {
            _plantList[i].gameObject.SetActive(false);
            _plantList[i].IsZoomedOutView = false;
        }
    }

    public void showPlant(int plantIndex)
    {
        HideAllPlants();
        _plantList[plantIndex].gameObject.SetActive(true);
    }
    
    public BasePlant GetPlantForPlantView(string plantName)
    {
        HideAllPlants();
        BasePlant plant = GetPlant(plantName);
        plant.gameObject.SetActive(true);
        return plant;
    }
    
    public BasePlant GetPlant(string plantName)
    {
        for (int i = 0; i < _plantList.Count; i++)
        {
            if (_plantList[i].PlantName == plantName)
            {
                return _plantList[i];
            }
        }

        return null;
    }
    
    public void AddPlant(BasePlant plant)
    {
        if (_plantList == null)
        {
            _plantList = new List<BasePlant>();
        }

        _plantList.Add(plant);
    }

    public void PlantTalked()
    {
        _nextTalkTimestamp = Time.time + Random.Range(GameDataMonoSingleton.Instance.RandomTalkIntervalInSecondsMin,
                                 GameDataMonoSingleton.Instance.RandomTalkIntervalInSecondsMax);
        _nextPlantTalk = Random.Range(0, _plantList.Count - 1);
        Debug.Log(string.Format("<color=magenta>***** Nextplant index 01- {0} </color>",_nextPlantTalk));

        if (GameDataMonoSingleton.Instance.IsCharacterConversationExhausted(_plantList[_nextPlantTalk].PlantName))
        {
            _nextPlantTalk = Random.Range(0, _plantList.Count - 1);
            Debug.Log(string.Format("<color=magenta>***** Nextplant index 02- {0} </color>",_nextPlantTalk));
            if (GameDataMonoSingleton.Instance.IsCharacterConversationExhausted(_plantList[_nextPlantTalk].PlantName))
            {
                string plantID = GameDataMonoSingleton.Instance.GetNextCharacterAvailiableForConversation();

                if (string.IsNullOrEmpty(plantID))
                {
                    return;
                }
                else
                {
                    _nextPlantTalk = GetPlantIndex(plantID);
                    Debug.Log(string.Format("<color=magenta>***** Nextplant index 03- {0} </color>",_nextPlantTalk));
                }
                    
            }
        }

        if (_nextPlantTalk < 0 || _nextPlantTalk > _plantList.Count)
        {
            Debug.Log(string.Format("<color=red>***** ERROR {0} trying to get out of range index </color>",_nextPlantTalk));

        }
        Debug.Log(string.Format("<color=magenta>*****Queueing {0} to talk</color>", _plantList[_nextPlantTalk].PlantName));
        _plantList[_nextPlantTalk].QueueForRandomConversation(_nextTalkTimestamp);
    }

    private int GetPlantIndex(string plantId)
    {
        for (int i = 0; i < _plantList.Count; i++)
        {
            if (_plantList[i].PlantName == plantId)
            {
                return i;
            }
        }

        return -1;
    }


    public Sprite GetPlantIcon(string lineSpeakerName)
    {
        for (int i = 0; i < _plantList.Count; i++)
        {
            if (_plantList[i].PlantName == lineSpeakerName)
            {
                return _plantList[i].Icon;
            }
        }

        return null;
    }

    public void GoToPlantView(string plantName)
    {
        _selectedPlant = plantName;
        RoomController.Instance.ShowPlantRoom();
    }

    public BasePlant GetSelectedPlant()
    {
        return GetPlantForPlantView(_selectedPlant);
    }

    public void ConversationComplete()
    {
        BasePlant plant = GetSelectedPlant();
        plant.EndConversation();
        PlantTalked();
    }
}
