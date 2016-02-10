using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GUIHandler : MonoBehaviour {
	public GameObject car;
	Rigidbody carRigidbody;
	CarController carControllerScript;
	FrameLimiter frameLimiterScript;
	public GUISkin guiSkin;
	bool escapePressed;
	bool debuggingEnabled;
	bool yesButtonPressed;
	public AudioSource quitSound;
	AudioSource soundSource;
	string titleScene = "Title";
	string pathToWarningSounds = "Sounds/sound";

	void Start () {
		soundSource = GetComponent<AudioSource>();
		AudioClip clip = (AudioClip)Resources.Load(pathToWarningSounds+DataKeeper.dk.currentSignal);
		soundSource.clip = clip;
		soundSource.volume = 1;
		carRigidbody = car.GetComponent<Rigidbody>();
		carControllerScript = car.GetComponent<CarController>();
		frameLimiterScript = GetComponent<FrameLimiter>();
		Time.timeScale = 1;
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			escapePressed = !escapePressed;
			yesButtonPressed = false;
			if(escapePressed)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}	
	}

	void OnGUI () {
		if (Input.GetKeyDown(KeyCode.F)) 
			Screen.fullScreen = !Screen.fullScreen; 

		GUI.skin = guiSkin;
		if(escapePressed){
			guiSkin.label.fontSize=30;
			guiSkin.label.alignment = TextAnchor.MiddleCenter;
			if(!yesButtonPressed){
				guiSkin.label.fontSize=30;
				guiSkin.label.alignment = TextAnchor.MiddleCenter;
				GUI.Label(new Rect(Screen.width*0.3f,200,600,70), "QUIT?");
				if(GUI.Button(new Rect(Screen.width*0.3f+170f,295,250,70),"YES")){
					if(DataKeeper.dk.isPractice){
						Time.timeScale = 1;
						StartCoroutine("Quit");
					}else
						yesButtonPressed = true;

				} else if(GUI.Button(new Rect(Screen.width*0.3f+170f,395,250,70),"NO")){
					escapePressed = false;
					Time.timeScale = 1;
				}
				if(GUI.Button(new Rect(Screen.width*0.3f+170f,495,250,70),"SOUND")){
					soundSource.Play();
				}
			}else {
				GUI.Label(new Rect(Screen.width*0.3f,200,600,70), "SAVE STATS?");
				if(GUI.Button(new Rect(Screen.width*0.3f+170f,295,250,70),"YES")){
					int numberOfRuns = PlayerPrefs.GetInt(DataKeeper.dk.currentPlayerName+"Runs");
					numberOfRuns++;
					PlayerPrefs.SetInt(DataKeeper.dk.currentPlayerName+"Runs", numberOfRuns);
					PlayerPrefs.SetInt(DataKeeper.dk.currentPlayerName+"Signal"+
					                   DataKeeper.dk.currentSignal,1); 
					Time.timeScale = 1;
					StartCoroutine("Quit");
				} else if(GUI.Button(new Rect(Screen.width*0.3f+170f,395,250,70),"NO")){
					Time.timeScale = 1;
					StartCoroutine("Quit");
				}
			}
		}

		if(debuggingEnabled){
			if(!escapePressed ){
			guiSkin.label.fontSize=15;
			guiSkin.label.alignment = TextAnchor.UpperLeft;
			GUILayout.Label("Instructions: (F)ullscreen, (G)UI on/off, S=Get FPS, X=Camera change");
			GUILayout.Label ("km/h: "+carRigidbody.velocity.magnitude * 3.6f);
			GUILayout.Label ("Throttle: "+carControllerScript.AccelOrBrakeKey);
			GUILayout.Label ("Steering: "+DataKeeper.dk.steeringInDegrees);
			GUILayout.Label("FPS: "+frameLimiterScript.FpsInfoString);
			GUILayout.Label("Forced Lane Dep: "+DataKeeper.dk.isForcedLaneDeparture);
			GUILayout.Label ("Auto steering: "+DataKeeper.dk.autoSteeringEnabled);
			GUILayout.Label ("Random: "+DataKeeper.dk.autoSteeringPlusOrMinus);
			GUILayout.Label ("isLookingForMsgTrigger: "+DataKeeper.dk.isLookingForMsgTrigger);
			GUILayout.Label ("Zone: "+DataKeeper.dk.straightZoneNumber);
			GUILayout.Label ("TOT ELAP TIME: "+ DataKeeper.dk.totalElapsedAutoSteeringTime);
			GUILayout.Label ("NUM LANE DEPS: "+DataKeeper.dk.numberOfForcedLaneDepartures);
			GUILayout.Label ("AVG TIME TO DEPARTURE: "+ DataKeeper.dk.totalElapsedAutoSteeringTime/DataKeeper.dk.numberOfForcedLaneDepartures);
			}	
		}
	}

	IEnumerator Quit(){
		AudioClip clip = (AudioClip)Resources.Load("Sounds/powerup10");
		quitSound.clip = clip;
		quitSound.volume = 0.3f;
		quitSound.Play ();
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(titleScene);
	}

	public bool DebuggingEnabled{
		get {return this.debuggingEnabled;}
		set {this.debuggingEnabled = value;}
	}
}
