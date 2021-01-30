using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConversation : MonoBehaviour
{
    [SerializeField] private BasePlant _plant;
    [SerializeField] private Collider2D _collider;

    public void Activate()
    {
        _collider.enabled = true;
    }
    
    
    public void Deactivate()
    {
        _collider.enabled = false;
    }
    
    void OnMouseDown()
    {
        _plant.StartConversation();
    }
}
