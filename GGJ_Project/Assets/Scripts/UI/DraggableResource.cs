using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableResource : DraggableObject
{
    [SerializeField] private GameDataMonoSingleton.RESOURCE_TYPE _resource;

    public GameDataMonoSingleton.RESOURCE_TYPE Resource => _resource;
}
