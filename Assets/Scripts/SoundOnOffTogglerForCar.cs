using UnityEngine;
using System.Collections;

public class SoundOnOffTogglerForCar : MonoBehaviour {
	bool soundOn;
	bool isTriggered;
	float startTime;
	float endTime;
	
	void OnTriggerExit(Collider other){
		if(other.tag == "RoadSmall"){
				soundOn = !soundOn;
				isTriggered = true;
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "RoadSmall"){
			if (soundOn){
				soundOn = !soundOn;
			}
			isTriggered = false;
		}
	}
		
	public bool IsTriggered{
		get {return this.isTriggered;}
		set {this.isTriggered = value;}
	}
}