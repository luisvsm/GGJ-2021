using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    [SerializeField] private GameObject _plantGameObject;
    [SerializeField] private GameObject _playerGameObject;
    
    [SerializeField] private ConversationLine _plantConversationLine;
    [SerializeField] private ConversationLine _playerConversationLine;

    private int _nextline = 0;

    private ConversationData.Conversation_Line[] _lines;
    //don't do this at home kids passing structs sucks :) 
    public void InitialiseConversation(ConversationData.Conversation_Line[] lines)
    {
        _lines = lines;
        DisplaytNextLine();
    }

    public void DisplaytNextLine()
    {
        Debug.Log(string.Format("<color=purple>nextline index {0} in game data </color>", _nextline));

        if (_nextline >= _lines.Length)
        {
            _nextline = 0;
            _lines = null;
            MenuController.Instance.EndConversation();
		}

        if (_lines != null)
        {
            bool isPlayer = string.IsNullOrEmpty(_lines[_nextline].SpeakerName);
            
            _plantGameObject.SetActive(!isPlayer);
            _playerGameObject.SetActive(isPlayer);

            if (isPlayer)
            {
                _playerConversationLine.InitialiseConversation(_lines[_nextline]);
				AudioController.Play("SFX_Generic_Tap");
			}
            else
            {
                _plantConversationLine.InitialiseConversation(_lines[_nextline]);
				AudioController.Play("SFX_Generic_Tap");
			}

            _nextline++;
        }
    }

    private void OnMouseDown()
    {
        DisplaytNextLine();
	}
}
