using UnityEngine;
using System.Collections;

public class CheckStraightPosition : MonoBehaviour {

	bool isStartPosition;
	bool isInStraightZone;
	bool isTriggeredMsg;
	static int END_STRAIGHT_ZERO = -1536;
	static int END_STRAIGHT_ONE = -1;
	static int END_STRAIGHT_TWO = 3981;
	static int END_STRAIGHT_THREE = -1901;
	static int END_STRAIGHT_FOUR = 2776;
	float positionToTriggerMsg;
	bool[] triggeredStraightZones = new bool[5];
	static int STRAIGHT_ZERO = 0;
	static int STRAIGHT_ONE = 1;
	static int STRAIGHT_TWO = 2;
	static int STRAIGHT_THREE = 3;
	static int STRAIGHT_FOUR = 4;
	bool randomFixed;
	static float MAX_SPEED;
	public GameObject car;
	Rigidbody carRigidBody;

	void Start () {
		setHasAlreadyBeenTriggered(STRAIGHT_ZERO);
		carRigidBody = car.GetComponent<Rigidbody>();
		MAX_SPEED = DataKeeper.dk.maxSpeed;
	}
		
	// If sound off AND in straight AND looking AND want to trigger message AND steering within threshold AND speed over 85km/h (highest pitch)
	void Update () {
		if(!DataKeeper.dk.isSoundOn && isInStraightZone && 
		   					DataKeeper.dk.isLookingForMsgTrigger && DataKeeper.dk.steeringInDegrees < DataKeeper.dk.steeringDegThresholdForMsgTrigger &&
		   					(carRigidBody.velocity.magnitude*3.6f) >= MAX_SPEED){

			if(DataKeeper.dk.straightZoneNumber == STRAIGHT_ZERO && !hasAlreadyBeenTriggered(STRAIGHT_ZERO)){
				float posZ = transform.position.z;
				if(!randomFixed){
					positionToTriggerMsg = Random.Range(posZ, END_STRAIGHT_ZERO);
					randomFixed = true;
				}
				if(posZ > positionToTriggerMsg){
					DataKeeper.dk.isTimeToTriggerMsg = true;
					DataKeeper.dk.isLookingForMsgTrigger = false;
					setHasAlreadyBeenTriggered(STRAIGHT_ZERO);
				}
			}
			else if(DataKeeper.dk.straightZoneNumber == STRAIGHT_ONE && !hasAlreadyBeenTriggered(STRAIGHT_ONE)){
				float posZ = transform.position.z;
				if(!randomFixed){
					positionToTriggerMsg = Random.Range(posZ, END_STRAIGHT_ONE);
					randomFixed = true;
				}
				if(posZ > positionToTriggerMsg){
					DataKeeper.dk.isTimeToTriggerMsg = true;
					DataKeeper.dk.isLookingForMsgTrigger = false;
					setHasAlreadyBeenTriggered(STRAIGHT_ONE);

				}
			}
			else if(DataKeeper.dk.straightZoneNumber == STRAIGHT_TWO && !hasAlreadyBeenTriggered(STRAIGHT_TWO)){
				float posX = transform.position.x;
				if(!randomFixed){
					positionToTriggerMsg = Random.Range(posX, END_STRAIGHT_TWO);
					randomFixed = true;

				}
				if(posX > positionToTriggerMsg){
					DataKeeper.dk.isTimeToTriggerMsg = true;
					DataKeeper.dk.isLookingForMsgTrigger = false;
					setHasAlreadyBeenTriggered(STRAIGHT_TWO);
				}
			}
			else if(DataKeeper.dk.straightZoneNumber == STRAIGHT_THREE && !hasAlreadyBeenTriggered(STRAIGHT_THREE)){
				float posZ = transform.position.z;
				if(!randomFixed){
					positionToTriggerMsg = Random.Range(posZ, END_STRAIGHT_THREE);
					randomFixed = true;
				} 									
				if(posZ < positionToTriggerMsg){			
					DataKeeper.dk.isTimeToTriggerMsg = true;
					DataKeeper.dk.isLookingForMsgTrigger = false;
					setHasAlreadyBeenTriggered(STRAIGHT_THREE);
				}
			}
			else if(DataKeeper.dk.straightZoneNumber == STRAIGHT_FOUR && !hasAlreadyBeenTriggered(STRAIGHT_FOUR)){
				float posX = transform.position.x;
				if(!randomFixed){
					positionToTriggerMsg = Random.Range(posX, END_STRAIGHT_FOUR);
					randomFixed = true;
				} 									
				if(posX < positionToTriggerMsg){			
					DataKeeper.dk.isTimeToTriggerMsg = true;
					DataKeeper.dk.isLookingForMsgTrigger = false;
					setHasAlreadyBeenTriggered(STRAIGHT_FOUR);
				}
			}
		}
	}

	void OnTriggerExit(Collider other){
			if(other.tag == "Straight0" || other.tag=="Straight1" || other.tag == "Straight2" || other.tag=="Straight3" || other.tag=="Straight4" ){
				isInStraightZone = false;
				positionToTriggerMsg = 0;
				randomFixed = false;
			}
	}
	
	void OnTriggerEnter(Collider other){
			string tag = other.tag;
			switch (tag)
			{
			case "Straight0":
				DataKeeper.dk.straightZoneNumber = STRAIGHT_ZERO;
				isInStraightZone = true;
				if(!hasAlreadyBeenTriggered(STRAIGHT_ZERO))
					DataKeeper.dk.isLookingForMsgTrigger = true;
				break;
			case "Straight1":
				DataKeeper.dk.straightZoneNumber = STRAIGHT_ONE;
				isInStraightZone = true;
				if(!hasAlreadyBeenTriggered(STRAIGHT_ONE))
					DataKeeper.dk.isLookingForMsgTrigger = true;
			break;
			case "Straight2":
				DataKeeper.dk.straightZoneNumber = STRAIGHT_TWO;
				isInStraightZone = true;
				if(!hasAlreadyBeenTriggered(STRAIGHT_TWO))
					DataKeeper.dk.isLookingForMsgTrigger = true;
				break;
			case "Straight3":
				DataKeeper.dk.straightZoneNumber = STRAIGHT_THREE;
				isInStraightZone = true;
				if(!hasAlreadyBeenTriggered(STRAIGHT_THREE))
					DataKeeper.dk.isLookingForMsgTrigger = true;
				break;
			case "Straight4":
				DataKeeper.dk.straightZoneNumber = STRAIGHT_FOUR;
				isInStraightZone = true;
				if(!hasAlreadyBeenTriggered(STRAIGHT_FOUR))
					DataKeeper.dk.isLookingForMsgTrigger = true;
				break;
			default:
				break;
			}
	}
		
	bool hasAlreadyBeenTriggered(int straight){
		return (triggeredStraightZones[straight]);
	}

	void setHasAlreadyBeenTriggered(int straight){
		for (int i = 0; i < triggeredStraightZones.Length; i++){
			if(i == straight)
				triggeredStraightZones[i] = true;
			else
				triggeredStraightZones[i] = false;
		}
	}
}
