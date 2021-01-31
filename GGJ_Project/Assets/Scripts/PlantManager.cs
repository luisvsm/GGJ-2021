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

    public string SelectedPlant => _selectedPlant;

    public Transform PlantViewPosition => _plantViewPosition;

    public void ShowAllPlants()
    {
        for (int i = 0; i < _plantList.Count; i++)
        {
            _plantList[i].gameObject.SetActive(true);
            _plantList[i].IsZoomedOutView = true;
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
        _plantList[_nextPlantTalk].QueueForRandomConversation(_nextTalkTimestamp);
    }
    
    public Sprite GetPlantIcon(string lineSpeakerName)
    {
        for (int i = 0; i < _plantList.Count; i++)
        {
            if (_plantList[i].name == lineSpeakerName)
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
}
