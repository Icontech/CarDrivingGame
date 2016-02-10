using UnityEngine;
using System.Collections;

public class DistanceDisplay : MonoBehaviour {
	public GameObject go;
	Rigidbody rb; 
	TextMesh tm;
	float totalDistance;
	// Use this for initialization
	void Start () {
		tm = GetComponent<TextMesh>();
		rb = go.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		float speed = (rb.velocity.magnitude * 3.6f);
		float timeInHours = (Time.smoothDeltaTime/60)/60;
		totalDistance += speed * timeInHours;
		tm.text = totalDistance.ToString("F2");
	}
}
