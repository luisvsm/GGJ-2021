using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        love,
        leafLitter,
        sun
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

    [SerializeField] private Sprite _toTalk;


    [Header("Happiness/sickness Emojis")]

    //[SerializeField] private Sprite _happy;
    [SerializeField] private Sprite _gimmePoop;
    [SerializeField] private Sprite _hot;
    [SerializeField] private Sprite _cold;
    //[SerializeField] private Sprite _sick;
    //[SerializeField] private Sprite _almostDead;
    //[SerializeField] private Sprite _dead;

    [Header("Configuration Values")]
    [SerializeField] private float _sickThreshold = 0.5f;
    [SerializeField] private float _almostDeadThreshold = 0.25f;
    [SerializeField] private float _tickerTimeIntervalInSeconds = 1.0f;
    [SerializeField] private float _randomTalkIntervalInSecondsMin = 20.0f;
    [SerializeField] private float _randomTalkIntervalInSecondsMax = 30.0f;
    [SerializeField] private float _happyPlantHappyTimeInSeconds = 120.0f;
    [SerializeField] private float _resourceHealAmount = 1.0f;
    
    private bool _tickerPaused;

    [Header("Decorations")]
    [SerializeField] private PlatDecoration[] _decorations;

    [Header("Conversations")]
    [SerializeField] private ConversationData[] _conversationDatas;
    

    #region encapsulated fields

    public Sprite Water => _water;

    public Sprite Poo => _poo;

    public Sprite Heat => _heat;

    public Sprite Heart => _heart;

    //public Sprite Happy => _happy;

    public Sprite GimmePoop => _gimmePoop;

    public Sprite Hot => _hot;

   // public Sprite Sick => _sick;

   // public Sprite AlmostDead => _almostDead;

    public Sprite Cold => _cold;

    //public Sprite Dead => _dead;

    public float TickerTimeIntervalInSeconds => _tickerTimeIntervalInSeconds;

    public bool TickerPaused => _tickerPaused;

    public Sprite ToTalk => _toTalk;

    public float RandomTalkIntervalInSecondsMin => _randomTalkIntervalInSecondsMin;

    public float RandomTalkIntervalInSecondsMax => _randomTalkIntervalInSecondsMax;

    public float HappyPlantHappyTimeInSeconds => _happyPlantHappyTimeInSeconds;
    public float ResourceHealAmount => _resourceHealAmount;

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
//    public Sprite GetEmojiIcon(float healthPercentage, int temperature, bool needIconSet)
//    {
//        if (!needIconSet)
//        {
//            if (healthPercentage > _sickThreshold)
//            {
//                if (temperature > 0)
//                {
//                    return _hot;
//                }
//                else if (temperature < 0)
//                {
//                    return _cold;
//                }
//
//                return _happy;
//            }
//
//            if (healthPercentage > _almostDeadThreshold)
//            {
//                return _sick;
//            }
//
//            return _almostDead;
//        }
//
//        if (healthPercentage < _almostDeadThreshold)
//        {
//            return _almostDead;
//        }
//
//        return null;
//    }

    public Sprite GetDectorationSprite(string name)
    {
        for (int i = 0; i < _decorations.Length; i++)
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

    public ConversationData.Character_Conversation GetConversation(string characterID, bool firstConvo = false)
    {
        int characterindex = GetCharacterConversationIndex(characterID);
        List<int> availiableConversations = null;

        if (characterindex == -1)
        {
            return new ConversationData.Character_Conversation();
        }
        if (firstConvo)
        {
            return _conversationDatas[characterindex].GetFirstConversation();
        }
        else
        {
            return _conversationDatas[characterindex].GetRandomConversation();
        }
        
    }


    public bool IsCharacterConversationExhausted(string characterID)
    {
        int characterindex = GetCharacterConversationIndex(characterID);
        if (characterindex == -1)
        {
            return true;
        }

        return _conversationDatas[characterindex].IsCharacterConversationExhausted();
    }
    
    private int GetCharacterConversationIndex(string characterID)
    {
        for (int i = 0; i < _conversationDatas.Length; i++)
        {
            if (_conversationDatas[i].PlantCharacterName == characterID)
            {
                return i;
            }
        }

        return -1;
    }

    public void StartNextConversation(string plantName, bool firstConvo)
    {
        ConversationData.Character_Conversation convo = GetConversation(plantName, firstConvo);
        if (convo.Conversation == null || convo.Conversation.Length == 0)
        {
           PlantManager.Instance.ConversationComplete();
            return;
        }

        MenuController.Instance.StartConversation(convo);
    }

    public string GetNextCharacterAvailiableForConversation()
    {
        for (int i = 0; i < _conversationDatas.Length; i++)
        {
            if (!_conversationDatas[i].IsCharacterConversationExhausted())
            {
                return _conversationDatas[i].PlantCharacterName;
            }
        }

        return String.Empty;
    }
}
