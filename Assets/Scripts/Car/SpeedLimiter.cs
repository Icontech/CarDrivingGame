using UnityEngine;
using System.Collections;

public class SpeedLimiter : MonoBehaviour {
	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate () {
		if ((rb.velocity.magnitude * 3.6f) >= DataKeeper.dk.maxSpeed || DataKeeper.dk.autoSteeringEnabled){
			rb.velocity = rb.velocity.normalized * DataKeeper.dk.maxSpeed/3.6f;
		}
	}
}
