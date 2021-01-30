using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConversationLine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _icon;

    //don't do this at home kids passing structs sucks :) 
    public void InitialiseConversation(ConversationData.Conversation_Line line)
    {
        if (_icon != null && string.IsNullOrEmpty(line.SpeakerName))
        {
            _icon.sprite = PlayerInventoryMonoSingleton.Instance.GetPlantIcon(line.SpeakerName);
        }

        _text.text = line.ConversationText;
    }
}
