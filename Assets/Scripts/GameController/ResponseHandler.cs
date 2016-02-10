using UnityEngine;
using System.Collections;

public class ResponseHandler : MonoBehaviour {
	public GameObject right;
	public GameObject left;
	SoundOnOffTogglerForCar rightTrigger;
	SoundOnOffTogglerForCar leftTrigger;
	bool isRecording;
	bool hasCompletedRecording;
	float startTime;
	float startDegreeAtSoundTriggered;
	float turnedDegreeAfterSoundTriggered;
	char departureSide;
	float responseTime;
	float time;

	void Start () {
		rightTrigger = right.GetComponent<SoundOnOffTogglerForCar>();
		leftTrigger = left.GetComponent<SoundOnOffTogglerForCar>();
	}

	void Update () {
		CheckIfSteeringWheelTurned();
	}

	void CheckIfSteeringWheelTurned(){
		//If no lane departure detected: Keep looking
		if (!isRecording && !hasCompletedRecording){
			//IF either side is triggered: start recording
			if ((rightTrigger.IsTriggered && !leftTrigger.IsTriggered) || (leftTrigger.IsTriggered && !rightTrigger.IsTriggered)){
				DataKeeper.dk.totalElapsedAutoSteeringTime+=(Time.time-DataKeeper.dk.autoSteeringStartTime);
				DataKeeper.dk.numberOfForcedLaneDepartures++;
				if(leftTrigger.IsTriggered)
					departureSide = 'L';
				else
					departureSide = 'R';
				isRecording = true;
				startTime = Time.time;
				startDegreeAtSoundTriggered = DataKeeper.dk.DegreesTurned(Input.GetAxis("Horizontal"));

			}
		}

		//If we're recording and haven't received a response yet
		else if(isRecording && !hasCompletedRecording) {
			float endDegree = DataKeeper.dk.DegreesTurned(Input.GetAxis("Horizontal"));
			turnedDegreeAfterSoundTriggered = Mathf.Abs (endDegree-startDegreeAtSoundTriggered);

			//IF turned degree over threshold: Valid response, create LaneDeparture object and start looking again
			if(turnedDegreeAfterSoundTriggered > DataKeeper.dk.steeringDegThresholdForResponse){
				DataKeeper.dk.autoSteeringEnabled = false;
				bool isCorrectResponse = false;
				responseTime = Time.time - startTime;
				if((departureSide == 'R' && endDegree < startDegreeAtSoundTriggered) || 
				   											(departureSide == 'L' && endDegree > startDegreeAtSoundTriggered))
				{
					isCorrectResponse = true;
				}
				RegisterLaneDeparture(responseTime,turnedDegreeAfterSoundTriggered,isCorrectResponse);
				isRecording = false;
				hasCompletedRecording = true;
			} 
			else if(!rightTrigger.IsTriggered && !leftTrigger.IsTriggered){
				isRecording = false;
				hasCompletedRecording = true;
			} 
		}
		//If recording completed, check if we are back on track and can start looking for a new departure
		else if(hasCompletedRecording){
			if(!rightTrigger.IsTriggered && !leftTrigger.IsTriggered){
				hasCompletedRecording = false;
				DataKeeper.dk.isForcedLaneDeparture = false;
			}
		}
	}

	void RegisterLaneDeparture(float time, float degree, bool isCorrectResponse){
		bool isForcedLaneDeparture = DataKeeper.dk.isForcedLaneDeparture;
		int testNumber = PlayerPrefs.GetInt(DataKeeper.dk.currentPlayerName+"Runs")+1;
		int signalTested = DataKeeper.dk.currentSignal;
		int zoneNumber = -1;
		if(DataKeeper.dk.isForcedLaneDeparture){
			zoneNumber = DataKeeper.dk.straightZoneNumber;
		}
		LaneDeparture ld = new LaneDeparture(DataKeeper.dk.currentPlayerName, time, degree, signalTested,departureSide,zoneNumber,isForcedLaneDeparture,isCorrectResponse,testNumber);
		DataKeeper.dk.laneDepartures.Add(ld);
	}
}
