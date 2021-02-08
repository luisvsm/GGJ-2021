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
		{
			AudioController.Play("SFX_Generic_Grab");
			//AudioController.Play("SFX_Generic_Drop");
		}
		else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.poo)
		{
			AudioController.Play("SFX_Poo_Grab");
			//AudioController.Play("SFX_Generic_Drop");
		}
		else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.sun)
		{
			AudioController.Play("SFX_Generic_Grab");
			//AudioController.Play("SFX_Generic_Drop");
		}
		else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.water)
		{
			AudioController.Play("SFX_WaterCan_Grab");
			//AudioController.Play("SFX_Generic_Drop");
		}
	}
	
	public override void StartDragging(){
		
		if ( _resource == GameDataMonoSingleton.RESOURCE_TYPE.poo) {
			// Check that we have the poo
			if(PlayerInventoryMonoSingleton.Instance.HasPoo())
				base.StartDragging();
		}else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.water ) {
			// Check that we have the water 
			if(PlayerInventoryMonoSingleton.Instance.HasWater())
				base.StartDragging();
		}else{
			// Else it's not a resource that we need to track
			base.StartDragging();
		}
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
		{
			AudioController.Play("SFX_Give_Love");
			AudioController.Play("SFX_Generic_Drop");
		}
		else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.poo)
		{
			AudioController.Play("SFX_Give_Poop");
			AudioController.Play("SFX_Generic_Drop");
		}
		else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.sun)
		{
			AudioController.Play("SFX_Give_Sun");
			AudioController.Play("SFX_Generic_Drop");
		}
		else if (_resource == GameDataMonoSingleton.RESOURCE_TYPE.water)
		{
			AudioController.Play("SFX_Give_Water");
			AudioController.Play("SFX_Generic_Drop");
		}
	}
}
