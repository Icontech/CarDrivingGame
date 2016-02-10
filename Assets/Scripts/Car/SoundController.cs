using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Drivetrain))]
public class SoundController : MonoBehaviour {
	public AudioClip engine;
	public AudioClip start;
	AudioSource engineSource;
	AudioSource startSource;
	Drivetrain drivetrain;
	bool gamePaused;
	
	AudioSource CreateAudioSource (AudioClip clip, bool isLoop, float vol, float delay) {
		GameObject go = new GameObject("audio");
		go.transform.parent = transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.AddComponent(typeof(AudioSource));
		go.GetComponent<AudioSource>().clip = clip;
		go.GetComponent<AudioSource>().loop = isLoop;
		go.GetComponent<AudioSource>().volume = vol;
		go.GetComponent<AudioSource>().PlayDelayed(delay);
		AudioSource audio = go.GetComponent<AudioSource>();  
		audio.bypassEffects = true;
		return audio;
	}
	
	void Start () {
		engineSource = CreateAudioSource(engine, true,0,0.7f);
		startSource = CreateAudioSource(start, false,0.2f,0);
		drivetrain = GetComponent (typeof (Drivetrain)) as Drivetrain;
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			gamePaused = !gamePaused;
		}
		if(gamePaused){
			engineSource.volume = 0;
		} else {
			engineSource.pitch = 0.3f + 1.1f * drivetrain.rpm / drivetrain.maxRPM;
			engineSource.volume = 0.2f + 0.1f * drivetrain.throttle;
		}
	}
}
