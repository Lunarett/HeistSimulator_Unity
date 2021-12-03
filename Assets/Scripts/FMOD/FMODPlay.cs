using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODPlay : MonoBehaviour
{
	[FMODUnity.EventRef]
	[SerializeField] private string _amb;

	private FMOD.Studio.EventInstance _eventInst;

	// Start is called before the first frame update
	void Start()
	{
		_eventInst = FMODUnity.RuntimeManager.CreateInstance(_amb);
		_eventInst.start();
	}

	public void StopAudio()
	{
		_eventInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}
}
