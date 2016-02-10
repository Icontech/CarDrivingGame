using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Drivetrain))]
public class CarController : MonoBehaviour {

	public bool steeringWheelEnabled;
	Rigidbody rb;
	public Wheel[] wheels;
	public Transform centerOfMass;
	float inertiaFactor = 1.5f;
	float brake;
	float throttle;
	float throttleInput;
	float steering;
	float lastShiftTime = -1;
	Drivetrain drivetrain;
	float shiftSpeed = 0.4f;
	float throttleTime = 1.0f;
	float throttleTimeTraction = 10.0f;
	float throttleReleaseTime = 0.5f;
	float throttleReleaseTimeTraction = 0.1f;
	bool tractionControl = true;
	float steerReleaseTime = 0.3f;
	float veloSteerReleaseTime = 0f;
	float steerCorrectionFactor = 4.0f;
	float steerTime = 0.8f;
	float veloSteerTime = 0.1f;
	float angle;
	Vector3 carDir;
	float fVelo;
	Vector3 veloDir;
	float steerInput = 0;
	float accelOrBrakeKey;
	float newSteerInput;
	float accelSpeed = 40f;
	float autoSteeringAngle;

	void Start () {
		rb = GetComponent<Rigidbody>();
		if (centerOfMass != null)
			GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
		GetComponent<Rigidbody>().inertiaTensor *= inertiaFactor;
		drivetrain = GetComponent (typeof (Drivetrain)) as Drivetrain;
		autoSteeringAngle = DataKeeper.dk.autoSteeringAngle;
	}
	
	void Update () {
		Steering();
		Throttle();	
	}

	void Throttle(){
		accelOrBrakeKey = Input.GetAxis("Vertical");
		bool accelPedal = accelOrBrakeKey > 0;
		bool brakePedal = accelOrBrakeKey < 0;
		bool accelKey = Input.GetKey (KeyCode.UpArrow);
		bool brakeKey = Input.GetKey (KeyCode.DownArrow);

		if (drivetrain.automatic && drivetrain.gear == 0){
			accelKey = Input.GetKey (KeyCode.DownArrow);
			brakeKey = Input.GetKey (KeyCode.UpArrow);
		}

		if (accelPedal) {
			if (drivetrain.slipRatio < 0.10f){
				throttle = accelSpeed*(accelOrBrakeKey * Time.deltaTime);
			}	
			if (throttleInput < 0)
				throttleInput = 0;
			throttleInput += (accelOrBrakeKey); 
			brake = 0;
		} else if (accelKey) {
			if (drivetrain.slipRatio < 0.10f)
				throttle += Time.deltaTime / throttleTime;
			else if (!tractionControl)
				throttle += Time.deltaTime / throttleTimeTraction;
			else
				throttle -= Time.deltaTime / throttleReleaseTime;
			if (throttleInput < 0)
				throttleInput = 0;
			throttleInput += Time.deltaTime / throttleTime;
			brake = 0;
		} else {
			if (drivetrain.slipRatio < 0.2f){
				throttle -= Time.deltaTime / throttleReleaseTime;
			} else{
				throttle -= Time.deltaTime / throttleReleaseTimeTraction;
			}
		}
		throttle = Mathf.Clamp01 (throttle);
		
		if (brakePedal) {
			if (drivetrain.slipRatio < 0.2f)
				brake = (-accelOrBrakeKey);
			throttleInput += -accelOrBrakeKey;
		} else if (brakeKey) {
			if (drivetrain.slipRatio < 0.2f)
				brake += Time.deltaTime / throttleTime;
			else
				brake += Time.deltaTime / throttleTimeTraction;
			throttle = 0;
			throttleInput -= Time.deltaTime / throttleTime;
		} else {
			if (drivetrain.slipRatio < 0.2f)
				brake -= Time.deltaTime / throttleReleaseTime;
			else
				brake -= Time.deltaTime / throttleReleaseTimeTraction;
		}
			
		brake = Mathf.Clamp01 (brake);
		throttleInput = Mathf.Clamp (throttleInput, -1, 1);
		float shiftThrottleFactor = Mathf.Clamp01((Time.time - lastShiftTime)/shiftSpeed);
		drivetrain.throttle = throttle * shiftThrottleFactor;
		drivetrain.throttleInput = throttleInput;

		foreach(Wheel w in wheels) {
			w.brake = brake;
			w.steering = steering;
		}
	}

	void Steering(){
		carDir = transform.forward;
		fVelo = rb.velocity.magnitude;
		veloDir = rb.velocity * (1/fVelo);
		angle = -Mathf.Asin(Mathf.Clamp( Vector3.Cross(veloDir, carDir).y, -1, 1));
		float optimalSteering = angle / (wheels[0].maxSteeringAngle * Mathf.Deg2Rad);
		if (fVelo < 1)
			optimalSteering = 0;

		if(steeringWheelEnabled){
			if (DataKeeper.dk.autoSteeringEnabled){
				int plusOrMin = DataKeeper.dk.autoSteeringPlusOrMinus;
				newSteerInput = (autoSteeringAngle*plusOrMin)/DataKeeper.dk.degreesOfRotation;
				DataKeeper.dk.steeringInDegrees = newSteerInput*DataKeeper.dk.degreesOfRotation;
				steerInput = newSteerInput;
				
			} else {
				steerInput = 0;
				newSteerInput = Input.GetAxis("Horizontal");
				DataKeeper.dk.steeringInDegrees = newSteerInput*DataKeeper.dk.degreesOfRotation;
				steerInput = newSteerInput;
			}
			steering = steerInput;
		}else{
			steerInput = 0;
			if (Input.GetKey (KeyCode.LeftArrow))
				steerInput = -1;
			if (Input.GetKey (KeyCode.RightArrow))
				steerInput = 1;

			if (steerInput < steering)
			{
				float steerSpeed = (steering>0)?(1/(steerReleaseTime+veloSteerReleaseTime*fVelo)) :(1/(steerTime+veloSteerTime*fVelo));
				if (steering > optimalSteering)
					steerSpeed *= 1 + (steering-optimalSteering) * steerCorrectionFactor;
				steering -= steerSpeed * Time.deltaTime;
				if (steerInput > steering)
					steering = steerInput;
			}
			else if (steerInput > steering)
			{
				float steerSpeed = (steering<0)?(1/(steerReleaseTime+veloSteerReleaseTime*fVelo)) :(1/(steerTime+veloSteerTime*fVelo));
				if (steering < optimalSteering)
					steerSpeed *= 1 + (optimalSteering-steering) * steerCorrectionFactor;
				steering += steerSpeed * Time.deltaTime;
				if (steerInput < steering)
					steering = steerInput;
			}
		}
	}

	public float AccelOrBrakeKey{
		get {return this.accelOrBrakeKey;}
		set { this.accelOrBrakeKey = value;}
	}
}