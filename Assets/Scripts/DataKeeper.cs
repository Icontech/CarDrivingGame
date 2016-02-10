using UnityEngine;
using System.Collections;

public class DataKeeper : MonoBehaviour {
	public static DataKeeper dk;
	public static int totalNumberOfSignalsToTest = 4;
	public int practiceSignal = totalNumberOfSignalsToTest;
	public string currentPlayerName;
	public int currentSignal;
	public ArrayList laneDepartures;
	public int numberOfRuns;
	public int[]signals= new int[totalNumberOfSignalsToTest];
	public bool isLoggedIn;
	public float startTriggerTime;
	public float startPlayTime;
	public int numberOfLaneDepartures;
	public bool isForcedLaneDeparture; 
	public bool autoSteeringEnabled;
	public int autoSteeringPlusOrMinus;
	public float degreesOfRotation;
	public bool isMessageReceived;
	public int maxSteeringAngle;
	public float maxSpeed;
	public float autoSteeringAngle;
	public bool isSoundOn;
	public bool isTimeToTriggerMsg;
	public bool isLookingForMsgTrigger;
	public float steeringInDegrees;
	public float steeringDegThresholdForMsgTrigger;
	public int straightZoneNumber;
	public int steeringDegThresholdForResponse;
	public bool randomMsgsEnabled;
	public bool isPractice;
	public ArrayList remainingSignals;
	public ArrayList testedSignals;
	public float autoSteeringStartTime;
	public float autoSteeringMaxTime;
	public int numberOfForcedLaneDepartures;
	public float totalElapsedAutoSteeringTime;
	public float startSteeringDegreeAtAutoSteeringTriggered;

	void Awake(){
		if (dk == null) {
			DontDestroyOnLoad(gameObject);
			dk = this;
		} else if(dk != this){
			Destroy(gameObject);
		}
	}
		
	void Start(){
		laneDepartures = new ArrayList();
		remainingSignals = new ArrayList();
		testedSignals = new ArrayList();
	}

	void Update(){
		numberOfLaneDepartures = laneDepartures.Count;
	}

	public float DegreesTurned(float steering){
		float degrees = steering*(DataKeeper.dk.degreesOfRotation);
		return degrees;
	}
}
