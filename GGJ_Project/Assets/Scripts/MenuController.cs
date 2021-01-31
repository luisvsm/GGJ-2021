using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviourSingleton<MenuController>
{
    public List<Animator> MenuListAnimators;

    //TODO:add animation when se get there 
    public Conversation _conversationBinder;
    public GameObject _backbutton;
    private int _activeMenuIndex;
    
    public void showMenu(int menuIndex)
    {
        for (int i = 0; i < MenuListAnimators.Count; i++)
        {
            MenuListAnimators[i].SetBool("open", menuIndex==i);
            if (menuIndex == i)
            {
                _activeMenuIndex = i;
            }
        }
    }
    
    public void HideAllMenu()
    {
        for (int i = 0; i < MenuListAnimators.Count; i++)
        {
            MenuListAnimators[i].SetBool("open", false);
        }
    }

    public void StartConversation(ConversationData.Character_Conversation convo)
    {
        HideAllMenu();
        _backbutton.SetActive(false);
        _conversationBinder.gameObject.SetActive(true);
        _conversationBinder.InitialiseConversation(convo.Conversation);
        GameDataMonoSingleton.Instance.SetTickerPaused(true);
    }

    public void EndConversation()
    {
        _backbutton.SetActive(true);
        _conversationBinder.gameObject.SetActive(false);
        showMenu(_activeMenuIndex);
        PlantManager.Instance.ConversationComplete();
        GameDataMonoSingleton.Instance.SetTickerPaused(false);
    }
}
