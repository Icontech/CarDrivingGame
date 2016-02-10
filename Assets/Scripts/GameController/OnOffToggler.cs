using UnityEngine;
using System.Collections;

public class OnOffToggler : MonoBehaviour {
	GUIHandler guiScript;
	System.Diagnostics.Process process;
	public GameObject rightTrigger;
	bool autoSteeringTimeIsOver;
	string pcADBfilePath ="PC PATH TO ADB FILE";	
	string macADBfilePath ="MAC PATH TO ADB FILE";
	string androidAppName = "ANDROID APP NAME TERMINAL COMMAND";


	void Start () {
		if(!DataKeeper.dk.randomMsgsEnabled){
			CheckStraightPosition script = rightTrigger.GetComponent<CheckStraightPosition>();
			script.enabled = false;
		}

		// This process is used to start an Android app on a smartphone connected via USB.
		//The app is used to distract the drivers from the driving task, forcing a lane departure.
		//The paths need to be set in order for this code to work.
		//Also, the "process.Start()" call needs to be uncommented, at the bottom of this class.

		process = new System.Diagnostics.Process();
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardInput = true;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.CreateNoWindow = true;
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor){
			process.StartInfo.FileName = pcADBfilePath;
		}
		else if(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor){
			process.StartInfo.FileName = macADBfilePath;
		}	
		process.StartInfo.Arguments = androidAppName;
		guiScript = GetComponent<GUIHandler>();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.G)){
			guiScript.DebuggingEnabled = !guiScript.DebuggingEnabled;
		}
		if (Input.GetKeyDown(KeyCode.Z)){
			DataKeeper.dk.isForcedLaneDeparture = !DataKeeper.dk.isForcedLaneDeparture;
		}
		if (Input.GetKeyDown(KeyCode.A)){
			autoSteeringTimeIsOver = false;
			if(DataKeeper.dk.isMessageReceived){
				DataKeeper.dk.isMessageReceived = false;
			}
			DataKeeper.dk.autoSteeringEnabled = !DataKeeper.dk.autoSteeringEnabled;
			if(DataKeeper.dk.autoSteeringEnabled){
				DataKeeper.dk.isForcedLaneDeparture = true;
				DataKeeper.dk.autoSteeringStartTime = Time.time;
			} else{
				DataKeeper.dk.isForcedLaneDeparture = false;
			}
			int[]plusOrMinus = {-1,1};
			int random = plusOrMinus[Random.Range(0,2)];
			DataKeeper.dk.autoSteeringPlusOrMinus = random;
		}

		if(DataKeeper.dk.autoSteeringEnabled && !DataKeeper.dk.isSoundOn){
			if(!autoSteeringTimeIsOver){
				float elapsedTime = Time.time - DataKeeper.dk.autoSteeringStartTime;
				if(elapsedTime >= DataKeeper.dk.autoSteeringMaxTime){
					DataKeeper.dk.startSteeringDegreeAtAutoSteeringTriggered = DataKeeper.dk.DegreesTurned(Input.GetAxis("Horizontal"));
					autoSteeringTimeIsOver = true;
				}
			}
			
			if(autoSteeringTimeIsOver){
				float endDegree = DataKeeper.dk.DegreesTurned(Input.GetAxis("Horizontal"));
				float turned = Mathf.Abs (endDegree-DataKeeper.dk.startSteeringDegreeAtAutoSteeringTriggered);
				if(turned > DataKeeper.dk.steeringDegThresholdForResponse){
					DataKeeper.dk.autoSteeringEnabled = false;
				}
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Q) || DataKeeper.dk.isTimeToTriggerMsg){
				DataKeeper.dk.isMessageReceived = !DataKeeper.dk.isMessageReceived;
				DataKeeper.dk.isTimeToTriggerMsg = false;
			if(DataKeeper.dk.isMessageReceived){
				//process.Start(); 
				DataKeeper.dk.isMessageReceived = false;
			}
		}
	}
}
