using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MenuController : MonoBehaviourSingleton<MenuController>
{
    public List<Animator> MenuListAnimators;

    //TODO:add animation when se get there 
    public Conversation _conversationBinder;
        
    public void showMenu(int menuIndex)
    {
        for (int i = 0; i < MenuListAnimators.Count; i++)
        {
            MenuListAnimators[i].SetBool("open", menuIndex==i);
        }
    }

    public void StartConversation(ConversationData.Character_Conversation convo)
    {
       _conversationBinder.gameObject.SetActive(true);
       _conversationBinder.InitialiseConversation(convo.Conversation);
    }

    public void EndConversation()
    {
        _conversationBinder.gameObject.SetActive(false);
        PlantManager.Instance.ConversationComplete();
    }
}
