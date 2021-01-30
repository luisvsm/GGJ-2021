using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    [SerializeField] private GameObject _plantGameObject;
    [SerializeField] private GameObject _playerGameObject;
    
    [SerializeField] private ConversationLine _plantConversationLine;
    [SerializeField] private ConversationLine _playerConversationLine;
    
    //don't do this at home kids passing structs sucks :) 
    public void InitialiseConversation(ConversationData.Conversation_Line[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            bool isPlayer = string.IsNullOrEmpty(lines[i].SpeakerName) ;
            
            _plantGameObject.SetActive(!isPlayer);
            _playerGameObject.SetActive(isPlayer);

            if (isPlayer)
            {
                _playerConversationLine.InitialiseConversation(lines[i]);
            }
            else
            {
                _plantConversationLine.InitialiseConversation(lines[i]);
            }
        }
    }
}
