using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceDragConsumer : DragConsumer
{
    [SerializeField] private GameObject _hoverEffect;
    [SerializeField] private BasePlant _plant;

     public void OnHover()
    {
        //Debug.Log(string.Format("<color=purple>ON DRAG IN EVENT</color>"));
        _hoverEffect.SetActive(true);
    }
    
     public void OnLeave()
    {
        //Debug.Log(string.Format("<color=purple>ON DRAG Out EVENT</color>"));
        _hoverEffect.SetActive(false);
    }
    
     public void OnDrop()
    {
       // Debug.Log(string.Format("<color=purple>ON DRAG Let Go</color>"));
        _hoverEffect.SetActive(false);
        //I'm Sorry ;_;
        DraggableResource test = DraggableObject.currentDraggableObject.gameObject.GetComponent<DraggableResource>();
        if (test != null)
        {
            _plant.AddResource(test.Resource);
        }
    }
    
}
