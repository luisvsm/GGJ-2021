using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClockStone;

public class Ed_AudioGenericTriggers : MonoBehaviour
{
	// Ed's basic script for basic triggers
	public void GenericGrab() { AudioController.Play("SFX_Generic_Grab"); }
	public void GenericDrop() { AudioController.Play("SFX_Generic_Drop"); }
	public void GenericTap() { AudioController.Play("SFX_Generic_Tap"); }
	public void GenericTransition() { AudioController.Play("SFX_Generic_Transition"); }
	public void GrabPoo() { AudioController.Play("SFX_Poo_Grab"); }
	public void GrabPlant() { AudioController.Play("SFX_PotPlant_Move"); }
	public void GrabWaterCan() { AudioController.Play("SFX_WaterCan_Grab"); }
	public void Custom(string audioToPlay) { AudioController.Play(audioToPlay); }
	public void CustomStop(string audioToStop) { AudioController.Stop(audioToStop); }
}
