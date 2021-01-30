using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int _nextConversationIndex = -1;
    private bool conversationExhausted = false;

    public bool ConversationExhausted => conversationExhausted;

    public string PlantCharacterName => _plantCharacterName;

    public Character_Conversation GetNextConversation()
    {
        _nextConversationIndex++;

        if (_nextConversationIndex < _conversationList.Length)
        {
            //todo:WIP to stop repeating conversations can easily change.
            if (_nextConversationIndex == _conversationList.Length - 1)
            {
                conversationExhausted = true;
            }
            return _conversationList[_nextConversationIndex];;
        }

        return new Character_Conversation();
    }
}
