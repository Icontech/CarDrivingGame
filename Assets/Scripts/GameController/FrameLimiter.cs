using UnityEngine;
using System.Collections;

public class FrameLimiter : MonoBehaviour {
	float frameCount = 0f;
	float dt = 0.0f;
	float updateRate = 4.0f;  // 4 updates per sec.
	float frameCountNew = 0f;
	float startTime = 0f;
	string fpsInfoString;

	void Awake () {
		QualitySettings.vSyncCount = 0;  
		Application.targetFrameRate = 70;
	}

	void Update()
	{
		frameCountNew++;
		frameCount++;
		dt += Time.deltaTime;
		if (dt > 1.0/updateRate)
		{
			frameCount = 0;
			dt -= 1.0f/updateRate;
		}

		if (Input.GetKeyDown(KeyCode.S)){
			float currentTime = Time.time;
			float fpsNew =frameCountNew/(currentTime-startTime);
			fpsInfoString = "Elapsed time: "+(currentTime-startTime)+", FrameCount: "+frameCountNew+", FPS: "+fpsNew;
			startTime = currentTime;
			frameCountNew = 0;
		}
	}

	public string FpsInfoString{
		get { return this.fpsInfoString;}
		set { this.fpsInfoString = value;}
	}
}
