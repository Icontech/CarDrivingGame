using UnityEngine;
using System.Collections;

public class CarPlacer : MonoBehaviour {
	public GameObject car;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1))
			MoveCar(1982f,-1683f,0);
		if (Input.GetKeyDown(KeyCode.Alpha2))
			MoveCar(2547.99f,-274f,0);
		if (Input.GetKeyDown(KeyCode.Alpha3))
			MoveCar(3480.1f, 913.5f, 90);
		if (Input.GetKeyDown(KeyCode.Alpha4))
			MoveCar(3564.1f, -1051.4f, 180);
		if (Input.GetKeyDown(KeyCode.Alpha5))
			MoveCar(3354.3f, -2286.6f, 270);
	}

	void MoveCar(float x, float z, int rot){
		car.transform.position = new Vector3(x,0.0f,z);
		car.transform.rotation = Quaternion.AngleAxis(rot, Vector3.up);
	}
}
