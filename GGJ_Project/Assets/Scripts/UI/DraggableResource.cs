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
		if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.love)
			AudioController.Play("SFX_Generic_Grab");
		else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.poo)
			AudioController.Play("SFX_Poo_Grab");
		else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.sun)
			AudioController.Play("SFX_Generic_Grab");
		else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.water)
			AudioController.Play("SFX_WaterCan_Grab");
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
		if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.love)
			AudioController.Play("SFX_Give_Love");
		else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.poo)
			AudioController.Play("SFX_Give_Poop");
		else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.sun)
			AudioController.Play("SFX_Give_Sun");
		else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.water)
			AudioController.Play("SFX_Give_Water");
	}
}
