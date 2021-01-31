using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableResource : DraggableObject
{
    [SerializeField] private GameDataMonoSingleton.RESOURCE_TYPE _resource;

    public GameDataMonoSingleton.RESOURCE_TYPE Resource => _resource;


    public override void AudioHookGrab()
    {
        Debug.Log("DraggableResource AudioHookGrab " + _resource);
    }

    public override void AudioHookHoverIn()
    {
        Debug.Log("DraggableResource AudioHookHoverIn" + _resource);
    }

    public override void AudioHookHoverOut()
    {
        Debug.Log("DraggableResource AudioHookHoverOut" + _resource);
    }

    public override void AudioHookLetGo()
    {
        Debug.Log("DraggableResource AudioHookLetGo" + _resource);
    }
}
