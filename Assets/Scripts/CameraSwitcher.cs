using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour {
	public Camera fpsCam;
	public Camera followCam;

	void Start(){
		fpsCam.enabled = true;
		followCam.enabled = false;
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.X)){
			fpsCam.enabled = !fpsCam.enabled;
			followCam.enabled = !followCam.enabled;
		}
	}
}
