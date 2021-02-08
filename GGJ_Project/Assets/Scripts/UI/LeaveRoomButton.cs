using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveRoomButton : MonoBehaviour
{
    [SerializeField] private Button _leavebutton;
    [SerializeField] private Image image;
    [SerializeField] public List<Sprite> roomButtons;

    private void Start()
    {
       RoomController.Instance.OnRoomChanged += OnRoomChanged;
       _leavebutton.interactable = RoomController.Instance.CanGoBack();
    }

    private void OnDestroy()
    {
        RoomController.Instance.OnRoomChanged -= OnRoomChanged;
    }

    private void OnRoomChanged(int roomNumber)
    {
        _leavebutton.interactable = RoomController.Instance.CanGoBack();
        SetSprite(roomNumber);
    }

    // Start is called before the first frame update
    public void GoBack()
    {
        RoomController.Instance.GoBack();
    }
    
    public void SetSprite(int roomNumber){
        if(roomButtons[roomNumber] == null){
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
            image.sprite = roomButtons[roomNumber];
        }
    }
    
}
