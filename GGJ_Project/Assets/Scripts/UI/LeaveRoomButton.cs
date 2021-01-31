using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveRoomButton : MonoBehaviour
{
    [SerializeField] private Button _leavebutton;
    private void Start()
    {
       RoomController.Instance.OnRoomChanged += OnRoomChanged;
       _leavebutton.interactable = RoomController.Instance.CanGoBack();
    }

    private void OnDestroy()
    {
        RoomController.Instance.OnRoomChanged -= OnRoomChanged;
    }

    private void OnRoomChanged()
    {
        _leavebutton.interactable = RoomController.Instance.CanGoBack();
    }

    // Start is called before the first frame update
    public void GoBack()
    {
        RoomController.Instance.GoBack();
    }
    
}
