using UnityEngine;
using System.Collections;
using System;

public class SoundTrigger : MonoBehaviour {
	public float startGain;
	private AudioSource soundSource;
	enum Fade {In, Out};
	public bool soundIsOn;
	public GameObject right;
	public GameObject left;
	SoundOnOffTogglerForCar rightTrigger;
	SoundOnOffTogglerForCar leftTrigger;

	void Start(){
		soundSource = GetComponent<AudioSource>();
		AudioClip clip = (AudioClip)Resources.Load("Sounds/sound"+DataKeeper.dk.currentSignal);
		soundSource.clip = clip;
		soundSource.volume = startGain;
		rightTrigger = right.GetComponent<SoundOnOffTogglerForCar>();
		leftTrigger = left.GetComponent<SoundOnOffTogglerForCar>();
	}

	void Update(){
		//TURN ON SOUND
		if (rightTrigger.IsTriggered && !leftTrigger.IsTriggered && !soundIsOn){
			ToggleSoundOnOff(0.5f);
		}
		if (leftTrigger.IsTriggered && !rightTrigger.IsTriggered && !soundIsOn){
			ToggleSoundOnOff(-0.5f);
		}
		//TURN OFF SOUND IF ON
		if(!rightTrigger.IsTriggered && !leftTrigger.IsTriggered && soundIsOn)
			ToggleSoundOnOff(0);
	}
		
	public void ToggleSoundOnOff(float panning){
		soundIsOn = !soundIsOn;
		DataKeeper.dk.isSoundOn = soundIsOn;
		if (soundIsOn){
			soundSource.panStereo = panning;
			soundSource.Play();
		}
	}
}
