using UnityEngine;
using System.Collections;

public class SpeedDisplay : MonoBehaviour {
	public GameObject go;
	Rigidbody rb; 
	TextMesh tm;
	// Use this for initialization
	void Start () {
		tm = GetComponent<TextMesh>();
		rb = go.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		int speed = (int)(rb.velocity.magnitude * 3.6f);
		tm.text = speed+"";
	}
}
