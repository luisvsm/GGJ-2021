using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ConversationData", order = 1)]
public class ConversationData : ScriptableObject
{
    public static string PLAYER_ID = "player";
    //don't do this at home kids passing structs sucks for memory but I needed a quick editor and this hack works :) 
    [Serializable]
    public struct Conversation_Line
    {
        public string SpeakerName;
        public string ConversationText;
        public string AudioID;

    }

    [Serializable]
    public struct Character_Conversation
    {
        public string ConversationID;
        public Conversation_Line[] Conversation;
        public bool GreetingConversation;
        public bool OnceOnly;
    }

    [SerializeField] private string _plantCharacterName;
    [Header("Leave the speakerName empty to link the line to the player")]
    [SerializeField] private Character_Conversation[] _conversationList;

    private List<string> _playedConversations;

    public string PlantCharacterName => _plantCharacterName;
    

    public Character_Conversation GetFirstConversation()
    {
        for (int i = 0; i < _conversationList.Length; i++)
        {
            if (_conversationList[i].GreetingConversation)
            {
                if (!_playedConversations.Contains(_conversationList[i].ConversationID))
                {
                    _playedConversations.Add(_conversationList[i].ConversationID);
                }
                
                return _conversationList[i];
            }
        }

        return GetRandomConversation();
    }

    public Character_Conversation GetRandomConversation()
    {
        if (_playedConversations == null)
        {
            _playedConversations = new List<string>();
        }

        List<int> availiableConversations = null;
        for (int i = 0; i < _conversationList.Length; i++)
        {
            if (!_playedConversations.Contains(_conversationList[i].ConversationID))
            {
                if (availiableConversations == null)
                {
                    availiableConversations = new List<int>();
                }

                availiableConversations.Add(i);
            }
        }

        if (availiableConversations != null)
        {
            int randomAvailiableIndex = Random.Range(0, availiableConversations.Count);
            int randomConversationIndex = availiableConversations[randomAvailiableIndex];
            if (!_playedConversations.Contains(_conversationList[randomConversationIndex].ConversationID))
            {
                _playedConversations.Add(_conversationList[randomConversationIndex].ConversationID);
            }
            return _conversationList[randomConversationIndex];
        }

        return new Character_Conversation();
    }

    public bool IsCharacterConversationExhausted()
    {
        return _playedConversations.Count == _conversationList.Length;
    }
}
