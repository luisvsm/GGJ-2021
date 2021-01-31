using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceDragCompostConsumer : DragConsumer
{
    [SerializeField] private GameObject _hoverEffect;

    public void OnHover()
    {
        // Debug.Log(string.Format("<color=purple>ON DRAG IN EVENT</color>"));
        _hoverEffect.SetActive(true);
    }

    public void OnLeave()
    {
        // Debug.Log(string.Format("<color=purple>ON DRAG Out EVENT</color>"));
        _hoverEffect.SetActive(false);
    }

    public void OnDrop()
    {
        // Debug.Log(string.Format("<color=purple>ON DRAG Let Go</color>"));
        _hoverEffect.SetActive(false);
        //I'm Sorry ;_; (Anna)
        // Me also :3 (Luis)
        DraggableResource test = DraggableObject.currentDraggableObject.gameObject.GetComponent<DraggableResource>();
        if (test != null)
        {
            if (test.Resource == GameDataMonoSingleton.RESOURCE_TYPE.leafLitter)
            {
                Destroy(test.gameObject);
                SpawnLeafLitter.LeafLitterWasDestroyed();
				AudioController.Play("SFX_Generic_Grab");
            }
        }
    }

}
