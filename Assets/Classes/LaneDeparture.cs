using UnityEngine;
using System.Collections;

public class LaneDeparture {
	string playerName;
	char departureSide;
	float steeringAngle;
	float reactionTime;
	int signalType;
	string timeStamp;
	bool isValidDeparture;
	bool isCorrectResponse;
	int testNumber;
	int straightZoneNumber;

	public LaneDeparture(string player, float reactionTime, float steering, int signal, 
	               char depSide, int straightZoneNumber, bool isValidDeparture, bool isCorrectResponse, int testNumber)
	{
		timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff");
		this.playerName = player;
		this.reactionTime = reactionTime;
		this.steeringAngle = steering;
		this.signalType = signal;
		this.departureSide = depSide;
		this.isValidDeparture = isValidDeparture;
		this.isCorrectResponse = isCorrectResponse;
		this.testNumber = testNumber;
		this.straightZoneNumber = straightZoneNumber;
	}

	public string GetInfo(){
		return  ""+timeStamp+", "+playerName+", "+reactionTime+", "+steeringAngle+", "+signalType+", "+
			departureSide+", "+straightZoneNumber+", "+isValidDeparture+", "+isCorrectResponse+", "+testNumber;
	}
}
